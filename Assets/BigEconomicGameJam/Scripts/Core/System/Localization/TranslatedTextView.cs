using CORE.CONST_SELECTOR;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace CORE
{
    [RequireComponent(typeof(TMP_Text))]
    public class TranslatedTextView: MonoBehaviour
    {
        [SerializeField, Required] private TMP_Text _textView;
        [SerializeField, Constant("LocalizationKeys")] private string _key;

        public string Key => _key;
        public TMP_Text TextView => _textView;
        
        private void Start()
        {
            LocalizationService.RegisterView(this);
        }

        public void SetKey(string key)
        {
            _key = key;
            _textView.text = ServiceLocator.GetService<LocalizationService>().GetTranslation(key);
        }
        
        private void OnDestroy()
        {
            LocalizationService.UnRegisterView(this);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_textView == null)
                _textView = GetComponent<TMP_Text>();
        }
#endif
    }
}