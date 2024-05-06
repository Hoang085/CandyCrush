using System;
using System.Threading.Tasks;
using DG.Tweening;
using H2910.Defines;
using H2910.PlayerData.Models;
using Honeti;
using UnityEngine;

namespace H2910.PlayerData
{
    public class PlayerProp : PlayerModelBase
    {
        private readonly PlayerPropModel _playerPropModel = new PlayerPropModel();
        internal int Gold => _playerPropModel.Gold;

        internal int VersionData => _playerPropModel.Version;
        internal LanguageCode LanguageType => _playerPropModel.LanguageType;

        internal string DisplayName => _playerPropModel.PlayerName;

        private int allCoin;
        private int boostCoin;
       
        public int AllCoin => allCoin;

        public int BoostCoin => boostCoin;
        public int LevelVoiceOfWisdom = 1;
        public bool[] LevelResolveStatus = new bool[10];
        public int CurrentIndexLevelResolve =-1;

        internal override async Task Load(string serverData)
        {
            try
            {
                RefreshDataAfterLogin();
                if (serverData != string.Empty)
                {
                    _playerPropModel.FromJson(serverData);
                }    
                else
                    await _playerPropModel.LoadLocalAsync();
                
                for (int i = 0; i < LevelResolveStatus.Length; i++)
                {
                    LevelResolveStatus[i] = false;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("Error!!! Can't load {0}", ex));
                return;
            }
        }

        private void RefreshDataAfterLogin()
        {
            LevelVoiceOfWisdom = 1;
            LevelResolveStatus = new bool[10];
            CurrentIndexLevelResolve = -1;
            allCoin = 0;
            boostCoin = 0;
        }

        internal override async void SaveLocal()
        {
            await _playerPropModel.SaveLocalAsync();
        }

        internal string ExportData()
        {
            return _playerPropModel.ToJson();
        }
        

        public void UpdatePlayerName(string playerName)
        {
            _playerPropModel.PlayerName = playerName;
            SaveLocal();
        }

        internal bool UseGold(int number,string useTaget)
        {
            if (number <= 0) return false;
            if (number > _playerPropModel.Gold)
                return false;
            _playerPropModel.UseGold(number, useTaget);
            SaveLocal();
            return true;
        }   
        
        internal bool UseResource(ResourceType type, int quantity, string useTaget)
        {
            if (quantity <= 0)
            {
                Debug.LogError("Wrong quantity");
                return false;
            }
            
            if (type == ResourceType.Gold)
            {
                if (_playerPropModel.Gold >= quantity)
                {
                    _playerPropModel.UseGold(quantity, useTaget);
                }    
                else
                {
                    Debug.LogError("Wrong number of Gold");
                    return false;
                }
            }
            SaveLocal();
            return true;
        }
        
        internal void SetResource(ResourceType type, int quantity)
        {
            _playerPropModel.SetResource(type, quantity);
            SaveLocal();
        }
        internal void AddResource(ResourceType type, int quantity, bool isBuyFromShop = false,bool canBoost=true, float delayTime = 0)
        {
            if(quantity < 0)
            {
                Debug.LogError("Error quantity < 0 " + type);
                return;
            }

            switch (type)
            {
                case ResourceType.Gold:
                    /*if (!isBuyFromShop)
                    {
                        int boostValue = ItemExchangeShopManager.Instance.GetBoostCoinValue();
                        allCoin += quantity;
                        if (boostValue <= 1 || !canBoost)
                        {
                            _playerPropModel.AddResource(type, quantity);
                        }      
                        else
                        {
                            _playerPropModel.AddResource(type, quantity * boostValue);
                            boostCoin += quantity * (boostValue - 1);
                        }
                    }
                    else*/
                        _playerPropModel.AddResource(type, quantity);
                    break;
                default:
                    _playerPropModel.AddResource(type, quantity);
                    break;
            }
            SaveLocal();
        }
        
        internal override void SaveLocalDirty()
        {
            if (_isWaiting)
                return;

            DOVirtual.DelayedCall(2f, () => {_isWaiting = false; }, true);
            _playerPropModel.SaveLocalAsync();
            _isWaiting = true;
        }
        
        internal void SetLanguageCode(LanguageCode languageCode)
        {
            _playerPropModel.LanguageType = languageCode;
            SaveLocal();
        }

        internal void ResetCurrentCoin()
        {
            allCoin = 0;
            boostCoin = 0;
        }
        internal void IncreaseLevelOfVoiceOfWisdom()
        {
            LevelVoiceOfWisdom++;
        }
        internal void ResetLevelOfVoiceOfWisdom()
        {
            LevelVoiceOfWisdom = 1;
            for (int i = 0; i < LevelResolveStatus.Length; i++)
            {
                LevelResolveStatus[i] = false;
            }
            CurrentIndexLevelResolve = -1;
            UIManager.Instance.ResetCoolDownWisdomBtn();
        }
        internal void SaveLevelResolveStatus(int index)
        {
            LevelResolveStatus[index] = true;
        }
    }
}