using System;

namespace CORE
{
    [Serializable]
    public struct LanguageSetting
    {
        public string Language;
        public Translation[] Translations;
    }
}