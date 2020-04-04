
namespace GameFramework
{
    public interface IHandler<T>
    {
        void Handle(T target);
    }
}
