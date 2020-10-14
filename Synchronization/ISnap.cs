
namespace GameFramework
{
    public interface ISnap
    {
        int Tick { get; set; }
        string Id { get; set; }

        ISnap Clone();
        bool EqualsMeaningfully(ISnap snap);
    }
}
