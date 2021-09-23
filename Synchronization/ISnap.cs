
namespace GameFramework
{
    public interface ISnap
    {
        int Tick { get; set; }

        ISnap Clone();
    }
}
