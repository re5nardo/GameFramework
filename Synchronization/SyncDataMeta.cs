using System;

namespace GameFramework
{
    [Serializable]
    public struct SyncDataMeta
    {
        public int tick;
        public string userId;
        public string controllerId;
        public byte[] hash;

        public SyncDataMeta(int tick, string userId, string controllerId, byte[] hash)
        {
            this.tick = tick;
            this.userId = userId;
            this.controllerId = controllerId;
            this.hash = hash;
        }
    }
}
