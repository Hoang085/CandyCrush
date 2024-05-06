using Bayat.Json;
using H2910.Defines;

namespace H2910.PlayerData.Models
{
    public class PlayerSettingModel : BaseModel
    {
        internal float BGMValue = 100;
        internal float SFXValue = 100;
        internal InputType InputType = InputType.Button;
        internal MainTutorialType TutorialType = MainTutorialType.None;

        internal PlayerSettingModel()
        {
            _modelKey = ModelKey.PlayerSettings.ToString();
        }

        public override string ToJson()
        {
            return JsonConvert.SerializeObject(ExportData());
        }

        public override void InitData()
        {
            base.InitData();
            BGMValue = 100;
            SFXValue = 100;
            InputType = InputType.Button;
            TutorialType = MainTutorialType.None;
        }

        public override void FromJson(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                InitData();
                return;
            }

            var cachedInfo = JsonConvert.DeserializeObject<PlayerSettingData>(json);
            FromData(cachedInfo);
        }

        internal void FromData(PlayerSettingData data)
        {
            if (data==null)
            {
                return;
            }

            BGMValue = data.BGMValue;
            SFXValue = data.SFXValue;
            InputType = data.InputType;
            TutorialType = data.TutorialType;
        }

        internal PlayerSettingData ExportData()
        {
            var data = new PlayerSettingData();
            data.BGMValue = BGMValue;
            data.SFXValue = SFXValue;
            data.InputType = InputType;
            data.TutorialType = TutorialType;
            return data;
        }
        
    }
}