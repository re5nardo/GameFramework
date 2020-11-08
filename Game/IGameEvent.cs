
namespace GameFramework
{
    public interface IGameEvent
    {
        int Seq { get; }
        int Tick { get; set; }
    }
}
