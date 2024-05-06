using System;
using H2910.Defines;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSetting : MonoBehaviour
{
    [SerializeField] private SettingAttribute settingAttribute;
    [SerializeField] private Sprite muteSprite;
    [SerializeField] private Sprite unMuteSprite;
    private PopupSetting _popupSettingManager;
    private Image _btImage;
    private bool _isMute;

    private void Start()
    {
        if (_btImage == null)
            _btImage = GetComponent<Image>();
    }

    public void Init(PopupSetting popupSetting)
    {
        if (_btImage == null)
            _btImage = GetComponent<Image>();
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
        if (value > 0)
        {
            _isMute = false;
            _btImage.sprite = unMuteSprite;
        }
        else
        {
            _isMute = true;
            _btImage.sprite = muteSprite;
        }
    }

    public void SetValue()
    {
        if(_isMute)
            ChangeVolume(1);
        else
            ChangeVolume(0);
    }

    private void ChangeVolume(float value)
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