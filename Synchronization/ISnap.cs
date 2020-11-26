
namespace GameFramework
{
    public interface ISnap
    {
        int Tick { get; set; }
       
        bool EqualsCore(ISnap snap);
        bool EqualsValue(ISnap snap);

        ISnap Set(ISynchronizable synchronizable);
        ISnap Clone();
    }
}
