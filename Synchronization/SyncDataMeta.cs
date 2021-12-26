using System;

namespace GameFramework
{
    [Serializable]
    public struct SyncDataMeta
    {
        public int tick;
        public string type;
        public string userId;
        public int entityId;
        public byte[] hash;

        public SyncDataMeta(int tick, string type, string userId, int entityId, byte[] hash)
        {
            this.tick = tick;
            this.type = type;
            this.userId = userId;
            this.entityId = entityId;
            this.hash = hash;
        }
    }
}
