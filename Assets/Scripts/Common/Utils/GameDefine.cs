public class GameDefine
{
    public const string SceneName_MainVillage = "MainVillage";
    public const string SceneName_Home = "Home";
    public const string SceneName_PlayMap = "PlayMap";
    public const string PlayerCombatTag = "PlayerCombat";
    public const string PlayerTag = "Player";
    public const string PlayerBullet = "PlayerBullet";
    public const string PlayerDarkBullet = "PlayerDarkBullet";
    public const int MaxCollectItem = 100;
    public const int MaxWeaponItem = 300;
    public const int MaxSlotItem = 100;
    public const float TimeToGather = 0.5f;
    public const int HealingBuyAds = 2;
    //List Doc
    public const float ListDocScale = 1.7f;
    public const float TimeScaleListDoc = 0.1f;
    public const int NumberLiveMap = 3;
    public const int LevelBeginShowInterAds = 104;
    public const int MaxMineIndex = 9;
    //
    public const double CriticalX = 1.5f;
    public const int PixelPerUnit = 32;
    public const int TotalAdsUpgradeSkill = 30;
    public const int TotalAdsInDay = 45;
    public const int TimeBetweenInterAds = 60;
    public const int TimeCooldownShield = 1;
    public const int TimeActveGuardianShield = 10;
    public const int TimeActveMagicShield = 20;
    public const int DayFreeAdsFromFirstLogin = 2;

    public const int PlayFabUpdateDataDelayTime = 120; // second

    public const string MainQuestM2 = "M2";
    public const string MainQuestM3 = "M3";
    public const string MainQuestM4 = "M4";
    public const string MainQuestM5 = "M5";
    public const string MainQuestM6 = "M6";
    public const string MainQuestM7 = "M7";
    public const string MainQuestM8 = "M8";
    public const string MainQuestM9 = "M9";
    public const string MainQuestM10 = "M10";
    public const string MainQuestM11 = "M11";
    public const string MainQuestM12 = "M12";
    public const string MainQuestM13 = "M13";

    #region Notification
    public const int LocalNotiRemindHowManyRepeateTimes = 28; // daily remind 

    public const string LocalNotiChannelId = "NotiChannelId";
    public const string LocalNotiChannelName = "Channel";
    public const string LocalNotiChannelDesc = "Channel notification for Ninja Trail";
    #endregion
    public enum RewardType
    {
        //Inventory Item
        None = 0,
        Bronze = 1,
        Iron = 2,
        Mithril = 3,
        Titan = 4,
        Silver = 5,
        Adamantium = 6,
        Orihalcon = 7,
        PoisonMushroom = 8,
        FireFlower = 9,
        Aloe = 10,
        IceFlower = 11,
        LightningFlower = 12,
        RottenFlower = 13,
        Lily = 14,
        SunFlower = 15,
        Warpstone = 16,
        HyssopFlower = 17,
        Herbal = 18, 
        Reed = 19,
        Resin = 20,
        CrystalFlower = 21,
        PoisonGrease = 22,
        FireGrease = 23,
        WaterGrease = 24,
        IceGrease = 25,
        LightningGrease = 26,
        BugGrease = 27,
        MagicGrease = 28,
        HolyGrease = 29,
        DarkGrease = 30,
        BronzeShuriken = 31,
        SteelShuriken = 32,
        MithrilSuriken = 33,
        TitanSuriken = 34,
        SliverShuriken = 35,
        AdamantiumShuriken = 36,
        OrihalconShuriken = 37,
        MarkOfRebirth = 38,
        Heal = 39,
        LargeHeal = 40,
        GreatHeal = 41,
        VastHeal = 42,
        FullHeal = 43,
        Shield = 44,
        MagicShield=45,
        AlchemistJewel = 46,
        MineralKey = 47,
        BronzeShurikenPiece = 48,
        AdsMedal = 49,

        CrimsonDoll = 51,
        OniDoll = 52,
        FoxDoll = 53,
        TenguDoll = 54,
        RaionDoll = 55,

        Gold = 100,
        Gem = 101,
        SurikenDefault = 102,
        InventorySlot = 103,
        ElderScroll = 104,
        GoldBoost = 107,
        AdvanceGoldBoost = 108,
        PremiumGoldBoost = 109,
        DIYBackpackUpgrade = 110,
        BackpackUpgrade = 111,
        AdvanceBackpackUpgrade = 112,
        PremiumBackpackUpgrade = 113,
        ShurikenCap20 = 114,

        // Kill enemy by ...
        KillByPoison = 150,
        KillByFire = 151,
        KillByWater = 152,
        KillByIce = 153,
        KillByLightning = 154,
        KillByBug = 155,
        KillByMagic = 156,
        KillByHoly = 157,
        KillByDark = 158,
        //Action

        //Unlock Skill
        UnlockUpgradeIntonjutsu = 160,
        UnlockUpgradeKenjutsu = 161,
        UnlockUpgradeCritical = 162,
        UnlockUpgradeEvade = 163,
        UnlockUpgradeTaijutsu = 164,

        //Unlock craft shuriken
        UnlockCraftBronzeShuriken = 170,
        UnlockCraftSteelShuriken = 171,
        UnlockCraftMithrilShuriken = 172,
        UnlockCraftTitanShuriken = 173,
        UnlockCraftSliverShuriken = 174,
        UnlockCraftAdamantiumShuriken = 175,
        UnlockCraftOrihalconShuriken = 176,
        //Unlock Craft grease
        UnlockCraftPoisonGrease = 180,
        UnlockCraftFireGrease = 181,
        UnlockCraftWaterGrease = 182,
        UnlockCraftIceGrease = 183,
        UnlockCraftLightningGrease = 184,
        UnlockCraftBugGrease = 185,
        UnlockCraftMagicGrease = 186,
        UnlockCraftHolyGrease = 187,
        UnlockCraftDarkGrease = 188,

       
        UnlockToru = 190,
        UnlockYin = 191,

        //Mapboss
        BossZ1 = 201,
        BossZ2 = 202,
        BossZ3 = 203,
        BossZ4 = 204,
        BossZ5 = 205,
        Zone2 = 206,
        Zone3 = 207,
        Zone4 = 208,
        Zone5 = 209,

        //Farming
        FarmPoisionMushroom = 220,
        FarmFireFlower = 221,
        FarmAloe = 222,
        FarmIceFlower = 223,
        FarmLightningFlower = 224,
        FarmRottenFlower = 225,
        FarmHerbal = 226,
        FarmReed = 227,
        FarmResin = 228,
        FarmLily = 229,
        FarmSunFlower = 230,
        FarmHyssopFlower = 231,
        FarmCrystalFlower = 232,

        //ItemQuest
        IconMining = 720,
        IconHarvest = 721,
        IconInventorySlotX5 = 722,
        TextReward = 1000,
        UnlockCraft = 1001,
        UnlockHarvest = 1002,
        UnlockMining = 1003,
        UnlockGarden= 1004,
        // Random
        RandomGrease= 1100,
        RandomCrop=1101,
        RandomOnFourCrop=1102,
        // Reward Pack 
        OneDayTelekinesis = 1200,
    }
    public enum InputType
    {
        None=0,
        Button=1,
        Joystick=2,
    }
    /*public enum EnemyType
    {
        None = 0,
        Soldier = 1,
        Sniper = 2,
        ShieldSoldier = 3,
        SpearSoldier = 4,
        ShieldSpearSoldier = 5,
        Archer = 6,
        BombardGuard = 7,
        Kamikaze = 8,
        SpikeShield = 9,
        ManEaterFlower = 10,
        Markman = 11,
        BatMan = 12,
        BigBat = 13,
        Dog_1 = 14,
        GhostTree = 15,
        Skeleton_1 = 16,
        AxeBandit = 17,
        SniperBandit = 18,
        LurkingBandit = 19,
        HangingBandit = 20,
        GuardianBandit = 21,
        SwordBandit = 22,
        CrossBowBandit = 23,
        Zoombie = 24,
        ZoombiePutrid = 25,
        ZoombieWithStick = 26,
        SkeletonSpear = 27,
        SkeletonSoldier = 28,
        SkeletonGrim = 29,
        SkeletonGuard = 30,
        SkeletonArcher = 31,
        SkeletonFire = 32,
        SkeletonDualSword = 33,
        GuardDog = 34,
        WolfDog = 35,
        WolfAlpha = 36,
        Goblin = 37,
        GoblinDualSword = 38,
        GoblinSoldier = 39,
        GoblinNimble = 40,
        GoblinSword = 41,
        LongNeck = 42,
        BossGuardCaptain = 43,
        BossTree = 44,
        MiniBossBirdOfDeath = 45,
        MiniBossKappa=46,
        OniLord = 47,
        Samebito=48,
        StrongGoblin=49,
        SkeletonPoision = 50,
        SkeletonLightning = 51,
    }*/

}