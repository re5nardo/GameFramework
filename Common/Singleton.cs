
namespace GameFramework
{
    public class Singleton<T> where T : class, new()
    {
        private static T instance = null;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                }

                return instance;
            }
        }

        public static bool IsInstantiated()
        {
            return instance != null;
        }

        public static void Instantiate()
        {
            if (instance == null)
            {
                instance = new T();
            }
        }
    }
}
