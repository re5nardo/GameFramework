using System;

namespace GameFramework
{
    [Serializable]
    public struct SyncDataMeta
    {
        public int tick;
        public string senderId;
        public string controllerId;
        public byte[] hash;

        public SyncDataMeta(int tick, string senderId, string controllerId, byte[] hash)
        {
            this.tick = tick;
            this.senderId = senderId;
            this.controllerId = controllerId;
            this.hash = hash;
        }
    }
}
