using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using H2910.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using H2910.UI.Popups;
using H2910.Defines;


public class HomeController : MonoBehaviour
{
    public static HomeController Instance;
    [SerializeField] private TextMeshProUGUI txtDisplayName;
    [SerializeField] private Button dailyRewardBtn;
    private Tween _tween;
    private bool _onClick;
    private UIHomePanel[] _panels;

    private void Awake()
    {
        _panels = GetComponentsInChildren<UIHomePanel>();
        Instance = this;
    }

    /*private void Start()
    {
        OnShowScreen();
        #region Tutorial Triger
        PlayerData.Instance.PlayerDailyReward.CheckResetHadShowInDay(System.DateTime.UtcNow);
        if (!PlayerData.Instance.PlayerDailyReward.HadShowInDay && !PlayerData.Instance.PlayerDailyReward.CheckHaveReceivedAllReward())
        {
            PlayerData.Instance.PlayerDailyReward.SetHadShowInDay(true);
            PopupManager.Instance.OnShowScreen(PopupName.DailyReward);
        }
        else if(PlayerData.Instance.PlayerProp.GetTotalSkill() == 0&& PlayerData.Instance.PlayerProp.IsUnlockFeature(UnlockFeature.CompleteTutorial3A))
            TutorialManager.Instance.ShowStepAfterWait("StartTut3", 1f);
        else if (!PlayerData.Instance.PlayerProp.IsUnlockFeature(UnlockFeature.CompleteTutorial12Ex) && AchievementManager.Instance.CheckKilledBossZone(1) && PlayerData.Instance.PlayerMapInfo.GetHighestLevel() < 200)// not go zone 2 yet
        {
            PopupManager.Instance.OnShowScreen(PopupName.Dialog,
              new PopupDialog.Param
              {
                  QuesId = "T12-EX",
                  NeedLoadResouces = true,
                  CallBack = () =>
                  {
                      PopupManager.Instance.OnShowScreen(PopupName.Gate, new PopupGate.Param { ZoneShowUnlock = MapType.FishingVillage });
                  }
              },
              ParentPopup.Hight);
        }
        else if (!PlayerData.Instance.PlayerProp.IsUnlockFeature(UnlockFeature.CompleteTutorial13Ex) && AchievementManager.Instance.CheckKilledBossZone(2) && PlayerData.Instance.PlayerMapInfo.GetHighestLevel() < 300)// not go zone 3 yet
        {
            PopupManager.Instance.OnShowScreen(PopupName.Dialog,
              new PopupDialog.Param
              {
                  QuesId = "T13-EX",
                  NeedLoadResouces = true,
                  CallBack = () =>
                  {
                      PopupManager.Instance.OnShowScreen(PopupName.Gate, new PopupGate.Param { ZoneShowUnlock = MapType.Forest });
                  }
              },
              ParentPopup.Hight);
        }
        
        else if (PlayerData.Instance.PlayerMapInfo.MapFinishCheck("z1map12") && !PlayerData.Instance.PlayerQuest.IsCompleteOrActiceQuest("Q5")&& PlayerData.Instance.PlayerMapInfo.GetHighestLevel() < 200)
        {
            TutorialManager.Instance.ShowStep("OpenGate");
            PlayerData.Instance.PlayerProp.UnlockFeature(UnlockFeature.CompleteTutorial12);
        }
        else if (PlayerData.Instance.PlayerMapInfo.MapFinishCheck("z2map12") && !PlayerData.Instance.PlayerQuest.IsCompleteOrActiceQuest("Q6")&& PlayerData.Instance.PlayerMapInfo.GetHighestLevel() < 300)
        {
            TutorialManager.Instance.ShowStep("OpenGate");
            PlayerData.Instance.PlayerProp.UnlockFeature(UnlockFeature.CompleteTutorial13);
        }
        else if (PlayerData.Instance.PlayerMapInfo.MapFinishCheck("z3map12") && !PlayerData.Instance.PlayerQuest.IsCompleteOrActiceQuest("Q7") && PlayerData.Instance.PlayerMapInfo.GetHighestLevel() < 400)
        {
            TutorialManager.Instance.ShowStep("OpenGate");
            PlayerData.Instance.PlayerProp.UnlockFeature(UnlockFeature.CompleteTutorial14);
        }
        //
        else if (!PlayerData.Instance.PlayerProp.IsUnlockFeature(UnlockFeature.CompleteTutorial4))
        {
            //triger go out map
            PopupManager.Instance.OnShowScreen(PopupName.Dialog,
              new PopupDialog.Param
              {
                  QuesId = "T4",
                  NeedLoadResouces = true,
                  CallBack = () => {
                      TutorialManager.Instance.ShowStep("GoOutMap");
                  }
              },
              ParentPopup.Hight);
        }
        else if (PlayerData.Instance.PlayerProp.IsUnlockFeature(UnlockFeature.CraftBronzeShuriken) && !PlayerData.Instance.PlayerProp.IsUnlockFeature(UnlockFeature.CompleteTutorial7)&& PlayerData.Instance.Inventory.CanCraftShurikenTutorial())
        {
            PopupManager.Instance.OnShowScreen(PopupName.Dialog,
              new PopupDialog.Param
              {
                  QuesId = "T7",
                  NeedLoadResouces = true,
                  CallBack = () => {
                      TutorialManager.Instance.ShowStep("OpenCraftShuriken");
                  }
              },
              ParentPopup.Hight);
        }
        else if(PlayerData.Instance.PlayerProp.IsUnlockFeature(UnlockFeature.CraftPoisonGrease) && !PlayerData.Instance.PlayerProp.IsUnlockFeature(UnlockFeature.CompleteTutorial9))
        {
            PopupManager.Instance.OnShowScreen(PopupName.Dialog,
              new PopupDialog.Param
              {
                  QuesId = "T9",
                  NeedLoadResouces = true,
                  CallBack = () => 
                  {
                      TutorialManager.Instance.ShowStep("OpenCraftGrease");
                  }
              },
              ParentPopup.Hight);
        }
        else
        {
            IAPManager.Instance.ShowPackWhenBackToVillage();
        }
#endregion
        QuestManager.Instance.OnCollectedFlame();
        PlayerData.Instance.PlayerDailyReward.CheckActiveTelekisness();
        if (DailyRewardManager.Instance != null)
        {
            DailyRewardManager.Instance.TriggerNotice();
        }

        if (QuestManager.Instance.DictQuestProgress.Count > 0)
            NoticeManager.Instance.TriggerEvent(Notice.Quest, true);

        // check reset free roll
        if (UpdateManager.Instance.HasInternetTime)
        {
            var nowDay = UpdateManager.Instance.InternetTime;
            if (PlayerData.Instance.PlayerLuckyWheel.LastDayReward.Date != nowDay.Date)
            {
                PlayerData.Instance.PlayerLuckyWheel.ResetPerDay();
            }
            if (PlayerData.Instance.PlayerLuckyWheel.FreeRoll)
            {
                if (PlayerData.Instance.PlayerMapInfo.MapFinishCheck(new MapInfo(MapType.Suburban, 10)))
                {
                    NoticeManager.Instance.TriggerEvent(Notice.LuckyWheelFree, true);
                }
            }
        }
    }*/

    public void OnChangeDisplayName(string displayName)
    {
        txtDisplayName.text = displayName;
    }    

    private void OnDestroy()
    {
        if (Instance != null)
            Instance = null;
        OnCloseScreen();
    }

    public void OnShowScreen()
    {
        _tween?.Kill();
        _onClick = false;
        foreach (var item in _panels)
        {
            item.OnShow();
        }
    }

    public void HideAllPanel()
    {
        foreach (var item in _panels)
        {
            item.OnHide();
        }
    }    

    private void UpdateOnlineRewardTimerTxt()
    {
        //Note: Lock Daily reward
        return;
    }

    public void OnCloseScreen()
    {
        //UpdateManager.Instance.OnUpdateSecond -= UpdateOnlineRewardTimerTxt;
    }

    public void OnBtnNewClick()
    {
        if (_onClick)
            return;
        BlockMultyClick();
        //AnalyticsManager.Instance.LogEventMenuClick("Mail");
        PopupManager.Instance.OnShowScreen(PopupName.Mail);
        HideAllPanel();
    }

    private void BlockMultyClick()
    {
        _onClick = true;
        DOVirtual.DelayedCall(0.1f, () => { _onClick = false; });
    }    
    
    public void SetBlockClick(bool isBlock)
    {
        _onClick = isBlock;
    }
    
    public void OnBtnSettingClick()
    {
        if (_onClick)
            return;
        BlockMultyClick();
        //AnalyticsManager.Instance.LogEventMenuClick("Setting");
        PopupManager.Instance.OnShowScreen(PopupName.Setting, ParentPopup.Hight);
        HideAllPanel();
    }

    /*public void OnBtnLuckyWheelClick()
    {
        if (OnClick)
            return;
        BlockMultyClick();
        HideAllPanel();
        AnalyticsManager.Instance.LogEventMenuClick("Lucky wheel");
        if (!PlayerData.Instance.PlayerMapInfo.MapFinishCheck(new MapInfo(MapType.Suburban, 10)))
        {
            PopupManager.Instance.OnShowScreen(PopupName.Notice,
                new PopupNotice.Param { Content = string.Format(LanguageUtils.GetLanguageValue(LanguageDefine.CompleteMap), LanguageUtils.GetLanguageValue(MapType.Suburban.ToString()) + " 10 "), NoticeType = NoticeType.LinkTo },
            ParentPopup.Hight);
            return;
        }
        PopupManager.Instance.OnShowScreen(PopupName.LuckyWheel);
    }   */
    
    public void OnBtnPlayMapClick()
    {
        if (_onClick)
            return;
        BlockMultyClick();
        HideAllPanel();
        //AnalyticsManager.Instance.LogEventMenuClick("Warp");

        PopupManager.Instance.OnShowScreen(PopupName.Gate);
    }
    
    public void OnBtnShop()
    {
        if (_onClick)
            return;
        BlockMultyClick();
        HideAllPanel();
        //AnalyticsManager.Instance.LogEventMenuClick("Shop");
        PopupManager.Instance.OnShowScreen(PopupName.ShopAll);
    }

    public void OnBtnDailyRewardClick()
    {
        if (_onClick)
            return;
        BlockMultyClick();
        /*if (!UpdateManager.Instance.HasInternetTime)
        {
            PopupManager.Instance.ShowNotice(LanguageUtils.GetLanguageValue(LanguageDefine.InternetCheck));
            return;
        }
        HideAllPanel();
        AnalyticsManager.Instance.LogEventMenuClick("DailyReward");*/
        PopupManager.Instance.OnShowScreen(PopupName.DailyReward);
    }

    public void OnBtnNameClick()
    {
        if (_onClick)
            return;
        BlockMultyClick();
        HideAllPanel();
        //AnalyticsManager.Instance.LogEventMenuClick("ChangeName");
        PopupManager.Instance.OnShowScreen(PopupName.ChangeName);
    }
    public GameObject GetButtonDailyReward()
    {
        return dailyRewardBtn.gameObject;
    }
    public GameObject GetButtonMap()
    {
        //return mapBtn.gameObject;
        return null;
    }
}
