using System;
using H2910.Defines;
using UnityEngine;
using UnityEngine.UI;

public class SliderSetting : MonoBehaviour
{
    [SerializeField] private SettingAttribute settingAttribute;
    private PopupSetting _popupSettingManager;
    private Slider _slider;
    private float _volume;

    private void Start()
    {
        if (_slider == null)
            _slider = GetComponent<Slider>();
    }

    public void Init(PopupSetting popupSetting)
    {
        if (_slider == null)
            _slider = GetComponent<Slider>();
        _popupSettingManager = popupSetting;
        if (settingAttribute == SettingAttribute.BGM)
        {
            _popupSettingManager.BgmChange += OnChangeValue;
        }else if (settingAttribute == SettingAttribute.Sound)
        {
            _popupSettingManager.SoundChange += OnChangeValue;
        }
    }

    private void OnChangeValue(float value)
    {
        _slider.value = value;
    }

    public void SetValue()
    {
        _volume = _slider.value;
        ChangeVolume(_volume);
    }

    public void ChangeVolume(float value)
    {
        if (settingAttribute == SettingAttribute.BGM)
        {
            _popupSettingManager.SetBgmVolume(value);
        }else if (settingAttribute == SettingAttribute.Sound)
        {
            _popupSettingManager.SetSoundVolume(value);
        }
    }

    private void OnDisable()
    {
        if (_popupSettingManager != null)
        {
            if (settingAttribute == SettingAttribute.BGM)
            {
                _popupSettingManager.BgmChange -= OnChangeValue;
            }else if (settingAttribute == SettingAttribute.Sound)
            {
                _popupSettingManager.SoundChange -= OnChangeValue;
            }
        }
    }
}