
namespace GameFramework
{
    public interface ISyncControllerBase
    {
        string ControllerId { get; }
        string OwnerId { get; }
        bool HasAuthority { get; }
        bool IsDirty { get; }
        SyncScope SyncScope { get; }

        void SetDirty();
        
        SyncDataEntry GetSyncDataEntry();
        SyncControllerData GetSyncControllerData();

        void OnSync(SyncDataEntry value);

        void OnInitialize();
        void OnFinalize();
    }

    public interface ISyncController<T> : ISyncControllerBase where T : ISyncData
    {
        T GetSyncData();

        void Sync(T value);
        void OnSync(T value);
    }
}
