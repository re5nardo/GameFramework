
namespace GameFramework
{
    public interface ISynchronizable
    {
        ISynchronizable Parent { get; set; }

        bool Enable { get; set; }
        bool EnableInHierarchy { get; }

        bool HasCoreChange { get; }

        bool IsDirty { get; }
        void SetDirty();

        ISnap GetSnap();

        void UpdateSynchronizable();
        void SendSynchronization();
        void OnReceiveSynchronization(ISnap snap);
        void Reconcile(ISnap snap);
    }
}
