
namespace GameFramework
{
    public interface IEntityComponent
    {
        IEntity Entity { get; }

        void OnAttached(IEntity entity);
        void OnDetached();
    }
}
