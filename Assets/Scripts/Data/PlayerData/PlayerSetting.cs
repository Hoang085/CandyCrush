using System;
using System.Threading.Tasks;
using DG.Tweening;
using H2910.Defines;
using H2910.PlayerData.Models;

namespace H2910.PlayerData
{
    public class PlayerSetting : PlayerModelBase
    {
        private readonly PlayerSettingModel _playerModel = new PlayerSettingModel();
        internal float BGMValue => _playerModel.BGMValue;
        internal float SFXValue => _playerModel.SFXValue;
        internal InputType InputType => _playerModel.InputType;

        internal override async Task Load(string data)
        {
            try
            {
                if (data != string.Empty)
                    _playerModel.FromJson(data);
                else
                {
                    await _playerModel.LoadLocalAsync();
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        internal string ExportData()
        {
            return _playerModel.ToJson();
        }

        internal override async void SaveLocal()
        {
            await _playerModel.SaveLocalAsync();
        }

        internal void SaveBGMValue(float value)
        {
            _playerModel.BGMValue = value;
            SaveLocal();
        }
        
        internal void SaveSFXValue(float value)
        {
            _playerModel.SFXValue = value;
            SaveLocal();
        }
        
        internal void SaveInputType(InputType inputType)
        {
            _playerModel.InputType = inputType;
            SaveLocal();
        }

        internal override async void SaveLocalDirty()
        {
            if(_isWaiting)
                return;
            DOVirtual.DelayedCall(0.5f, () =>
            {
                _isWaiting = false;
            },true);
            await _playerModel.SaveLocalAsync();
            _isWaiting = true;
        }

        internal void SetMainTutorialType(MainTutorialType type)
        {
            if (_playerModel.TutorialType == MainTutorialType.None && type != MainTutorialType.None)
            {
                _playerModel.TutorialType = type;
                SaveLocal();
            }
        }

        internal MainTutorialType GetMainTutorialType()
        {
            if (_playerModel.TutorialType == MainTutorialType.None)
                SetMainTutorialType(MainTutorialType.Default);
            return _playerModel.TutorialType;
        }
    }
}