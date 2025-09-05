using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using NaughtyAttributes;
using Cysharp.Threading.Tasks;

namespace CORE
{
    [CreateAssetMenu(fileName = "LocalizationConfig", menuName = "LocalizationConfig")]
    public class LocalizationConfig : ScriptableObject
    {
        [SerializeField] private string URL;
        [SerializeField] private LanguageSetting[] _languageSettings;

        public LanguageSetting[] LanguageSettings => _languageSettings;
        
#if UNITY_EDITOR
        
        [Button("Load from table")]
        private async void Load()
        {
            await DownloadAndParseCSVAsync();
        }

        private async UniTask DownloadAndParseCSVAsync()
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(URL))
            {
                await webRequest.SendWebRequest().ToUniTask();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error downloading CSV: " + webRequest.error);
                }
                else
                {
                    ParseCSV(webRequest.downloadHandler.text);
                }
            }
        }

        private void ParseCSV(string csvText)
        {
            string[] lines = csvText.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length < 2)
            {
                Debug.LogError("CSV file is empty or has no data.");
                return;
            }

            string[] headers = lines[0].Split(',').Select(header => header.Trim()).ToArray();

            string[] languages = headers.Skip(1).ToArray();

            _languageSettings = new LanguageSetting[languages.Length];

            for (int i = 0; i < languages.Length; i++)
            {
                string language = languages[i];

                _languageSettings[i] = new LanguageSetting
                {
                    Language = language,
                    Translations = new Translation[lines.Length - 1]
                };

                for (int j = 1; j < lines.Length; j++)
                {
                    string[] fields = lines[j].Split(',').Select(field => field.Trim()).ToArray();

                    _languageSettings[i].Translations[j - 1] = new Translation
                    {
                        Key = fields[0],
                        Text = fields[i + 1]
                    };
                }
            }

            Debug.Log("Localization settings loaded successfully.");
        }
#endif
    }
}
