
namespace GameFramework
{
    public interface IEntityComponent
    {
        IEntity Entity { get; }

        void Attach(IEntity entity);
        void Detach();
    }
}
