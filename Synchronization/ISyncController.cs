
namespace GameFramework
{
    public interface ISyncController<T> where T : ISyncData
    {
        string ControllerId { get; }
        string OwnerId { get; }
        bool HasAuthority { get; }
        bool IsDirty { get; }
        SyncScope SyncScope { get; }

        void SetDirty();

        T GetSyncData();
        SyncDataEntry GetSyncDataEntry();
        SyncControllerData GetSyncControllerData();

        void Sync(T value);

        void OnSync(T value);
        void OnSync(SyncDataEntry value);

        void OnInitialize();
        void OnFinalize();
    }
}
