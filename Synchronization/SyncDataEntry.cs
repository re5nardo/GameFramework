using System;

namespace GameFramework
{
    [Serializable]
    public struct SyncDataEntry
    {
        public SyncDataMeta meta;
        public ISyncData data;
    }
}
