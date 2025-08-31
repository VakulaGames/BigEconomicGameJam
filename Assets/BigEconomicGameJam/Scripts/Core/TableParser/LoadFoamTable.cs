using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace CORE
{
    public abstract class LoadFoamTable<T>: ScriptableObject
    {
#if UNITY_EDITOR
        [field: SerializeField] protected string Url { get; private set; }

        protected CancellationTokenSource LoadDataCancellationTokenSource { get; set; }

        protected bool LoadButtonBlocked { get; set; }


        [Button, HideIf(nameof(LoadButtonBlocked))]
        protected abstract UniTaskVoid LoadTableAsyncEditor();
        
        [Button, ShowIf(nameof(LoadButtonBlocked))]
        protected virtual void StopLoadingProcess()
        {
            LoadDataCancellationTokenSource?.Cancel();
            LoadButtonBlocked = false;
        }

        protected virtual async UniTask<List<T>> LoadToJSONConfig(string url)
        {
            LoadDataCancellationTokenSource?.Cancel();
            LoadDataCancellationTokenSource = new CancellationTokenSource();

            UnityWebRequest webRequest = UnityWebRequest.Get(url);
            await webRequest.SendWebRequest().ToUniTask(cancellationToken: LoadDataCancellationTokenSource.Token);

            if(webRequest.result == UnityWebRequest.Result.ConnectionError
               || webRequest.result == UnityWebRequest.Result.ProtocolError
               || LoadDataCancellationTokenSource.IsCancellationRequested)
            {
                Debug.LogError("Error: " + webRequest.error);
                return null;
            }

            Debug.Log("Table load successful");

            string csv = webRequest.downloadHandler.text;
            return CsvToConvertStruct(csv);
        }

        protected List<T> CsvToConvertStruct(string value = null, string splitSymbol = ",")
        {
            string[] lines = value.Split("\r\n");

            List<string> csvLinesList = lines.ToList();

            string[] tableHeaders = csvLinesList[0].Split(splitSymbol);

            List<T> objectList = new List<T>();

            for(int i = 1; i < csvLinesList.Count; i++)
            {
                string[] thisLineSplit = csvLinesList[i].Split(splitSymbol);

                IEnumerable<KeyValuePair<string, string>> pairedWithHeader =
                    tableHeaders.Zip(thisLineSplit,
                        (headerName, valueCell) =>  {
                            valueCell = string.IsNullOrWhiteSpace(valueCell) ? "none" : valueCell;
                            return new KeyValuePair<string, string>(headerName, valueCell);
                        });

                JObject jsonObject = 
                    new JObject(Enumerable.Select(pairedWithHeader, jsonRecord => new JProperty(jsonRecord.Key, jsonRecord.Value)));
                
                objectList.Add(DeserializeJSON(jsonObject, objectList));
            }

            return objectList;
        }

        protected virtual T DeserializeJSON(JObject jsonObject, List<T> objectList = null)
        {
            return JsonConvert.DeserializeObject<T>(jsonObject.ToString());
        }

#endif
    }
}