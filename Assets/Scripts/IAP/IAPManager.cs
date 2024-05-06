using System;
using System.Collections.Generic;
using H2910.Common.Singleton;
using H2910.Defines;
using DG.Tweening;
using System.Collections;
using UnityEngine;

public class IAPManager : ManualSingletonMono<IAPManager>
{

    [SerializeField] List<HealRank> healRanks;
    private bool _isInit;
    private List<IAPType> _listActivePack = new List<IAPType>();
    private bool _isRemoveAds;
    private DateTime _timeShowPopupInventory = DateTime.MinValue;
    private TimeSpan _timeLeftTrialAds = TimeSpan.Zero;
    private TimeSpan _timeLeftRemoveAds = TimeSpan.Zero;
    private DateTime _timeShowSupportPack;
    private int _totalMinDelaySupportPack;
    private int _totalRebirth;


    /// <summary>
    /// Gets the products.
    /// </summary>
    /// <value>The products.</value>
    /*public ProductCollection Products { get; private set; }

    public TimeSpan TimeLeftTrialAds => timeLeftTrialAds;
    public TimeSpan TimeLeftRemoveAds => timeLeftRemoveAds;

    public event Action<IAPType> OnPurchasedComplete;
    public event Action OnUpdatePack;

    public List<IAPType> ListActivePack => _listActivePack;
    private List<IAPType> listBackVillage = new List<IAPType>();

    private DictPackConfig dictConfig => GameConfig.Instance.PackConfig.DictPackConfig;
    private List<PackDelayInfo> listSupportInfo = new List<PackDelayInfo>(5);
    private Action cbBuyBossHunterSucces;
    private Action cbBuyBossHunterFail;
    private void Start()
    {
        InitializePurchasing();
        for(int i = 0; i < 5; i++)
        {
            listSupportInfo.Add(new PackDelayInfo());
        }
        /*try
        {
            var options = new InitializationOptions()
                .SetEnvironmentName(environment);

            await UnityServices.InitializeAsync(options);
        }
        catch (Exception exception)
        {
            Debug.Log("Exeption init unity services "+ exception.Message);
        }#1#
    }
    public bool IsRemoveAds
    {
        get
        {
            if (_isRemoveAds)
                AchievementManager.Instance.WatchAds();
            return _isRemoveAds;
        }
    }
    void InitializePurchasing()
    {
        var module = StandardPurchasingModule.Instance();
        module.useFakeStoreUIMode = FakeStoreUIMode.StandardUser;
        m_IsGooglePlayStoreSelected = Application.platform == RuntimePlatform.Android && module.appStore == AppStore.GooglePlay;
        ConfigurationBuilder builder;
        if (Application.platform == RuntimePlatform.Android)
        {
            builder = ConfigurationBuilder.Instance(module);
        }
        else
        {
            builder = ConfigurationBuilder.Instance(module);
        }
        builder.products.Clear();
        foreach (var item in dictProductIDs.DictProductID)
        {
            builder.AddProduct(item.Value, ProductType.Consumable);
        }

        var appIdentifier = Application.identifier;
#if !UNITY_EDITOR
       validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), appIdentifier);
#endif
        UnityPurchasing.Initialize(this, builder);
    }

    public void InitializePack(bool isFirstSetup, bool isLogin)
    {
        if (!isFirstSetup && !isLogin && !PlayerData.Instance.InitData)
            return;
        CheckData();
    }    

    public void InitTime()
    {
        if (!_isInit)
        {
            _isInit = true;
            UpdateManager.Instance.OnUpdateSecond += UpdateTime;
        }
        CheckPack(IAPType.TrialRemoveAds);
        CheckPack(IAPType.RemoveAds);
        OnUpdatePack?.Invoke();
    }
    public bool IsActivePack(IAPType type)
    {
        return _listActivePack.Contains(type);
    }    

    public void CheckData()
    {
        foreach(var pack in dictConfig)
        {
            if(pack.Key != IAPType.RemoveAds && pack.Key != IAPType.TrialRemoveAds)
                CheckPack(pack.Key, false);
        }
        CheckPackSupport();
        OnUpdatePack?.Invoke();
    }

    public void CheckPack(IAPType type, bool needRefresh = true)
    {
        if (!PlayerData.Instance.InitData)
            return;
            //Debug.Log(" check Pack " + type + " " + DateTime.Now);
        switch (type)
        {
            case IAPType.TrialRemoveAds:
                if (CheckAdsActivePack(IAPType.PermanentRemoveAds))
                    break;
                if (CheckAdsActivePack(IAPType.TrialRemoveAds))
                {
                    var timeFromLastBuy = UpdateManager.Instance.InternetTime - PlayerData.Instance.PlayerPack.DictPackData[IAPType.TrialRemoveAds].TimeBought;
                    if(timeFromLastBuy >= TimeSpan.FromDays(1))
                    {
                        PlayerData.Instance.PlayerPack.UpdateActiveData(IAPType.TrialRemoveAds, false);
                        if (CheckAdsActivePack(IAPType.RemoveAds))
                            _isRemoveAds = false;
                    }    
                    else
                    {
                        timeLeftTrialAds = TimeSpan.FromDays(1) - timeFromLastBuy;
                        _isRemoveAds = true;
                    }    
                }    
                else if (PlayerData.Instance.PlayerPack.FirstTimeLoginInternet != null)
                {
                    if ((UpdateManager.Instance.InternetTime - (DateTime)PlayerData.Instance.PlayerPack.FirstTimeLoginInternet).TotalDays >= GameDefine.DayFreeAdsFromFirstLogin && PlayerData.Instance.PlayerMapInfo.GetHighestLevel() > 200)
                    {
                        CheckLimitBuyPack(type);
                    }
                }     
                break;
            case IAPType.RemoveAds:
                if (CheckAdsActivePack(IAPType.PermanentRemoveAds))
                {
                    RemovePackFromList(type);
                    break;
                }
                if(!PlayerData.Instance.PlayerPack.DictPackData.ContainsKey( IAPType.RemoveAds))
                {
                    AddPack(IAPType.RemoveAds);
                    break;
                }    
                var timeLastBuy = UpdateManager.Instance.InternetTime - PlayerData.Instance.PlayerPack.DictPackData[IAPType.RemoveAds].TimeBought;
                if (timeLastBuy >= TimeSpan.FromDays(30))
                {
                    PlayerData.Instance.PlayerPack.UpdateActiveData(IAPType.RemoveAds, false);
                    if (CheckAdsActivePack(IAPType.TrialRemoveAds))
                        _isRemoveAds = false;
                    AddPack(IAPType.RemoveAds);
                }
                else
                {
                    PlayerData.Instance.PlayerPack.UpdateActiveData(IAPType.RemoveAds, true);
                    timeLeftRemoveAds = TimeSpan.FromDays(30) - timeLastBuy;
                    _isRemoveAds = true;
                    RemovePackFromList(type);
                }
               
                break;
            case IAPType.PermanentRemoveAds:
                if (CheckAdsActivePack(IAPType.PermanentRemoveAds))
                {
                    RemovePackFromList(type);
                    _isRemoveAds = true;
                }    
                else
                {
                    AddPack(IAPType.RemoveAds);
                    AddPack(IAPType.PermanentRemoveAds);
                }    
                break;
            case IAPType.Telekinesis:
                if (PlayerData.Instance.PlayerProp.IsUnlockFeature( UnlockFeature.CraftBronzeShuriken) || PlayerData.Instance.PlayerProp.IsUnlockFeature(UnlockFeature.CompleteTutorialUsingGrease))
                {
                    CheckLimitBuyPack(type);
                }
                break;
            case IAPType.GardenExpand:
                if (PlayerData.Instance.PlayerProp.IsUnlockFeature(UnlockFeature.UnlockGarden))
                {
                    CheckLimitBuyPack(type);
                }
                break;
            case IAPType.InventorySlot:
                if (PlayerData.Instance.Inventory.TotalSlot < GameDefine.MaxSlotItem)
                    CheckLimitBuyPack(type);
                else
                {
                    RemovePackFromList(type);
                }   
                break;
            case IAPType.Skin1:
                //AddPack(type);
                break;
            case IAPType.Support1:
                AddPack(type);
                break;
            case IAPType.Support2:
                AddPack(type);
                break;
            case IAPType.Support3:
                AddPack(type);
                break;
            case IAPType.Support4:
                AddPack(type);
                break;
            case IAPType.Support5:
                AddPack(type);
                break;
            case IAPType.BossHunter:
                CheckLimitBuyPack(type);
                break;
            case IAPType.SkillPack1:
                if (!PlayerData.Instance.PlayerProp.IsMaxSkill(SkillUpgradeType.Kenjutsu))
                    CheckLimitBuyPack(type);
                else
                    RemovePackFromList(type);
                break;
            case IAPType.SkillPack2:
                if (!PlayerData.Instance.PlayerProp.IsMaxSkill(SkillUpgradeType.Intonjutsu))
                    CheckLimitBuyPack(type);
                else
                    RemovePackFromList(type);
                break;
            case IAPType.SkillPack3:
                if(!PlayerData.Instance.PlayerProp.IsMaxSkill(SkillUpgradeType.Evade))
                    CheckLimitBuyPack(type);
                else
                    RemovePackFromList(type);
                break;
            case IAPType.SkillPack4:
                if(!PlayerData.Instance.PlayerProp.IsMaxSkill(SkillUpgradeType.Critical))
                    CheckLimitBuyPack(type);
                else
                    RemovePackFromList(type);
                break;
            case IAPType.SkillPack5:
                if (!PlayerData.Instance.PlayerProp.IsMaxSkill(SkillUpgradeType.Taijutsu))
                    CheckLimitBuyPack(type);
                else
                    RemovePackFromList(type);
                break;
            case IAPType.SkillPack6:
                if (PlayerData.Instance.PlayerAdventureShop.GetDaysFromFirstPlay() >= 2 || PlayerData.Instance.PlayerMapInfo.GetMapStatus(new MapInfo( MapType.Suburban, 5)) != LevelMapStatus.Lock)
                {
                    if (!PlayerData.Instance.PlayerProp.IsMaxAllSkill())
                        CheckLimitBuyPack(type);
                    else
                        RemovePackFromList(type);
                }
                else
                    RemovePackFromList(type);
                break;
            case IAPType.ExtraShuriken:
                if (PlayerData.Instance.PlayerAdventureShop.GetDaysFromFirstPlay() >= 1 || PlayerData.Instance.PlayerQuest.ListCompleteQuest.Contains("M8") || PlayerData.Instance.PlayerQuest.ListCompleteQuest.Contains("M12"))
                {
                    CheckLimitBuyPack(type);
                }
                break;
            case IAPType.UpgradeShurikenMax:
                bool isAddPackUpgradeMax = false;
                if (PlayerData.Instance.Inventory.ListWeapon.Count <= 1)
                {
                    RemovePackFromList(type);
                    break;
                }
                for (int i = 0; i < PlayerData.Instance.Inventory.ListWeapon.Count; i++)
                {
                    if (PlayerData.Instance.Inventory.ListWeapon[i].ItemData.InventoryItem != InventoryItem.DefautShuriken && PlayerData.Instance.Inventory.ListWeapon[i].ItemData.InventoryItem != InventoryItem.OrihalconShuriken)
                    {
                        AddPack(type);
                        isAddPackUpgradeMax = true;
                        break;
                    }
                }
                if (!isAddPackUpgradeMax)
                    RemovePackFromList(type);
                break;
            case IAPType.UpgradeShuriken:
                bool isAddPack = false;
                if(PlayerData.Instance.Inventory.ListWeapon.Count <= 1)
                {
                    RemovePackFromList(type);
                    break;
                }     
                for(int i = 0; i < PlayerData.Instance.Inventory.ListWeapon.Count; i++)
                {
                    if(PlayerData.Instance.Inventory.ListWeapon[i].ItemData.InventoryItem != InventoryItem.DefautShuriken && PlayerData.Instance.Inventory.ListWeapon[i].ItemData.InventoryItem != InventoryItem.OrihalconShuriken)
                    {
                        AddPack(type);
                        isAddPack = true;
                        break;
                    }    
                }
                if(!isAddPack)
                    RemovePackFromList(type);
                break;
            default:
                break;
        }
        if (needRefresh)
            OnUpdatePack?.Invoke();
    } 
    
    private void CheckPackSupport()
    {
        int maxHealth = MainStats.Instance.MaxHealth;
        for (int i = 0; i < 5; i++)
        {
            var totalMinutes = (DateTime.UtcNow - listSupportInfo[i].TimeShowPack).TotalMinutes;
            if (totalMinutes > 0 && totalMinutes < listSupportInfo[i].TotalMinDelay)
                continue; 
            if (maxHealth >= healRanks[i].HPMin && maxHealth <= healRanks[i].HPMax)
                listSupportInfo[i].TotalRebirth = healRanks[i].TotalRebirth;
            else if(maxHealth < healRanks[i].HPMin)
                listSupportInfo[i].TotalRebirth = healRanks[i].TotalRebirth + Math.Min(10, (healRanks[i].HPMin - maxHealth));    
            else
                listSupportInfo[i].TotalRebirth = healRanks[i].TotalRebirth + Math.Min(20, (maxHealth - healRanks[i].HPMax) * 2);
        } 
    }    

    private void RemovePackFromList(IAPType type)
    {
        if (_listActivePack.Contains(type))
            _listActivePack.Remove(type);
    }   
    
    private void AddPack(IAPType type)
    {
        if (!_listActivePack.Contains(type))
            _listActivePack.Add(type);
    }   
    
    private bool CheckAdsActivePack(IAPType type)
    {
        if(dictConfig[type].Limit == 0)
        {
            return PlayerData.Instance.PlayerPack.GetActivePack(type);
        }    
        if (PlayerData.Instance.PlayerPack.GetActivePack(type) || PlayerData.Instance.PlayerPack.GetTotalBuyCount(type) >= dictConfig[type].Limit)
            return true;
        else
            return false;
    }    

    private void CheckLimitBuyPack(IAPType type)
    {
        if (PlayerData.Instance.PlayerPack.DictPackData.ContainsKey(type))
        {
            if (PlayerData.Instance.PlayerPack.GetTotalBuyCount(type) < dictConfig[type].Limit)
            {
                AddPack(type);
            }
            else
            {
                RemovePackFromList(type);
            }
        }
        else
        {
            InitData(type);
            AddPack(type);
        }
    }    

    private void InitData(IAPType type)
    {
        PackData data = new PackData();
        PlayerData.Instance.PlayerPack.InitData(type, data);
    } 

    public void PlayerRebirth()
    {
        if(!PlayerData.Instance.PlayerProp.IsUnlockFeature(UnlockFeature.CompleteMainTutorial))
        {
            return;
        }
        _totalRebirth++;
        int minValue = -1;
        int minIndex = -1;
        for(int i = 0; i < listSupportInfo.Count; i++)
        {
            if (listSupportInfo[i].TotalRebirth > 0 && (!PlayerData.Instance.Inventory.DictTotalItem.ContainsKey(MatchInventoryFromPack(i)) || PlayerData.Instance.Inventory.DictTotalItem[MatchInventoryFromPack(i)] == 0) && GetShowPopupToday(GetTypeSupportPack(i)))
            {
                if(minValue == -1)
                {
                    minIndex = i;
                    minValue = listSupportInfo[i].TotalRebirth;
                }
                else if(minValue > listSupportInfo[i].TotalRebirth)
                {
                    minValue = listSupportInfo[i].TotalRebirth;
                    minIndex = i;
                }
            }
        }
        if (minIndex != -1 && (DateTime.UtcNow - _timeShowSupportPack).TotalMinutes >= _totalMinDelaySupportPack && _totalRebirth >= 2 
            && listSupportInfo[minIndex].TotalRebirth != -1 && GetShowPopupToday(GetTypeSupportPack(minIndex))
            && (DateTime.UtcNow - listSupportInfo[minIndex].TimeShowPack).TotalMinutes >= listSupportInfo[minIndex].TotalMinDelay
            )
        {
            listSupportInfo[minIndex].TotalRebirth = -1;
            listSupportInfo[minIndex].TotalMinDelay += listSupportInfo[minIndex].TotalMinDelay == 0 ? 10 : 5;
            ShowPackSupport(minIndex);
            return;
        }

        if (minIndex == -1)
        {
            CheckPackSupport();
        }    
    }

    private void UpdateIconPackSupport(IAPType type)
    {
        if(type != IAPType.Support1 && type != IAPType.Support2 && type != IAPType.Support3 && type != IAPType.Support4 && type != IAPType.Support5)
            return;
        PlayerData.Instance.PlayerPack.SetCanShowIconPack(IAPType.Support1, type == IAPType.Support1);
        PlayerData.Instance.PlayerPack.SetCanShowIconPack(IAPType.Support2, type == IAPType.Support2);
        PlayerData.Instance.PlayerPack.SetCanShowIconPack(IAPType.Support3, type == IAPType.Support3);
        PlayerData.Instance.PlayerPack.SetCanShowIconPack(IAPType.Support4, type == IAPType.Support4);
        PlayerData.Instance.PlayerPack.SetCanShowIconPack(IAPType.Support5, type == IAPType.Support5);
    } 
    
    private InventoryItem MatchInventoryFromPack(int index)
    {
        switch (index)
        {
            case 0:
                return InventoryItem.Heal;
            case 1:
                return InventoryItem.LargeHeal;
            case 2:
                return InventoryItem.GreatHeal;
            case 3:
                return InventoryItem.VastHeal;
            case 4:
                return InventoryItem.FullHeal;
            default:
                return InventoryItem.None;
        }
    }

    private void BuySupportPack(IAPType type)
    {
        
    }

    private void ShowPackSupport(int index)
    {
        _timeShowSupportPack = DateTime.UtcNow;
        _totalMinDelaySupportPack = 1;
        IAPType type = GetTypeSupportPack(index);
        AddPack(type);
        UpdateIconPackSupport(type);  
        PopupManager.Instance.OnShowScreen(PopupName.Pack, new PopupPack.Param { Type = type });
        _listActivePack.Add(type);
        OnUpdatePack?.Invoke();
    }    
    
    private IAPType GetTypeSupportPack(int index)
    {
        switch (index)
        {
            case 0:
                return IAPType.Support1;
            case 1:
                return IAPType.Support2;
            case 2:
                return IAPType.Support3;
            case 3:
                return IAPType.Support4;
            case 4:
                return IAPType.Support5;
            default:
                return IAPType.Support1;
        }
    }

    public void SetShowPopupTodayAgain(IAPType type, bool canShowAgain)
    {
        if(canShowAgain)
            PlayerData.Instance.PlayerPack.SetDontShowAgainToday(DateTime.MinValue, type);  
        else
        {
            PlayerData.Instance.PlayerPack.SetDontShowAgainToday(DateTime.UtcNow, type);
        }       
    } 
    
    public bool GetShowPopupToday(IAPType type)
    {
        return DateTime.UtcNow.Date != PlayerData.Instance.PlayerPack.GetDontShowAgainToday(type).Date;
    }    

    public void UpdateTime()
    {
        if(UpdateManager.Instance.HasInternetTime)
        {
            if(CheckAdsActivePack(IAPType.TrialRemoveAds))
            {
                timeLeftTrialAds -= TimeSpan.FromSeconds(1);
                if (timeLeftTrialAds <= TimeSpan.Zero)
                {
                    PlayerData.Instance.PlayerPack.UpdateActiveData(IAPType.TrialRemoveAds, false);
                    if (!CheckAdsActivePack(IAPType.PermanentRemoveAds) && !CheckAdsActivePack(IAPType.RemoveAds))
                        _isRemoveAds = false;
                }    
            }

            if (CheckAdsActivePack(IAPType.RemoveAds))
            {
                timeLeftRemoveAds -= TimeSpan.FromSeconds(1);
                if (timeLeftRemoveAds <= TimeSpan.Zero)
                {
                    PlayerData.Instance.PlayerPack.UpdateActiveData(IAPType.RemoveAds, false);
                    if (!CheckAdsActivePack(IAPType.PermanentRemoveAds) && !CheckAdsActivePack(IAPType.TrialRemoveAds))
                        _isRemoveAds = false;
                }
            }
        }    
    }
    public bool CanShowIconPack(IAPType type)
    {
        return ListActivePack.Contains(type) && PlayerData.Instance.PlayerPack.GetCanShowIconPack(type);
    } 
    public void OnBuyPack(IAPType type, Action callback = null)
    {
        if(type == IAPType.TrialRemoveAds)
        {
            if(!UpdateManager.Instance.HasInternetTime)
            {
                PopupManager.Instance.ShowNotice(LanguageUtils.GetLanguageValue(LanguageDefine.InternetCheck));
                return;
            }   
            
            if (PlayerData.Instance.PlayerPack.DictPackData.ContainsKey(IAPType.TrialRemoveAds) && PlayerData.Instance.PlayerPack.GetTotalBuyCount(type) > 0)
                return;
            PlayerData.Instance.PlayerPack.BuyPack(IAPType.TrialRemoveAds);
            timeLeftTrialAds = TimeSpan.FromDays(1);
            RemovePackFromList(IAPType.TrialRemoveAds);
            OnPurchasedComplete?.Invoke(IAPType.TrialRemoveAds);
            _isRemoveAds = true;
            AnalyticsManager.Instance.LogEvent(AnalyticEventID.trial_remove_ads, new Param(AnalyticLevelParam.level_highest, PlayerData.Instance.PlayerMapInfo.GetHighestLevel()));
            //PopupManager.Instance.OnShowScreen(PopupName.RewardPack, new PopupRewardPack.Param { Type = IAPType.TrialRemoveAds });
            RewardPack(type);
            return;
        }
        else if( type == IAPType.RemoveAds)
        {
            if (!UpdateManager.Instance.HasInternetTime)
            {
                PopupManager.Instance.ShowNotice(LanguageUtils.GetLanguageValue(LanguageDefine.InternetCheck));
                return;
            }
        }    

        if (m_StoreController == null)
            return;
        if (dictProductIDs.DictProductID.ContainsKey(type))
        {
#if UNITY_EDITOR
            TestPurchasing(type);
#elif DEV
            m_StoreController.InitiatePurchase(dictProductIDs.DictProductID[type]);
            TestPurchasing(type);
#else
            m_StoreController.InitiatePurchase(dictProductIDs.DictProductID[type]);
#endif
        }
        else
            Debug.LogError("Check IAP product ID");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message = null)
    {
        var errorMessage = $"Purchasing failed to initialize. Reason: {error}.";

        if (message != null)
        {
            errorMessage += $" More details: {message}";
        }

        Debug.Log(errorMessage);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
#if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX
        // Prepare the validator with the secrets we prepared in the Editor
        // obfuscation window.
        try
        {
            var validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);

            var result = validator.Validate(args.purchasedProduct.receipt);
            foreach (IPurchaseReceipt productReceipt in result)
            {
                if (productReceipt != null && productReceipt.productID != null && !string.IsNullOrEmpty(productReceipt.transactionID))
                {
                    foreach (var item in dictProductIDs.DictProductID)
                    {
                        if (item.Value == productReceipt.productID)
                        {
                            if (dictConfig.ContainsKey(item.Key))
                            {
                                OnPurchasingPackComplete(item.Key);
                                CheckPack(item.Key);
                                OnUpdatePack?.Invoke();
                            }
                            AnalyticsManager.Instance.LogEventBuyItem(AnalyticEventID.buy_pack_ev, productReceipt.productID, 1);
                            OnPurchasedComplete?.Invoke(item.Key);
                            return PurchaseProcessingResult.Complete;
                        }
                    }
                }
            }
            return PurchaseProcessingResult.Pending;
        }
        catch (IAPSecurityException)
        {
            return PurchaseProcessingResult.Pending;
        }
#endif
    }

    private void TestPurchasing(IAPType type)
    {
        if (dictConfig.ContainsKey(type))
        {
            OnPurchasingPackComplete(type);
            CheckPack(type);
            OnUpdatePack?.Invoke();
        }
        //AnalyticsManager.Instance.LogEventBuyItem(AnalyticEventID.buy_pack_ev, dictProductID[type], 1);
        OnPurchasedComplete?.Invoke(type);
    }
    
    private void OnPurchasingPackComplete(IAPType type)
    {
        PlayerData.Instance.PlayerPack.BuyPack(type);
        
        switch (type)
        {
            case IAPType.RemoveAds:
                _isRemoveAds = true;
                timeLeftRemoveAds = TimeSpan.FromDays(30);
                PlayerData.Instance.PlayerPack.UpdateActiveData(type, true);
                break;
            case IAPType.PermanentRemoveAds:
                PlayerData.Instance.PlayerPack.UpdateActiveData(IAPType.PermanentRemoveAds, true);
                RemovePackFromList(IAPType.RemoveAds);
                _isRemoveAds = true;
                break;
            case IAPType.Telekinesis:
                PlayerData.Instance.PlayerPack.UpdateActiveData(type, true);
                PlayerData.Instance.Inventory.AddSlotInventory(10);
                break;
            case IAPType.GardenExpand:
                PlayerData.Instance.PlayerFarming.BuySlot(FarmingSlot.Slot3, 0);
                break;
            case IAPType.InventorySlot:
                PlayerData.Instance.Inventory.AddSlotInventory(100);
                break;
            case IAPType.Skin1:
                break;
            case IAPType.ExtraShuriken:
                PlayerData.Instance.PlayerPack.UpdateActiveData(IAPType.UpgradeShurikenMax, true);
                PlayerData.Instance.PlayerPack.SetCanShowIconPack(IAPType.UpgradeShurikenMax, true);
                break;
            case IAPType.UpgradeShurikenMax:
                break;
            case IAPType.UpgradeShuriken:
                PlayerData.Instance.PlayerPack.UpdateActiveData(IAPType.UpgradeShurikenMax, true);
                PlayerData.Instance.PlayerProp.AddResource(ResourceType.Gold, 20000, true);
                break;
            case IAPType.Support1:
                break;
            case IAPType.Support2:
                break;
            case IAPType.Support3:
                break;
            case IAPType.Support4:
                break;
            case IAPType.Support5:
                break;
            case IAPType.SkillPack1:
                PlayerData.Instance.PlayerProp.BuyPackUpgradeSkill(type);
                CheckPack(IAPType.SkillPack6);
                CloseCurrentPopupAndShowUpgradeSkillPopUp(1);
                break;
            case IAPType.SkillPack2:
                PlayerData.Instance.PlayerProp.BuyPackUpgradeSkill(type);
                CheckPack(IAPType.SkillPack6);
                CloseCurrentPopupAndShowUpgradeSkillPopUp(0);
                break;
            case IAPType.SkillPack3:
                PlayerData.Instance.PlayerProp.BuyPackUpgradeSkill(type);
                QuestManager.Instance.DebugCompleteQuest("Q4");
                CloseCurrentPopupAndShowUpgradeSkillPopUp(4);
                CheckPack(IAPType.SkillPack6);
                break;
            case IAPType.SkillPack4:
                PlayerData.Instance.PlayerProp.BuyPackUpgradeSkill(type);
                QuestManager.Instance.DebugCompleteQuest("Q3");
                CheckPack(IAPType.SkillPack6);
                CloseCurrentPopupAndShowUpgradeSkillPopUp(3);
                break;
            case IAPType.SkillPack5:
                PlayerData.Instance.PlayerProp.BuyPackUpgradeSkill(type);
                CheckPack(IAPType.SkillPack6);
                CloseCurrentPopupAndShowUpgradeSkillPopUp(2);
                break;
            case IAPType.SkillPack6:
                RemovePackFromList(IAPType.SkillPack1);
                RemovePackFromList(IAPType.SkillPack2);
                RemovePackFromList(IAPType.SkillPack3);
                RemovePackFromList(IAPType.SkillPack4);
                RemovePackFromList(IAPType.SkillPack5);
                PlayerData.Instance.PlayerProp.BuyPackUpgradeSkill(type);
                QuestManager.Instance.DebugCompleteQuest("Q3");
                QuestManager.Instance.DebugCompleteQuest("Q4");
                PlayerData.Instance.PlayerProp.AddResource(ResourceType.Gold, 100000, true);
                PlayerData.Instance.PlayerProp.AddResource(ResourceType.Gem, 100 + PlayerData.Instance.PlayerProp.GetGemReturnOnSkillsHaveBeenUpgraded(), true);
                CloseCurrentPopupAndShowUpgradeSkillPopUp();
                break;
            case IAPType.BossHunter:
                PlayerData.Instance.PlayerPack.UpdateActiveData(type, true);
                PlayerData.Instance.Inventory.AddSlotInventory(5);
                cbBuyBossHunterSucces?.Invoke();
                Debug.Log("continue with more live");
                break;
            default:
                break;
        }

        bool checkMail = false;

        for (int i = 0; i < dictConfig[type].Rewards.Count; i++)
        {
            if (PlayerData.Instance.Inventory.CanAddItem(dictConfig[type].Rewards[i]))
                PlayerData.Instance.Inventory.AddItem(dictConfig[type].Rewards[i], ignoreShowPack: true);
            else
            {
                PlayerData.Instance.PlayerMail.AddNewMail(new MailData
                {
                    Content = dictConfig[type].PackName + "'s item has been delivered",
                    Rewards = dictConfig[type].Rewards[i],
                    TimeGetMail = DateTime.UtcNow,
                    TimeCountDown = TimeSpan.FromDays(99)
                });
                checkMail = true;
                //PlayerData.Instance.Inventory.AddSlotInventory();
                //PlayerData.Instance.Inventory.AddItem(dictConfig[type].Rewards[i], ignoreShowPack: true);
            }
        }

        if (type != IAPType.UpgradeShuriken)
            //new
            RewardPack(type,null,checkMail);
            //old
            /*PopupManager.Instance.OnShowScreen(PopupName.RewardPack, new PopupRewardPack.Param { Type = type, isFullInventory = checkMail , CallBack = ()=> {

                List<RewardInfo> rewards = new List<RewardInfo>();
                for(int i = 0; i < dictConfig[type].Rewards.Count; i++)
                {
                    rewards.Add(new RewardInfo(dictConfig[type].Rewards[i].InventoryItem, dictConfig[type].Rewards[i].Quantity));
                }    
                PopupManager.Instance.OnShowScreen(PopupName.GetItem, new PopupGetItem.Param
                    {
                        ListReward = rewards,
                        PopupType = PopupGetItem.PopupGetItemType.List,
                        callback = () => {
                            if (checkMail)
                                PopupManager.Instance.OnShowScreen(PopupName.Notice, new PopupNotice.Param
                                {
                                    Content = "Because your inventory is full, some rewards have been sent to mailbox.",
                                    CallBack = () => { 
                                        PopupManager.Instance.OnShowScreen(PopupName.Mail); },
                                            NoticeType = NoticeType.LinkTo,
                                            Title = "Mesenge",
                                            btnText = "OK"
                                }, ParentPopup.Hight);
                        }
                    }, ParentPopup.Hight);
                }
            });#1#

        

        if (type == IAPType.ExtraShuriken)
        {
            CheckPack(IAPType.UpgradeShuriken);
            CheckPack(IAPType.UpgradeShurikenMax);
        }

        PlayerData.Instance.SaveAllDataToPlayfab(null);
    }

    private static void CloseCurrentPopupAndShowUpgradeSkillPopUp(int index=-1)
    {
        PopupManager.Instance.OnCloseScreen( PopupName.Pack);
        DOVirtual.DelayedCall(0.5f, () =>
        {
            PopupManager.Instance.OnShowScreen(PopupName.UpgradeSkill, new PopupUpgradeSkill.Param { Index = index }, ParentPopup.Hight);
        });
    }

    public void RewardPack(IAPType type, Sprite extraSprite = null, bool checkMail = false)
    {
        List<Sprite> listIconReward = new List<Sprite>();
        foreach (var item in GameConfig.Instance.PackConfig.DictPackConfig[type].rewardsIcon)
            listIconReward.Add(item);
        if (extraSprite != null)
            listIconReward.Add(extraSprite);
        StartCoroutine(AnimationRewardPack(listIconReward,checkMail));
    }
    IEnumerator AnimationRewardPack(List<Sprite> icons, bool fullInventory = false)
    {
        for (int i = 0; i < icons.Count; i++)
        {
            Vector3 stopTrans;
            Vector3 tagetTrans;
            var obj = ObjectPool.Instance.GetPoolObject(ObjectPoolType.ItemReward);
            if (obj != null)
            {
                obj.transform.position = PopupManager.Instance.transform.position;
                if (icons[i].name == "Ic_gold" || icons[i].name == "Gem" || icons[i].name == "ic_heart_lives")
                {
                    stopTrans = UIManager.Instance.GetPos(ItemUIType.CurrencyCollectPoint);
                    tagetTrans = UIManager.Instance.GetPos(ItemUIType.CollectTargetPoint);
                }
                else
                {
                    stopTrans = UIManager.Instance.GetPos(ItemUIType.ItemCollectCheckPoint);
                    tagetTrans = UIManager.Instance.GetPos( ItemUIType.BtnInventory);
                }
                obj.GetComponent<UIItemReward>().SetUpRewardMove(icons[i],1, stopTrans, tagetTrans, .7f);
            }
            yield return new WaitForSecondsRealtime(.2f);
        }
        yield return new WaitForSecondsRealtime(2f);
        if (fullInventory)
            PopupManager.Instance.OnShowScreen(PopupName.Notice, new PopupNotice.Param
            {
                Content = LanguageUtils.GetLanguageValue(LanguageDefine.SendToMail),
                CallBack = () => {
                    PopupManager.Instance.OnShowScreen(PopupName.Mail);
                },
                NoticeType = NoticeType.LinkTo,
                Title = LanguageUtils.GetLanguageValue(LanguageDefine.Message),
                btnText = LanguageUtils.GetLanguageValue(LanguageDefine.Ok),
            }, ParentPopup.Hight);
    }
    public void ShowPackAfterLogin()
    {
        if (PopupManager.Instance.IsPopupShowing(PopupName.Dialog) || TutorialManager.Instance.IsShowingTutorial())
        {
            return;
        }    
        if (PlayerData.Instance.PlayerMapInfo.GetMapStatus(new MapInfo(MapType.Suburban, 3)) != LevelMapStatus.Lock && !PlayerData.Instance.PlayerPack.GetActivePack(IAPType.PermanentRemoveAds) && GetShowPopupToday(IAPType.PermanentRemoveAds))
        {
            PopupManager.Instance.OnShowScreen(PopupName.Pack, new PopupPack.Param
            {
                Type = IAPType.PermanentRemoveAds,
                CallBack = () =>
                    {
                        if (ListActivePack.Contains(IAPType.ExtraShuriken) && GetShowPopupToday(IAPType.ExtraShuriken))
                        {
                            DOVirtual.DelayedCall(0.3f, () => {
                                PopupManager.Instance.OnShowScreen(PopupName.Pack, new PopupPack.Param
                                {
                                    Type = IAPType.ExtraShuriken
                                });
                                PlayerData.Instance.PlayerPack.SetCanShowIconPack(IAPType.ExtraShuriken, true);
                            });
                        } 
                    }
            });
            PlayerData.Instance.PlayerPack.SetCanShowIconPack(IAPType.PermanentRemoveAds, true);
            OnUpdatePack?.Invoke();
        }
        else
        {
            if (ListActivePack.Contains(IAPType.ExtraShuriken) && GetShowPopupToday(IAPType.ExtraShuriken))
            {
                PopupManager.Instance.OnShowScreen(PopupName.Pack, new PopupPack.Param
                {
                    Type = IAPType.ExtraShuriken
                });
                PlayerData.Instance.PlayerPack.SetCanShowIconPack(IAPType.ExtraShuriken, true);
            }
            else
            {
                ShowRandomPack();
            }
        }
    }

    private void ShowRandomPack()
    {
        List<IAPType> listIPA = new List<IAPType>();
        foreach (var item in ListActivePack)
        {
            if (item != IAPType.PermanentRemoveAds && item != IAPType.ExtraShuriken && item != IAPType.SkillPack1 && item != IAPType.SkillPack2
                && item != IAPType.SkillPack3 && item != IAPType.SkillPack4 && item != IAPType.SkillPack5 && item != IAPType.RemoveAds && item != IAPType.TrialRemoveAds)
                listIPA.Add(item);
        }
        if(listIPA.Count > 1)
        {
            var type = listIPA.RandomElement();
            PopupManager.Instance.OnShowScreen(PopupName.Pack, new PopupPack.Param
            {
                Type = type
            });
            PlayerData.Instance.PlayerPack.SetCanShowIconPack(type, true);
            UpdateIconPackSupport(type);
        }
    }

    public void ShowPackExtraShuriken()
    {
        if (!GetShowPopupToday(IAPType.ExtraShuriken))
            return;
        for(int i = 0; i < PlayerData.Instance.Inventory.ListWeapon.Count; i++)
        {
            if (PlayerData.Instance.Inventory.ListWeapon[i].ItemData.InventoryItem != InventoryItem.DefautShuriken && PlayerData.Instance.Inventory.ListWeapon[i].ItemData.Quantity > 0)
                return;
        }    
        if (ListActivePack.Contains(IAPType.ExtraShuriken))
        {
            PopupManager.Instance.OnShowScreen(PopupName.Pack, new PopupPack.Param { Type = IAPType.ExtraShuriken });
            PlayerData.Instance.PlayerPack.SetCanShowIconPack(IAPType.ExtraShuriken, true);
            OnUpdatePack?.Invoke();
        }    
    }

    public void ShowAdsPack()
    {
        if (!GetShowPopupToday(IAPType.PermanentRemoveAds))
            return;

        if (PopupManager.Instance.IsPopupShowing(PopupName.UpgradeSkill))
            return;

        if (PopupManager.Instance.IsPopupShowing(PopupName.Garden))
            return;

        if (PopupManager.Instance.IsPopupShowing(PopupName.LuckyWheel))
            return;

        if (ListActivePack.Contains(IAPType.PermanentRemoveAds) && !IsRemoveAds)
        {
            PopupManager.Instance.OnShowScreen(PopupName.Pack, new PopupPack.Param { Type = IAPType.PermanentRemoveAds }, ParentPopup.Hight);
        }
    }    

    public void ShowBossHunterPack(Action callbackSuccess, Action callbackFail)
    {
        if (!PlayerData.Instance.PlayerPack.GetActivePack(IAPType.BossHunter) && GetShowPopupToday(IAPType.BossHunter))
        {
            cbBuyBossHunterFail = callbackFail;
            cbBuyBossHunterSucces = callbackSuccess;
            PopupManager.Instance.OnShowScreen(PopupName.Pack, new PopupPack.Param { Type = IAPType.BossHunter, CallBack = callbackFail });
            PlayerData.Instance.PlayerPack.SetCanShowIconPack(IAPType.BossHunter, true);
            OnUpdatePack?.Invoke();
        }
        else
            callbackFail?.Invoke();
    }

    public void ShowPopupSkill()
    {
        if (!PlayerData.Instance.PlayerProp.IsMaxAllSkill() && GetShowPopupToday(IAPType.SkillPack6) && _listActivePack.Contains(IAPType.SkillPack6))
        {
            PopupManager.Instance.OnShowScreen(PopupName.Pack, new PopupPack.Param { Type = IAPType.SkillPack6 });
            PlayerData.Instance.PlayerPack.SetCanShowIconPack(IAPType.SkillPack6, true);
        }
    }

    public void ShowPackUpgradeShurikenMax()
    {
        if (!GetShowPopupToday(IAPType.UpgradeShurikenMax) || !_listActivePack.Contains(IAPType.UpgradeShurikenMax))
            return;
        PlayerData.Instance.PlayerPack.SetCanShowIconPack(IAPType.UpgradeShurikenMax, true);
        PopupManager.Instance.OnShowScreen(PopupName.Pack, new PopupPack.Param { Type = IAPType.UpgradeShurikenMax });
        OnUpdatePack?.Invoke();
    }    

    public void ShowInventoryPack()
    {
        var timeFromeLastShow = DateTime.UtcNow - _timeShowPopupInventory;
        if (PlayerData.Instance.Inventory.TotalSlot < GameDefine.MaxSlotItem && GetShowPopupToday(IAPType.InventorySlot) && timeFromeLastShow >= TimeSpan.FromMinutes(5))
        {
            _timeShowPopupInventory = DateTime.UtcNow;
            PlayerData.Instance.PlayerPack.SetCanShowIconPack(IAPType.InventorySlot, true);
            PopupManager.Instance.OnShowScreen(PopupName.Pack, new PopupPack.Param { Type = IAPType.InventorySlot });
            OnUpdatePack?.Invoke();
        }
    }

    public void ShowPackWhenBackToVillage()
    {
        if (PopupManager.Instance.IsPopupShowing(PopupName.Dialog) || TutorialManager.Instance.IsShowingTutorial())
        {
            return;
        }
        DOVirtual.DelayedCall(0.7f, () =>
        {
            //CheckListShowWhenBackVillage(IAPType.Telekinesis);
            CheckListShowWhenBackVillage(IAPType.PermanentRemoveAds);
            CheckListShowWhenBackVillage(IAPType.SkillPack6);
            CheckListShowWhenBackVillage(IAPType.UpgradeShuriken);

            if (listBackVillage.Count > 0)
            {
                var type = listBackVillage[0];
                listBackVillage.Remove(type);
                listBackVillage.Add(type);
                PopupManager.Instance.OnShowScreen(PopupName.Pack, new PopupPack.Param { Type = type });
                PlayerData.Instance.PlayerPack.SetCanShowIconPack(type, true);
                OnUpdatePack?.Invoke();
            }
        });
    }
    
    private void CheckListShowWhenBackVillage(IAPType type)
    {
        if(GetShowPopupToday(type) && ListActivePack.Contains(type))
        {
            if (!listBackVillage.Contains(type))
                listBackVillage.Add(type);
        }
        else
            RemoveListShowWhenBackVillage(type);
    }

    private void RemoveListShowWhenBackVillage(IAPType type)
    {
        if (listBackVillage.Contains(type))
            listBackVillage.Remove(type);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        if (product.definition.id == dictProductIDs.DictProductID[IAPType.BossHunter])
        {
            cbBuyBossHunterFail?.Invoke();
            cbBuyBossHunterFail = null;
        }
        AnalyticsManager.Instance.LogEventBuyItem(AnalyticEventID.buy_pack_ev, product.definition.id, 0);
    }

    public Product GetProduct(IAPType type)
    {
        var productId = dictProductIDs.DictProductID.ContainsKey(type) ? dictProductIDs.DictProductID[type] : string.Empty;
        if (string.IsNullOrEmpty(productId) || Products == null)
            return null;
        return Products.WithID(productId);
    }
    */

    public string GetTxtPrice(IAPType type)
    {
        /*//var product = GetProduct(type);
        if (product != null)
        {
            if (product.metadata.isoCurrencyCode == CurrencyCode.JPY.ToString())
            {
                string txt = "¥";
                decimal num = product.metadata.localizedPrice;
                string num1 = num.ToString("###,###,###");
                return (txt + num1);
            }
            else
                return product.metadata.localizedPriceString;
        }
        else
            return string.Empty;*/
        return null;
    }
    
    /// <summary>
    /// iOS Specific.
    /// This is called as part of Apple's 'Ask to buy' functionality,
    /// when a purchase is requested by a minor and referred to a parent
    /// for approval.
    /// 
    /// When the purchase is approved or rejected, the normal purchase events
    /// will fire.
    /// </summary>
    /// <param name="item">Item.</param>

    [System.Serializable]
    public class HealRank
    {
        public int TotalRebirth;
        public int HPMin;
        public int HPMax;
    }

    public class PackDelayInfo
    {
        public DateTime TimeShowPack = DateTime.MinValue;
        public int TotalMinDelay;
        public int TotalRebirth;
    }
}

// The following classes are used to deserialize JSON results provided by IAP Service
// Please, note that Json fields are case-sensetive and should remain fields to support Unity Deserialization via JsonUtilities
public class JsonData
{
    public string orderId;
    public string packageName;
    public string productId;
    public long purchaseTime;
    public int purchaseState;
    public string purchaseToken;
}
