using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using H2910.Base;
using H2910.Data;
using H2910.Defines;
using H2910.UI.Popups;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupSetting : BasePopUp
{
    [SerializeField] private string termsOfServiceUrl;
    [SerializeField] private string privacyPolicyUrl;
    [SerializeField] private ButtonSetting[] buttonSettings;
    [SerializeField] private SliderSetting[] sliderSettings;
    [SerializeField] private TextMeshProUGUI textVersion;
    [SerializeField] private TextMeshProUGUI txtUserId;
    [SerializeField] private GameObject commingSoonObj;
    [SerializeField] TextMeshProUGUI txtUserNameGoogle;
    [SerializeField] Button btnGoogleInSetting;
    [SerializeField] Button btnFaceBookInSetting;

    public event Action<float> BgmChange;
    public event Action<float> SoundChange;
    private float bgmVolume;
    private float soundVolume;
    private Tween tween;

    public override void OnShowScreen()
    {
        base.OnShowScreen();
        bgmVolume = PlayerData.Instance.PlayerSetting.BGMValue;
        soundVolume = PlayerData.Instance.PlayerSetting.SFXValue;
        foreach (var bt in buttonSettings)
        {
            bt.Init(this);
        }
        foreach (var sl in sliderSettings)
        {
            sl.Init(this);
        }
        SetBgmVolume(bgmVolume);
        SetSoundVolume(soundVolume);
        textVersion.text = Application.version.ToString();
        txtUserId.text = string.Empty;
        tween?.Kill();
        commingSoonObj.SetActive(false);
    }
    public void SetBgmVolume(float value)
    {
        bgmVolume = value;
        PlayerData.Instance.PlayerSetting.SaveBGMValue(bgmVolume);
        SoundManager.Instance.GlobalVMusicVolume = bgmVolume;
        BgmChange?.Invoke(bgmVolume);
    }
    public void SetSoundVolume(float value)
    {
        soundVolume = value;
        PlayerData.Instance.PlayerSetting.SaveSFXValue(soundVolume);
        SoundManager.Instance.GlobalSoundsVolume = soundVolume;
        SoundChange?.Invoke(soundVolume);
    }

    public void TermsOfService()
    {
        if (OnClick)
            return;
        BlockMultyClick();
        Application.OpenURL(termsOfServiceUrl);
    } 
    
    public void PrivacyPolicy()
    {
        if (OnClick)
            return;
        BlockMultyClick();
        Application.OpenURL(privacyPolicyUrl);
    }
    public void Close()
    {
        if (OnClick)
            return;
        BlockMultyClick();
        OnCloseScreen();
        tween?.Kill();
    }
    public override void OnCloseScreen()
    {
        base.OnCloseScreen();
    }

    public void OnBtnGoogleClick()
    {
        if (OnClick)
            return;
        BlockMultyClick();
        PopupManager.Instance.ShowNotice(LanguageUtils.GetLanguageValue(LanguageDefine.COMING_SOON));
    }
    public void OnBtnFacebookClick()
    {
        if (OnClick)
            return;
        BlockMultyClick();
        PopupManager.Instance.ShowNotice(LanguageUtils.GetLanguageValue(LanguageDefine.COMING_SOON));
    }


}