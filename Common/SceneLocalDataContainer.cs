
namespace GameFramework
{
    public class SceneLocalDataContainer : MonoSingleton<SceneLocalDataContainer>
    {
        public static T Get<T>() where T : SceneLocalData
        {
            return Instance.GetInternal<T>();
        }

        private T GetInternal<T>() where T : SceneLocalData
        {
            return gameObject.GetOrAddComponent<T>();
        }
    }
}
