
namespace GameFramework
{
    public interface IGameEvent
    {
        int seq { get; }
        int tick { get; set; }
    }
}
