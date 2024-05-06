using System;
using System.Collections.Generic;
using H2910.Defines;
using Honeti;
using SuperScrollView;

namespace H2910.PlayerData.Models
{
    public class PlayerPropData
    {
        public int Gold;
        public string PlayerName;
        public int Version;
        public LanguageCode LanguageType;
    }
    

    public class PackData
    {
        public int TotalBoughtCount;
        public DateTime TimeBought;
        public bool IsActive;
        public bool CanShowIcon;
        public DateTime DateDontShowAgain;
    }
    
    public class PlayerPackData
    {
        public Dictionary<IAPType, PackData> DictPackData = new Dictionary<IAPType, PackData>();
        public DateTime? FirstTimeLoginInternet;
    }

    public class DailyGiftData
    {
        public Dictionary<DailyGiftType, DateTime> DictClaimGift = new Dictionary<DailyGiftType, DateTime>();
    }

    public class MailData
    {
        public int MailIndex;
        public DateTime TimeGetMail;
        public bool IsCollected;
        public bool IsRead;
        public string Content;
        public ItemData Rewards = new ItemData();
        public TimeSpan TimeCountDown;
    }

    public class PlayerMailData
    {
        public List<MailData> Mails = new List<MailData>();
        public bool HasNewMail;
        public int MailIndex;
    }

    public class LuckyWheelData
    {
        public int AdsRemainToWatch = 10;
        public int AdsCount;
        public bool FreeSpin;
        public DateTime LastDayReward;
    }
    
    public class PlayerSettingData
    {
        public float BGMValue;
        public float SFXValue;
        public InputType InputType;
        public MainTutorialType TutorialType = MainTutorialType.None;
    }
    
    public class DailyRewardDataInfo
    {
        public List<DailyRewardData> ListDailyRewardData = new List<DailyRewardData>();
        public DateTime LastLoginDay;
        public DateTime? TimeActiveTelekisness;
        public int Count;
        public bool HadShowInDay;
    }
    [System.Serializable]
    public class DailyRewardData
    {
        public int day;
        public bool logined;
        public bool claimed;
    }

    public class QuestData
    {
        public List<string> ListCompleteQuest = new List<string>();
        public Dictionary<string, QuestProgressData> DictActiveQuest = new Dictionary<string, QuestProgressData>();
        public bool IsCompleteMainTutorial;
        public long LongestTimeFinishQuest;
        public int MainQuestFinishCount;
        public int SideQuestFinishCount;
        public string IdQuestNearestFinish;
        public string IdQuestLongestTimeFinish;
        public DateTime? TimeResetDailyQuest;
    }

    public class QuestProgressData
    {
        public string QuestId;
        public int CurrentStep;
        public int LastCacheStep;
        public string CurrentValue;
        public string TargetValue;
        public bool IsAccepted;
        public bool IsComplete;
        //public QuestStatus QuestStatus;
        public DateTime? TimeAcceptQuest;
        public long TimeQuestInprogress;
        public DateTime? LastUpdateQuestInProgressTime;
    }
    
}