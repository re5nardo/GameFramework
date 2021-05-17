
namespace GameFramework
{
    public interface ISynchronizable
    {
        bool Enable { get; set; }

        bool HasCoreChange { get; }

        bool IsDirty { get; }
        void SetDirty();

        ISnap GetSnap();

        void UpdateSynchronizable();
        void SendSynchronization();
        void OnReceiveSynchronization(ISnap snap);
    }
}
