
namespace GameFramework
{
    public interface IComponent
    {
        IEntity Entity { get; }

        void OnCommand(ICommand command);
        void OnAttached(IEntity entity);
        void OnDetached();
    }
}