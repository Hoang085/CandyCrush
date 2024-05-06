using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using H2910.Utils;
using System.Xml;
using System.Threading.Tasks;
using H2910.Common.Singleton;
using H2910.Data;

namespace Honeti
{
    /// <summary>
    /// I18N Language code enum.
    /// </summary>
    public enum LanguageCode
    {
        /// <summary>
        /// English
        /// </summary>
        EN = 0,

        /// <summary>
        /// Vietnamese
        /// </summary>
        VN = 1,
    }

    /// <summary>
    /// Internationalization component.
    /// Use getValue() to translate text.
    /// Use setLanguage() to change current application language.
    /// All translations are in _langs variable.
    /// </summary>
    public class I18N : ManualSingletonMono<I18N>
    {
        #region STATIC

        /// <summary>
        /// Default language.
        /// </summary>
        private static LanguageCode _defaultLang = LanguageCode.EN;

        private static I18N _instance = null;

        /// <summary>
        /// I18N components instance.
        /// </summary>

        #endregion

        #region EVENTS

        public delegate void LanguageChange(LanguageCode newLanguage);

        public static event LanguageChange OnLanguageChanged;

        public delegate void FontChange(I18NFonts newFont);

        public static event FontChange OnFontChanged;
        public static event Action OnInitComplete;

        #endregion

        #region PRIVATE VARS

        /// <summary>
        /// Language table.
        /// </summary>
        private Hashtable _langs = new Hashtable();

        /// <summary>
        /// Current game language. Using getValue() will translate to that language.
        /// </summary>
        [SerializeField]
        private LanguageCode _gameLang = LanguageCode.EN;

        /// <summary>
        /// Returned text when there is no translation.
        /// </summary>
        private string _noTranslationText = "Translation missing for {0}";

        /// <summary>
        /// Language file asset.
        /// Expected file is tsv format (tab-separated values) 
        /// but saved as cvs (Unity does not accept tsv files as TextAssets).
        /// 
        /// Structure:
        /// langKey EN  PL  [other langs]
        /// ^exampleKey translationEN1  translationPL1  [other langs]
        /// ...
        /// 
        /// LangKey column has to be the first column.
        /// 
        /// Language key has to start with '^' character.
        /// If line does not start with '^' it is not parsed.

        /// <summary>
        /// List of available languages, parsed from language file.
        /// </summary>
        private List<LanguageCode> _availableLangs;

        /// <summary>
        /// When true, I18NText controls will change font for different languages.
        /// Fonts will be selected from _langFonts list. When there is no custom
        /// font set fot language, I18N controls will use default font.
        /// </summary>
        [SerializeField]
        private bool _useCustomFonts = false;
        private string _path = "Language";

        public void SetUseCustomFonts(bool isUseCustomFont)
        {
            _useCustomFonts = isUseCustomFont;
        }

        /// <summary>
        /// Current custom font.
        /// </summary>
        private I18NFonts _currentCustomFont;

        /// <summary>
        /// Custom fonts list for different languages.
        /// </summary>
        [SerializeField]
        private List<I18NFonts> _langFonts;

        #endregion

        #region PROPERTIES

        /// <summary>
        /// Current game language
        /// </summary>
        public LanguageCode gameLang => _gameLang;

        /// <summary>
        /// True, when I18N is using custom fonts
        /// </summary>
        public bool useCustomFonts => _useCustomFonts;

        /// <summary>
        /// Return current custom font or null, when I18N 
        /// is not currently using custom fonts.
        /// </summary>
        public I18NFonts customFont
        {
            get
            {
                if (_useCustomFonts)
                {
                    return _currentCustomFont;
                }
                return null;
            }
        }

        public bool IsInitComplete;

        #endregion

        #region PUBLIC METHODS

        /*public void MergeMoreData(string text, bool replace = true)
        {
            Hashtable table = _parseLanguage();
            foreach (DictionaryEntry entry in table)
            {
                Debug.Log("MergeMoreData" + entry.Key);
                if (!_langs.ContainsKey(entry.Key))
                {
                    continue;
                }
                Hashtable keys = entry.Value as Hashtable;
                if (keys == null)
                {
                    continue;
                }
                Hashtable keysBase = _langs[entry.Key] as Hashtable;
                if (keysBase == null)
                {
                    keysBase = new Hashtable();
                    _langs.Add(entry.Key, keysBase);
                }
                foreach (DictionaryEntry key in keys)
                {
                    //Note: Nathan update to skip merge if replace==false
                    if (replace || !keysBase.ContainsKey(key.Key))
                    {
                        keysBase[key.Key] = key.Value;
                    }
                }
            }
        }*/

        public bool HasKey(string key)
        {
            Hashtable lang = _langs[_gameLang] as Hashtable;
            if (lang == null)
            {
                Debug.LogError("Miss languageCode " + _gameLang);
                if (!_langs.ContainsKey(LanguageCode.EN))
                {
                    ReadXmlFromResourcesAndAddToHashtable(LanguageCode.EN);
                }
                if (!_langs.ContainsKey(LanguageCode.EN))
                    return false;
                return (_langs[LanguageCode.EN] as Hashtable).ContainsKey(key);
            }
            if(!lang.ContainsKey(key))
            {
                Debug.LogError($"Miss key {key} of {_gameLang}");
                return (_langs[LanguageCode.EN] as Hashtable).ContainsKey(key);
            }   
            else
                return true;
        }

        public int CountKey(string key)
        {
            int rs = 0;
            Hashtable lang = _langs[_gameLang] as Hashtable;
            if (lang != null)
            {
                foreach (string keyData in lang.Keys)
                {
                    if (keyData.Contains(key))
                    {
                        rs++;
                    }
                }
            }
            return rs;
        }

        /// <summary>
        /// Change current language.
        /// Set default language if not initialized or recognized.
        /// </summary>
        /// <param name="langCode">Language code</param>
        public void setLanguage(string langCode)
        {
            LanguageCode langEnum;
            if (!Enum.TryParse(langCode, out langEnum))
            {
                langEnum = _defaultLang;
            }
            setLanguage(langEnum);
        }

        /// <summary>
        /// Change current language.
        /// Set default if language not initialized or recognized.
        /// </summary>
        /// <param name="langCode">Language code</param>
        public async void setLanguage(LanguageCode langCode)
        {
            if (_langs.ContainsKey(langCode))
            {
                _gameLang = langCode;
            }
            else
            {
                await ReadXmlFromResourcesAndAddToHashtable(langCode);
                if (_langs.ContainsKey(langCode))
                    _gameLang = langCode;
                else
                {
                    _gameLang = _defaultLang;
                    Debug.LogError(string.Format("Language {0} not recognized! Using default language.", langCode));
                }
            }

            if(PlayerData.Instance != null)
                PlayerData.Instance.PlayerProp.SetLanguageCode(langCode);

            if (OnLanguageChanged != null)
            {
                OnLanguageChanged(_gameLang);
            }

            if (_useCustomFonts)
            {
                I18NFonts newFont = null;
                _currentCustomFont = null;
                if (_langFonts != null && _langFonts.Count > 0)
                {
                    foreach (I18NFonts f in _langFonts)
                    {
                        if (f.lang == _gameLang)
                        {
                            newFont = f;
                            _currentCustomFont = f;
                            break;
                        }
                    }
                }

                if (OnFontChanged != null)
                {
                    OnFontChanged(newFont);
                }
            }
            else
            {
                _currentCustomFont = null;
            }
        }

        /// <summary>
        /// Get key value in current language.
        /// </summary>
        /// <param name="key">Translation key. String should start with '^' character</param>
        /// <returns>Translation value</returns>
        public string getValue(string key)
        {
            return getValue(key, null);
        }

        /// <summary>
        /// Get key value in current language with additional params. 
        /// Currently not working.
        /// </summary>
        /// <param name="key">Translation key. String should start with '^' character and can contain params ex. {0} {1}...</param>
        /// <param name="parameters">Additional parameters.</param>
        /// <returns>Translation value</returns>
        public string getValue(string key, params string[] parameters)
        {

            if (string.IsNullOrEmpty(key))
            {
                return "";
            }

            Hashtable lang = _langs[_gameLang] as Hashtable;

            if (lang == null)
            {
                ReadXmlFromResourcesAndAddToHashtable(_gameLang);
                lang = _langs[_gameLang] as Hashtable;
                if (lang == null)
                    lang = _langs[LanguageCode.EN] as Hashtable;
            }
            string val = "";
            if (!lang.ContainsKey(key))
            {
                Debug.LogError($"Miss key {key} of {_gameLang}");
                val = (_langs[LanguageCode.EN] as Hashtable)[key] as string;
            }
            else
                val = lang[key] as string;

            if (val == null || val.Length == 0)
            {
                if (key == "")
                {
                    return "";
                }
                return string.Format(_noTranslationText, key);
            }

            if (parameters != null && parameters.Length > 0)
            {
                return string.Format(val.Replace("\\n", Environment.NewLine), parameters);
            }
            return val.Replace("\\n", Environment.NewLine);
        }

        public async void LoadData(Action callback)
        {
            await _parseLanguage();
            callback?.Invoke();
        }    

        /// <summary>
        /// Initialize component
        /// </summary>
        public void Init()
        {
            try
            {
                setLanguage(PlayerData.Instance.PlayerProp.LanguageType);
            }
            catch
            {
                setLanguage(_defaultLang);
            }
        }

        #endregion
        #region PRIVATE METHODS
        /// <summary>
        /// Parse language file
        /// </summary>
        /// <param name="lang">Language file asset</param>
        /// <returns>Parsed language hashtable.</returns>
        private async Task _parseLanguage()
        {
            var tasks = new List<Task>();
            //foreach (object langCode in Enum.GetValues(typeof(LanguageCode)))
            //{
                Task task = ReadXmlFromResourcesAndAddToHashtable(_defaultLang);
                tasks.Add(task);
            //}
            await Task.WhenAll(tasks);
            IsInitComplete = true;
        }

        private async Task ReadXmlFromResourcesAndAddToHashtable(object languageCode)
        {
            if (_langs.ContainsKey(languageCode))
            {
                return;
            }
            string path = $"Language/language_{languageCode}";
            TextAsset textAsset = Resources.Load<TextAsset>(path);
            if (textAsset != null)
            {
                string xmlContent = textAsset.text;
                Hashtable languageTable = await ParseLanguageFromXmlAsync(xmlContent);
                _langs[languageCode] = languageTable;
            }
            else
            {
                Debug.LogError("Resource not found at path: " + path);
            }
        }

        private Task<Hashtable> ParseLanguageFromXmlAsync(string xmlContent)
        {
            return Task.Run(() =>
            {
                Hashtable table = new Hashtable();

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlContent);

                XmlNodeList stringNodes = xmlDoc.SelectNodes("//string");
                foreach (XmlNode stringNode in stringNodes)
                {
                    string name = stringNode.Attributes["name"].Value;
                    string value = stringNode.InnerText;
                    table[name] = value;
                }

                return table;
            });
        }

        #endregion
    }

    #region HELPER CLASSES

    /// <summary>
    /// Helper class, containing font parameters.
    /// </summary>
    [Serializable]
    public class I18NFonts
    {
        #region PUBLIC VARS

        /// <summary>
        /// Font language code.
        /// </summary>
        public LanguageCode lang;

        /// <summary>
        /// Font
        /// </summary>
        public Font font;

        /// <summary>
        /// Font
        /// </summary>
        public TMPro.TMP_FontAsset fontTMP;

        /// <summary>
        /// True, when components should use custom line spacing.
        /// </summary>
        public bool customLineSpacing = false;

        /// <summary>
        /// Custom line spacing value.
        /// </summary>
        public float lineSpacing = 1.0f;

        /// <summary>
        /// True, when components should use custom font size.
        /// </summary>
        public bool customFontSizeOffset = false;

        /// <summary>
        /// Custom font size offset in percents.
        /// e.g. 55, -10
        /// </summary>
        public int fontSizeOffsetPercent = 0;

        /// <summary>
        /// True, when components should use custom alignment.
        /// </summary>
        public bool customAlignment = false;

        /// <summary>
        /// Custom alignment value.
        /// </summary>
        public TextAlignment alignment = TextAlignment.Left;

        /// <summary>
        /// Custom alignment value.
        /// </summary>
        public TMPro.TextAlignmentOptions alignmentTMP = TMPro.TextAlignmentOptions.MidlineLeft;

        #endregion
    }

    /// <summary>
    /// Helper class, containing sprite parameters.
    /// </summary>
    [Serializable]
    public class I18NSprites
    {
        #region PUBLIC VARS

        /// <summary>
        /// Sprite lang code.
        /// </summary>
        public LanguageCode language;

        /// <summary>
        /// Sprite.
        /// </summary>
        public Sprite image;

        #endregion
    }

    /// <summary>
    /// Helper class, containing sound parameters.
    /// </summary>
    [Serializable]
    public class I18NSounds
    {
        #region PUBLIC VARS

        /// <summary>
        /// Sound language code.
        /// </summary>
        public LanguageCode language;

        /// <summary>
        /// Audio clip.
        /// </summary>
        public AudioClip clip;

        #endregion
    }

    #endregion
}