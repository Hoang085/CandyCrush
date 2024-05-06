using Bayat.Json;
using H2910.Defines;
using Honeti;

namespace H2910.PlayerData.Models
{
    public class PlayerPropModel : BaseModel
    {
        private int _gold;
        public string PlayerName;
        public int Gold => _gold;
        private string _deviceID;
        public LanguageCode LanguageType = LanguageCode.EN;

        public override void InitData()
        {
            base.InitData();
            _gold = 0;
            PlayerName = string.Empty;
            PlayerName = $"Player{UnityEngine.Random.Range(10000, 99999)}";
        }

        public PlayerPropModel()
        {
            _modelKey = ModelKey.PlayerProp.ToString();
        }

        public override string ToJson()
        {
            return JsonConvert.SerializeObject(ExportData());
        }

        public override void FromJson(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                InitData();
                return;
            }

            var cachedInfo = JsonConvert.DeserializeObject<PlayerPropData>(json);
            FromData(cachedInfo);
        }

        internal void SetResource(ResourceType type, int quantity)
        {
            switch (type)
            {
                case ResourceType.Gold:
                    _gold = quantity;
                    break;
                default:
                    break;
            }
        }

        internal void AddResource(ResourceType type, int quantity)
        {
            switch (type)
            {
                case ResourceType.Gold:
                    _gold += quantity;
                    break;
                default:
                    break; 
            }
            SaveLocalAsync();
        }

        public void UseGold(int number, string useTarget)
        {
            _gold -= number;
            SaveLocalAsync();
        }

        public void FromData(PlayerPropData data)
        {
            if(data==null)
                return;
            _gold = data.Gold;
            Version = data.Version;
            LanguageType = data.LanguageType;
            PlayerName = string.IsNullOrEmpty(data.PlayerName)
                ? $"Player{UnityEngine.Random.Range(10000, 99999)}"
                : data.PlayerName;
             
        }
        
        public PlayerPropData ExportData()
        {
            var playerPropData = new PlayerPropData();
            
            playerPropData.Gold = _gold;
            playerPropData.Version = Version;
            playerPropData.PlayerName = PlayerName ;
            playerPropData.LanguageType = LanguageType;
           
            return playerPropData;
        }
    }
}