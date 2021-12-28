using System;

namespace GameFramework
{
    [Serializable]
    public struct SyncControllerData
    {
        public string controllerId;
        public string ownerId;
        
        public SyncControllerData(string controllerId, string ownerId)
        {
            this.controllerId = controllerId;
            this.ownerId = ownerId;
        }
    }
}
