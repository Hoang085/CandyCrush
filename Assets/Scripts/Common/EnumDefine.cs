using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace H2910.Defines
{
    public enum ParentPopup
    {
        Default,
        Hight
    }

    public enum PopupShowingType
    {
        FadeInOut,
        NotFade
    }

    public enum MainTutorialType
    {
        None = 0,
        Default = 1,
        NewTutorial = 2,
        NoTutorial = 3
    }

    public enum NoticeColor
    {
        Red,
        White
    }

    public enum PopupName
    {
        None, 
        Inventory,
        Gate,
        Notice,
        Exchange,
        GetItem,
        Victory,
        QuestPopup,
        Menu,
        QuestBoard,
        Setting,
        RateUs,
        LuckyWheel,
        Garden,
        Pack,
        PackShop,
        RewardPack,
        Achievement,
        DailyReward,
        ConfirmHealing,
        Tutorial,
        Mail,
        Rebirth,
        Defeat,
        Dialog,
        ShopAll,
        ChangeName,
        Login,
        VoiceOfWisdom,
        SelectData,
        NoticePlayOffline,
        ConfirmUseItem
    }

    public enum LocalNotiType
    {
        None =0,
        DailyLoginReward =1,
        Test =99
    }

    public enum DailyGiftType
    {
        GoldFree, 
        GoldAds,
        GemFree,
        GemAds,
        MaterialFree,
        MaterialAds
    }
    public enum IAPType
    {
        Gem1,
        Gem2,
        Gem3,
        Gem4,
        Gem5,
        Gem6,
        RemoveAds,
        PermanentRemoveAds,
        TrialRemoveAds,
        Support1,
        Support2,
        Support3,
        Support4,
        Support5,
    }
    public enum Notice
    {
        News,
        Quest,
        LuckyWheelFree,
        DailyReward,
        OnlineReward,
        Achievement,
        Mail,
        DailyGiftGold,
        DailyGiftGem,
    }

    public enum NoticeIconType
    {
        New = 0,
        Warn = 1,
    }
    
    public enum InputType
    {
        None=0,
        Button=1,
        Joystick=2,
    }
    
    public enum SettingAttribute 
    { 
        None,
        BGM,
        Sound
    }
    public enum ModelKey
    {
        PlayerProp,
        MapInfo,
        Quest,
        AdventureShop,
        LuckyWheel,
        Pack,
        Mail,
        DailyAndOnlineReward,
        PlayerSettings,
        DailyGift,
        SubData2,
        SubData,
    }
    public enum ResourceType
    {
        Gold,
        Gem,
        Ads
    }
    
}