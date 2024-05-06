using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bayat.Json;
using H2910.Common.Singleton;
using H2910.Defines;
using H2910.PlayerData;
using Honeti;
using UnityEditor;

namespace H2910.Data
{
    [CreateAssetMenu(menuName = "Data/PlayerData")]
    public class PlayerData : ManualSingletonMono<PlayerData>
    {
        #region Static Data

        internal readonly PlayerProp PlayerProp = new PlayerProp();
        internal readonly PlayerSetting PlayerSetting = new PlayerSetting();

        #endregion

        private static List<ModelKey> _lstMainDataKeys = new List<ModelKey>
        {
            ModelKey.PlayerProp,
            ModelKey.MapInfo,
        };

        private static List<ModelKey> _lstSubDataKeys = new List<ModelKey>
        {
            ModelKey.Quest,
            ModelKey.DailyAndOnlineReward,
            ModelKey.Pack,
        };

        private List<ModelKey> _lstSecondaryDataKeys = new List<ModelKey>
        {
            ModelKey.AdventureShop,
            ModelKey.DailyGift,
            ModelKey.PlayerSettings,
            ModelKey.Mail,
            ModelKey.LuckyWheel,
        };


        private async void Start()
        {
#if UNITY_EDITOR || DEV
            Debug.unityLogger.logEnabled = true;
#else
            Debug.unityLogger.logEnabled = false;
#endif
            //await Setup(null, true);
        }

        /*internal async Task Setup(Dictionary<ModelKey, string> dctDatas, bool firstSetup = false)
        {
            if (dctDatas == null)
            {
                dctDatas = new Dictionary<ModelKey, string>();
            }

            if(dctDatas.ContainsKey(ModelKey.SubData2))
            {
                var subData = JsonConvert.DeserializeObject<Dictionary<ModelKey, string>>(dctDatas[ModelKey.SubData]);
                if(subData != null)
                {
                    foreach (var model in _lstSubDataKeys)
                    {
                        if (subData.ContainsKey(model))
                        {
                            if (!dctDatas.ContainsKey(model))
                                dctDatas.Add(model, subData[model]);
                            else
                                dctDatas[model] = subData[model];
                        }
                    }
                }
            }

            if (dctDatas.ContainsKey(ModelKey.SubData))
            {
                var subData = JsonConvert.DeserializeObject<Dictionary<ModelKey, string>>(dctDatas[ModelKey.SubData2]);
                if (subData != null)
                {
                    foreach (var model in _lstSecondaryDataKeys)
                    {
                        if (!dctDatas.ContainsKey(model))
                            dctDatas.Add(model, subData[model]);
                        else
                            dctDatas[model] = subData[model];
                    }
                }
            }

            EnumExtensions.ForeachEnum<ModelKey>(value =>
            {
                if (!dctDatas.ContainsKey(value))
                    dctDatas[value] = string.Empty;
            });

            await Task.WhenAll(
                               PlayerProp.Load(dctDatas[ModelKey.PlayerProp]),
                               PlayerMapInfo.Load(dctDatas[ModelKey.MapInfo]),
                               PlayerQuest.Load(dctDatas[ModelKey.Quest]),
                               PlayerAdventureShop.Load(dctDatas[ModelKey.AdventureShop]),
                               PlayerLuckyWheel.Load(dctDatas[ModelKey.LuckyWheel]),
                               PlayerPack.Load(dctDatas[ModelKey.Pack]),
                               PlayerDailyReward.Load(dctDatas[ModelKey.DailyAndOnlineReward]),
                               PlayerMail.Load(dctDatas[ModelKey.Mail]),
                               PlayerSettings.Load(dctDatas[ModelKey.PlayerSettings]),
                               PlayerDailyGift.Load(dctDatas[ModelKey.DailyGift])
                            );

            InitData = true;
            StartCoroutine(DelayGenerate(firstSetup));
        }

        private IEnumerator DelayGenerate(bool firstSetup)
        {
            yield return new WaitForEndOfFrame();
            NoticeManager.Instance.Init();
            MainStats.Instance.Init();
            UpdateManager.Instance.InitTime(firstSetup, true);
            if(firstSetup)
            {
                I18N.Instance.LoadData(()=> {
                    I18N.Instance.Init();
                    if (!PlayerProp.IsUnlockFeature(UnlockFeature.CompleteMainTutorial))
                    {
                        _coroutineIP = StartCoroutine(GetIPData.IERequestToIpAPI());
                        DOVirtual.DelayedCall(2f, () => {
                            if (_coroutineIP != null)
                                StopCoroutine(_coroutineIP);
                        });
                    }
                });
            }
            //Note: Lock Notification
            //LocalNotification.Instance.Init();
            if (PlayerMail.ListMails.Count > 0)
            {
                NoticeManager.Instance.TriggerEvent(Notice.Mail, true);
            }
            loginTime = DateTime.UtcNow;
            yield return new WaitForSeconds(0.5f);
            UIManager.Instance.Init();
            OnInitDataComplete?.Invoke();
            LogMessage.Log("Init Data", LogMessage.LogMessageType.PlayerData);
        }

        private void OnApplicationPause(bool pause)
        {
            if(pause)
                SaveAllDataToPlayfab(null, true);
        }

        private void OnApplicationQuit()
        {
            var exitTime = DateTime.UtcNow;
            var timePlayed = exitTime - loginTime;

            foreach (var dict in PlayerQuest.DictActiveQuest)
            {
                if (dict.Value.TimeQuestInprogress == 0 && dict.Value.IsAccepted)
                {
                    PlayerQuest.UpdateLastUpdateQuestInProgressWhenExit(dict.Key);
                    PlayerQuest.UpdateTimeQuestInProgressWhenExit(dict.Key, (long)(exitTime - dict.Value.TimeAcceptQuest).Value.TotalSeconds);
                }
                else if (dict.Value.TimeQuestInprogress != 0 && dict.Value.IsAccepted)
                {
                    if (dict.Value.LastUpdateQuestInProgressTime.Value < exitTime && dict.Value.LastUpdateQuestInProgressTime.Value > loginTime)
                    {
                        PlayerQuest.UpdateTimeQuestInProgressWhenExit(dict.Key, dict.Value.TimeQuestInprogress +
                            (long)timePlayed.TotalSeconds - (long)(dict.Value.LastUpdateQuestInProgressTime.Value - loginTime).TotalSeconds);
                    }
                    else
                    {
                        PlayerQuest.UpdateTimeQuestInProgressWhenExit(dict.Key, dict.Value.TimeQuestInprogress + (long)timePlayed.TotalSeconds);
                    }
                }
            }
        }*/

        /*internal void SaveDataToServer()
        {
            if(AuthenManager.Instance.FirebaseUser != null)
            {
                Dictionary<ModelKey, string> dctDatas = new Dictionary<ModelKey, string>();
                var keys = EnumExtensions.GetAllItems<ModelKey>();
                foreach(var key in keys)
                {
                    dctDatas.Add(key, ExportModelData(key));
                }
                firebaseRealtime.UploadDataToServer(dctDatas);
            }
        }#1#
        public void SaveAllDataToPlayfab(Action<bool> callback, bool forceSave = false, bool delay = false)
        {
            if (!AuthenPlayFabManager.Instance.IsPlayFabLoggedin)
                return;
            LogMessage.Log("Save all data to playfab", LogMessage.LogMessageType.PlayerData);
            PlayerProp.CachePlayfabId();
            Dictionary<ModelKey, string> dctSubDatas = new Dictionary<ModelKey, string>();
            Dictionary<ModelKey, string> dctSubDatas2 = new Dictionary<ModelKey, string>();
            Dictionary<ModelKey, string> dctData = new Dictionary<ModelKey, string>();
            EnumExtensions.ForeachEnum<ModelKey>(enumValue =>
            {
                if (_lstMainDataKeys.Contains(enumValue))
                    dctData.Add(enumValue, ExportModelData(enumValue));
                else if (_lstSubDataKeys.Contains(enumValue))
                    dctSubDatas.Add(enumValue, ExportModelData(enumValue));
                else if (_lstSecondaryDataKeys.Contains(enumValue))
                    dctSubDatas2.Add(enumValue, ExportModelData(enumValue));
            });

            string subDatas = JsonConvert.SerializeObject(dctSubDatas);
            string subDatas2 = JsonConvert.SerializeObject(dctSubDatas2);

            dctData.Add(ModelKey.SubData, subDatas);
            dctData.Add(ModelKey.SubData2, subDatas2);

            if (forceSave)
            {
                PlayFabManager.UserData.SetData(dctData, (mainResultType) =>
                {
                    if (mainResultType == PlayFabApiResultType.Error)
                    {
                        Debug.LogError(string.Format("Error!!! Something wrong when save main data to playfab..."));
                        callback?.Invoke(false);
                        return;
                    }
                    /*PlayFabManager.UserData.SetData(dctData, (subResultType) =>
                    {
                        if (subResultType == PlayFabApiResultType.Error)
                        {
                            Debug.LogError(string.Format("Error!!! Something wrong when save sub data (Part 1) to playfab..."));
                            callback?.Invoke(false);
                            return;
                        }
                        LogMessage.Log("save data server complete", LogMessage.LogMessageType.PlayerData);
                        Debug.Log(string.Format("TitleData save SUBDATA (Part 1) success"));
                        callback?.Invoke(true);
                    }, forceSave);#1#
                    Debug.Log(string.Format("TitleData save MAIN success"));
                }, forceSave);
            }
            else
            {
                if (H2910Utils.UtcSecondNow - _lastCloudSaveTime <= GameDefine.PlayFabUpdateDataDelayTime && delay)
                {
                    callback?.Invoke(false);
                    return;
                }

                CloudScriptManager.CallCloudScript<GetDeviceLoginRespone>(new GetDeviceLoginRequest(), (data) => {
                    if (!string.IsNullOrEmpty(data.DeviceId) && PlayFabUtils.GetDeviceId() != data.DeviceId)
                    {
                        //Debug.LogError("Device id change!!!");
                        AuthenPlayFabManager.Instance.LogOut();
                        callback?.Invoke(false);
                        return;
                    }
                    else
                    {
                        PlayFabManager.UserData.SetData(dctData, (mainResultType) =>
                        {
                            if (mainResultType == PlayFabApiResultType.Error)
                            {
                                Debug.LogError(string.Format("Error!!! Something wrong when save main data to playfab..."));
                                callback?.Invoke(false);
                                return;
                            }

                            /*PlayFabManager.UserData.SetData(dctSubDatas, (subResultType) =>
                            {
                                if (subResultType == PlayFabApiResultType.Error)
                                {
                                    Debug.LogError(string.Format("Error!!! Something wrong when save sub data (Part 1) to playfab..."));
                                    callback?.Invoke(false);
                                    return;
                                }
                                LogMessage.Log("save data server complete", LogMessage.LogMessageType.PlayerData);
                                _lastCloudSaveTime = H2910Utils.UtcSecondNow;
                                callback?.Invoke(true);
                            }, forceSave);#1#
                        }, forceSave);
                    }
                }
                , (error) =>
                {
                    Debug.LogError(error.ErrorMessage);
                });
            }
        }

        internal void SaveCustomDataToPlayfab(List<ModelKey> listModel, Action<bool> callback = null)
        {
            Dictionary<ModelKey, string> dctDatas = new Dictionary<ModelKey, string>();
            for(int i = 0; i < listModel.Count;i++)
            {
                if (_lstMainDataKeys.Contains(listModel[i]))
                    dctDatas.Add(listModel[i], ExportModelData(listModel[i]));
            }

            CloudScriptManager.CallCloudScript<GetDeviceLoginRespone>(new GetDeviceLoginRequest(), (data) => {
                if (!string.IsNullOrEmpty(data.DeviceId) && PlayFabUtils.GetDeviceId() != data.DeviceId)
                {
                    Debug.LogError("Device id change!!!");
                    AuthenPlayFabManager.Instance.LogOut();
                    callback?.Invoke(false);
                    return;
                }
                else
                {
                    PlayFabManager.UserData.SetData(dctDatas, (mainResultType) =>
                    {
                        if (mainResultType == PlayFabApiResultType.Error)
                        {
                            Debug.LogError(string.Format("Error!!! Something wrong when save main data to playfab..."));
                            callback?.Invoke(false);
                            return;
                        }
                        callback?.Invoke(true);
                    }, false);
                }
            }
            , (error) =>
            {
                Debug.LogError(error.ErrorMessage);
            });
        }

        internal void Logout()
        {
            InitData = false;
            MainStats.Instance.Logout();
            UIManager.Instance.Logout();
        }

        internal async Task CheckDataFromServerAfterLoginSuccess(Dictionary<ModelKey, string> dctCloudDatas, Action<LoginAccountStatus> callback)
        {
            if(dctCloudDatas == null)
            {
                LogMessage.Log("cloud null", LogMessage.LogMessageType.PlayerData);
                CheckNewAccount(dctCloudDatas, callback);
                return;
            }
            if(!dctCloudDatas.ContainsKey(ModelKey.PlayerProp))
            {
                LogMessage.Log("cloud prop null", LogMessage.LogMessageType.PlayerData);
                CheckNewAccount(dctCloudDatas, callback);
                return;
            }
            else
            {
                var cloudPropData = JsonConvert.DeserializeObject<PlayerPropData>(dctCloudDatas[ModelKey.PlayerProp]);
                if(cloudPropData == null)
                {
                    LogMessage.Log("cloud propdata null after convert", LogMessage.LogMessageType.PlayerData);
                    CheckNewAccount(dctCloudDatas, callback);
                    return;
                }

                if (cloudPropData.DeviceID == PlayerProp.DeviceId && AuthenPlayFabManager.Instance.PlayfabId == PlayerProp.PlayfabID)
                {
                    if (cloudPropData.Version < PlayerProp.VersionData)
                    {
                        await Task.Delay(1000);
                        SaveAllDataToPlayfab(null, true);
                        SaveLocal();
                        callback?.Invoke(LoginAccountStatus.CurrentUser);
                        LogMessage.Log("check user verion local > cloud", LogMessage.LogMessageType.PlayerData);
                    }
                    else
                    {
                        await SaveSystemAPI.ClearAsync();
                        Logout();
                        await Task.Delay(500);
                        await Setup(dctCloudDatas);
                        await Task.Delay(500);
                        SaveAllDataToPlayfab(null, true);
                        LogMessage.Log("check user version local < clound", LogMessage.LogMessageType.PlayerData);
                        SaveLocal();
                        callback?.Invoke(LoginAccountStatus.SelectDataFromServer);
                    }
                    return;
                }
                else if(string.IsNullOrEmpty(PlayerProp.PlayfabID) && PlayerProp.IsUnlockFeature(UnlockFeature.CompleteMainTutorial))
                {
                    //PopupManager.Instance.ShowNotice("ID: " + PlayerProp.PlayfabID + " . Main tutorial " + PlayerProp.IsUnlockFeature(UnlockFeature.CompleteMainTutorial));
                    InitData = false;
                    SelectData(dctCloudDatas, callback);
                }
                else if(cloudPropData.DeviceID != PlayerProp.DeviceId && AuthenPlayFabManager.Instance.PlayfabId == PlayerProp.PlayfabID)
                {
                    /*if(AuthenPlayFabManager.Instance.PlayfabId == PlayerProp.PlayfabID)
                        PopupManager.Instance.ShowNotice("ID user");
                    else
                        PopupManager.Instance.ShowNotice("device Id");
                    InitData = false;
                    SelectData(dctCloudDatas, callback);#1#
                    await SaveSystemAPI.ClearAsync();
                    Logout();
                    await Task.Delay(500);
                    await Setup(dctCloudDatas);
                    await Task.Delay(500);
                    SaveAllDataToPlayfab(null, true);
                    SaveLocal();
                    callback?.Invoke(LoginAccountStatus.SelectDataFromServer);
                    LogMessage.Log("check user !Device Id ", LogMessage.LogMessageType.PlayerData);
                }
                else
                {
                    await SaveSystemAPI.ClearAsync();
                    Logout();
                    await Task.Delay(500);
                    await Setup(dctCloudDatas);
                    await Task.Delay(500);
                    SaveAllDataToPlayfab(null, true);
                    SaveLocal();
                    callback?.Invoke(LoginAccountStatus.SelectDataFromServer);
                    LogMessage.Log("check user other ", LogMessage.LogMessageType.PlayerData);
                }
            }
        }

        private async void CheckNewAccount(Dictionary<ModelKey, string> dctCloudDatas, Action<LoginAccountStatus> callback)
        {
            LogMessage.Log($"check user: !account {PlayerProp.PlayfabID} + {AuthenPlayFabManager.Instance.PlayfabId}", LogMessage.LogMessageType.PlayerData);
            if (!string.IsNullOrEmpty(PlayerProp.PlayfabID) && PlayerProp.PlayfabID != AuthenPlayFabManager.Instance.PlayfabId)
            {
                await SaveSystemAPI.ClearAsync();
                Logout();
                await Task.Delay(500);
                await Setup(new Dictionary<ModelKey, string>());
                callback?.Invoke(LoginAccountStatus.SelectDataFromServer);
                await Task.Delay(500);
                SaveAllDataToPlayfab(null, true);
                SaveLocal();
                LogMessage.Log("check user: !account ", LogMessage.LogMessageType.PlayerData);
            }
            else
            {
                LogMessage.Log("check user: new user, save to playfab ", LogMessage.LogMessageType.PlayerData);
                callback?.Invoke(LoginAccountStatus.NewUse);
                SaveAllDataToPlayfab(null, true);
                SaveLocal();
            }
        }

        private void SelectData(Dictionary<ModelKey, string> dctCloudDatas, Action<LoginAccountStatus> callback)
        {
            var currentLevelHightest = PlayerMapInfo.GetHighestLevel();
            if (currentLevelHightest == 0)
                currentLevelHightest = 101;
            int curZone = currentLevelHightest / 100;
            int curLevelMax = currentLevelHightest % 100;
            var cloudPropData = JsonConvert.DeserializeObject<PlayerPropData>(dctCloudDatas[ModelKey.PlayerProp]);
            PopupManager.Instance.OnShowScreen(PopupName.SelectData, new PopupSelectData.Param
            {
                LocalUserInformation = new PopupSelectData.UserInfomation
                {
                    Progress = LanguageUtils.GetLanguageValue(((MapType)curZone).ToString()) + " " + curLevelMax.ToString(),
                    PlayerName = PlayerProp.DisplayName,
                    FlameTrace = PlayerMapInfo.TotalStar,
                    Gem = PlayerProp.Gem,
                    Gold = PlayerProp.Gold
                },
                ServerUserInformation = new PopupSelectData.UserInfomation
                {
                    PlayerName = cloudPropData.PlayerName,
                    Progress = GetMapHighestName(dctCloudDatas[ ModelKey.MapInfo]),
                    FlameTrace = GetTotalFlameCloudData(dctCloudDatas[ModelKey.MapInfo]),
                    Gem = cloudPropData.Gem,
                    Gold = cloudPropData.Gold
                },
                Callback = async (isLocalUser) =>
                {
                    if (isLocalUser)
                    {
                        callback?.Invoke(LoginAccountStatus.selectDataLocal);
                        InitData = true;
                        await Task.Delay(1000);
                        SaveAllDataToPlayfab(null, true);
                        SaveLocal();
                    }
                    else
                    {
                        await SaveSystemAPI.ClearAsync();
                        Logout();
                        await Task.Delay(500);
                        await Setup(dctCloudDatas);
                        callback?.Invoke(LoginAccountStatus.SelectDataFromServer);
                        await Task.Delay(500);
                        SaveAllDataToPlayfab(null, true);
                        SaveLocal();
                    }
                }
            }, ParentPopup.Hight);
        }

        private int GetTotalFlameCloudData(string mapData)
        {
            var cloudMapInfoData = JsonConvert.DeserializeObject<MapInfoData>(mapData);
            if (cloudMapInfoData != null)
            {
                int total = 0;
                foreach (var item in cloudMapInfoData.DictMapUnlockInfo)
                {
                    if ((int)item.Value.LevelMapStatus > 1 && item.Key.Substring(item.Key.Length - 2) != "99")// not check boss map!
                        total += (int)item.Value.LevelMapStatus - 1;
                }
                return total;
            }
            else
                return 0;
        }

        private string GetMapHighestName(string mapData)
        {
            string severMapProgress = string.Empty;
            var cloudMapInfoData = JsonConvert.DeserializeObject<MapInfoData>(mapData);
            if(cloudMapInfoData != null)
            {
                var currentLevelHightest = cloudMapInfoData.LevelHighest;
                int curZone = currentLevelHightest / 100;
                int curLevelMax = currentLevelHightest % 100;
                if (curZone == 0)
                    curZone = 1;
                severMapProgress = LanguageUtils.GetLanguageValue(((MapType)curZone).ToString()) + " " + curLevelMax.ToString();
            }
            return severMapProgress;
        }

        private string ExportModelData(ModelKey key)
        {
            switch (key)
            {
                case ModelKey.Achievement:
                    return PlayerAchievement.ExportData();
                case ModelKey.Inventory:
                    return Inventory.ExportData();
                case ModelKey.PlayerProp:
                    return PlayerProp.ExportData();
                case ModelKey.MapInfo:
                    return PlayerMapInfo.ExportData();
                case ModelKey.LuckyWheel:
                    return PlayerLuckyWheel.ExportData();
                case ModelKey.Quest:
                    return PlayerQuest.ExportData();
                case ModelKey.AdventureShop:
                    return PlayerAdventureShop.ExportData();
                case ModelKey.ItemExchangeShop:
                    return PlayerItemExchangeShop.ExportData();
                case ModelKey.Farming:
                    return PlayerFarming.ExportData();
                case ModelKey.BossAndMiniboss:
                    return PlayerBossManager.ExportData();
                case ModelKey.PlayerVersionCheck:
                    return PlayerVersionCheck.ExportData();
                case ModelKey.Pack:
                    return PlayerPack.ExportData();
                case ModelKey.Mail:
                    return PlayerMail.ExportData();
                case ModelKey.DailyAndOnlineReward:
                    return PlayerDailyReward.ExportData();
                case ModelKey.PlayerSettings:
                    return PlayerSettings.ExportData();
                case ModelKey.DailyGift:
                    return PlayerDailyGift.ExportData();
                case ModelKey.WeaponData:
                    return PlayerWeaponData.ExportData();
                case ModelKey.CraftData:
                    return PlayerCraftData.ExportData();
                case ModelKey.MineralMine:
                    return PlayerMineralData.ExportData();
                default:
                    Debug.LogError(string.Format("Other key {0}", key));
                    return string.Empty;
            }
        }

        public void SaveLocalDelay(int timeDelay)
        {
            SaveAllDataToPlayfab(null, false);
            DOVirtual.DelayedCall(timeDelay, () => {
                SaveLocal();
            });
        }

        internal void InitializeFirebase()
        {
            //firebaseRealtime.InitFirebase();
        }

        public void SaveLocal()
        {
            Inventory.SaveLocal();
            PlayerProp.SaveLocal();
            PlayerMapInfo.SaveLocal();
            PlayerQuest.SaveLocal();
            PlayerAdventureShop.SaveLocal();
            PlayerItemExchangeShop.SaveLocal();
            PlayerLuckyWheel.SaveLocal();
            PlayerFarming.SaveLocal();
            PlayerBossManager.SaveLocal();
            PlayerVersionCheck.SaveLocal();
            PlayerPack.SaveLocal();
            PlayerMail.SaveLocal();
            PlayerAchievement.SaveLocal();
            PlayerDailyGift.SaveLocal();
            PlayerDailyReward.SaveLocal();
            PlayerSettings.SaveLocal();
            PlayerWeaponData.SaveLocal();
            PlayerCraftData.SaveLocal();
            PlayerMineralData.SaveLocal();
        }
        #endregion
    }*/
    }
}
