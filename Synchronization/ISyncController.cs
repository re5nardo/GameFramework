
namespace GameFramework
{
    public interface ISyncController<T> where T : ISyncData
    {
        int EntityId { get; }
        string OwnerId { get; }
        bool HasAuthority { get; }
        bool IsDirty { get; }

        void SetDirty();
        T GetSyncData();

        void Sync(T value);
        void OnSync(T value);
        void OnSyncDataEntry(SyncDataEntry value);
    }
}
