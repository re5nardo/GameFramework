
namespace GameFramework
{
    public class ModelComponentBase : IModelComponent
    {
        public IEntity Entity { get; private set; }

        public virtual void OnCommand(ICommand command)
        {
        }

        public virtual void OnAttached(IEntity entity)
        {
            Entity = entity;
        }

        public virtual void OnDetached()
        {
            Entity = null;
        }

        public virtual void Initialize(params object[] param)
        {
        }
    }
}
