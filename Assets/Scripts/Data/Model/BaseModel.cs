using System;
using System.Threading.Tasks;
using Bayat.SaveSystem;

namespace H2910.PlayerData.Models
{
    public abstract class BaseModel
    {
        public int Version;
        protected string _modelKey = "";
        protected bool _isDirty;
        public abstract string ToJson();

        public virtual void InitData()
        {
            Version = 0;
        }

        public abstract void FromJson(String json);

        protected string GetFileName()
        {
            return _modelKey;
        }

        public virtual async Task<string> LoadLocalAsync()
        {
            var file = GetFileName();
            var isExisted = await SaveSystemAPI.ExistsAsync(file);
            string json = null;
            if (isExisted)
                json = await SaveSystemAPI.LoadAsync<string>(GetFileName());
            FromJson(json);
            return json;
        }

        public virtual async Task SaveLocalAsync()
        {
            if(_isDirty)
                return;
            _isDirty = true;
            Version++;
            await SaveSystemAPI.SaveAsync(GetFileName(), ToJson());
            _isDirty = false;
        }
    }
}