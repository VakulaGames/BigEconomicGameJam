using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CORE
{
    public class LocalizationService: AbstractMonoService, IInitializable
    {
        [SerializeField] private LocalizationConfig _config;

        private static List<TranslatedTextView> _views = new List<TranslatedTextView>();

        public override Type RegisterType => typeof(LocalizationService);
        public string CurrentLanguage { get; private set; }

        private Dictionary<string, Dictionary<string, string>> _languageSettings = null;

        public void Init()
        {
            _languageSettings = new Dictionary<string, Dictionary<string, string>>();
            CurrentLanguage = "Ru";
            TryFillLangyageDic(CurrentLanguage);
        }

        public static void RegisterView(TranslatedTextView translatedTextView)
        {
            _views.Add(translatedTextView);
        }

        public static void UnRegisterView(TranslatedTextView translatedTextView)
        {
            _views.Remove(translatedTextView);
        }

        public void SetLanguage(string language)
        {
            CurrentLanguage = language;

            if (!_languageSettings.ContainsKey(language))
            {
                if (!TryFillLangyageDic(language)) return;
            }

            TranslateAll();
        }

        public string GetTranslation(string key)
        {
            return _languageSettings[CurrentLanguage][key];
        }

        private bool TryFillLangyageDic(string language)
        {
            LanguageSetting setting = _config.LanguageSettings.FirstOrDefault(x => x.Language == language);

            if (setting.Language == null)
            {
                Debug.LogError($"Language {language} not found in config!");
                return false;
            }

            _languageSettings[language] = new Dictionary<string, string>();

            foreach (var translation in setting.Translations)
            {
                _languageSettings[language][translation.Key] = translation.Text;
            }

            return true;
        }

        private void TranslateAll()
        {
            foreach (var textView in _views)
            {
                textView.TextView.text = _languageSettings[CurrentLanguage][textView.Key];
            }
        }
    }
}