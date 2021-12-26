using System;

namespace GameFramework
{
    [Serializable]
    public struct SyncControllerData
    {
        public string type;
        public int entityId;
        public string ownerId;
        
        public SyncControllerData(string type, int entityId, string ownerId)
        {
            this.type = type;
            this.entityId = entityId;
            this.ownerId = ownerId;
        }
    }
}
