using System;
using System.Collections.Generic;
using H2910.Data;
using Honeti;
using TMPro;
using UnityEditor;
using UnityEngine;

public class ChooseLanguage : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    private List<string> _listLanguage = new List<string>();

    private void Start()
    {
        var languages = Enum.GetValues(typeof(LanguageCode));
        foreach (var language in languages)
        {
            var enumLanguage = (LanguageCode)language;
            switch (enumLanguage)
            {
                case LanguageCode.EN:
                    _listLanguage.Add("English");
                    break;
                case LanguageCode.VN:
                    _listLanguage.Add("Tiếng Việt");
                    break;
            }
        }

        LanguageCode languageCode = PlayerData.Instance.PlayerProp.LanguageType;
        dropdown.AddOptions(_listLanguage);
        dropdown.value = (int)languageCode;
        dropdown.onValueChanged.AddListener(OnDropdownChange);
    }

    private void OnDropdownChange(int value)
    {
        if (Enum.IsDefined(typeof(LanguageCode), value) == true)
        {
            I18N.Instance.setLanguage((LanguageCode)value);
            PlayerData.Instance.PlayerProp.SetLanguageCode((LanguageCode)value);
        }
        else
        {
            return;
        }
    }
}