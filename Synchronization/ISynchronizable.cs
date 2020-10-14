
namespace GameFramework
{
    public interface ISynchronizable
    {
        ISynchronizable Parent { get; set; }

        bool Enable { get; set; }
        bool EnableInHierarchy { get; }

        int WaitingInterval { get; }
        ISnap LastSendSnap { get; }

        bool HasMeaningfulChange { get; }

        bool IsDirty { get; }
        void SetDirty();

        bool IsValidToSend { get; }

        ISnap GetSnap();

        void SendSynchronization(bool checkCondition = true);
        void OnReceiveSynchronization(ISnap snap);

        void Reconcile(ISnap snap);
    }
}
