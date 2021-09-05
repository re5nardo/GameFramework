
namespace GameFramework
{
    public interface IEntityComponent
    {
        IEntity Entity { get; }

        void OnCommand(ICommand command);
        void OnAttached(IEntity entity);
        void OnDetached();
    }
}