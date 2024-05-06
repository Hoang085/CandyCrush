using System.Threading.Tasks;

namespace H2910.PlayerData
{
    public abstract class PlayerModelBase
    {
        protected bool _isWaiting;
        internal abstract void SaveLocal();
        internal abstract Task Load(string data);
        internal abstract void SaveLocalDirty();
    }
}