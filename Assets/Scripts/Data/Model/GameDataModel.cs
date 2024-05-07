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
    
    public class PlayerSettingData
    {
        public float BGMValue;
        public float SFXValue;
        public InputType InputType;
        public MainTutorialType TutorialType = MainTutorialType.None;
    }
    
}