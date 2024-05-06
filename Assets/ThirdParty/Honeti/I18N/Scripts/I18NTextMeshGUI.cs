using UnityEngine;
using System.Collections;
using TMPro;

namespace Honeti
{
    public class I18NTextMeshGUI : MonoBehaviour
    {
        private string _key = "";
        [SerializeField]
        private string m_key = "";
        private TextMeshProUGUI _text;
        private bool _initialized = false;
        private bool _isValidKey = false;
        private TMP_FontAsset _defaultFont;
        private float _defaultLineSpacing;
        private int _defaultFontSize;
        private TextAlignmentOptions _defaultAlignment;

        [SerializeField]
        private bool _dontOverwrite = false;

        [SerializeField]
        private string[] _params;

        private void OnEnable()
        {
            if (!_initialized)
            {
                _init();
            }
            updateTranslation();
        }

        private void OnDestroy()
        {
            if (_initialized)
            {
                I18N.OnLanguageChanged -= _onLanguageChanged;
                I18N.OnFontChanged -= _onFontChanged;
                I18N.OnInitComplete -= OnInitComplte;
            }
        }

        /// <summary>
        /// Change text in Text component.
        /// </summary>
        private void _updateTranslation()
        {
            if (_text)
            {
                if (!_isValidKey)
                {
                    _key = m_key;//_text.text;

                    _isValidKey = true;
                }
                if(I18N.Instance != null && I18N.Instance.IsInitComplete)
                    _text.text = I18N.Instance.getValue(_key, _params);
            }
        }

        /// <summary>
        /// Update translation text.
        /// </summary>
        /// <param name="invalidateKey">Force to invalidate current translation key</param>
        public void updateTranslation(bool invalidateKey = false)
        {
            if (invalidateKey)
            {
                _isValidKey = false;
            }

            _updateTranslation();
        }

        /// <summary>
        /// Init component.
        /// </summary>
        private void _init()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _key = m_key; //_text.text;
            _initialized = true;
            I18N.OnLanguageChanged += _onLanguageChanged;
            I18N.OnFontChanged += _onFontChanged;
            I18N.OnInitComplete += OnInitComplte;
            if (I18N.Instance && I18N.Instance.useCustomFonts)
            {
                _changeFont(I18N.Instance.customFont);
            }
            _isValidKey = true;

            if (!_text)
            {
                Debug.LogWarning(string.Format("{0}: Text component was not found!", this));
            }
        }

        private void _onLanguageChanged(LanguageCode newLang)
        {
            _updateTranslation();
        }

        private void _onFontChanged(I18NFonts newFont)
        {
            _changeFont(newFont);
        }

        private void OnInitComplte()
        {
            _updateTranslation();
        }

        private void _changeFont(I18NFonts f)
        {
            if (_dontOverwrite)
            {
                return;
            }

            if (f != null)
            {
                if (f.fontTMP)
                {
                    _text.font = f.fontTMP;
                }
                else
                {
                    _text.font = _defaultFont;
                }
                if (f.customLineSpacing)
                {
                    _text.lineSpacing = f.lineSpacing;
                }
                if (f.customFontSizeOffset)
                {
                    _text.fontSize = (int) (_defaultFontSize + _defaultFontSize * f.fontSizeOffsetPercent / 100);
                }
                if (f.customAlignment)
                {
                    _text.alignment = f.alignmentTMP;
                }
            }
            else
            {
                _text.font = _defaultFont;
                _text.lineSpacing = _defaultLineSpacing;
                _text.fontSize = _defaultFontSize;
                _text.alignment = _defaultAlignment;
            }
        }
    }
}