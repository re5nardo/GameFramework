using UnityEngine;

namespace GameFramework
{
    public abstract class SceneLocalData : MonoBehaviour
    {
        private void Awake()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
        }
    }
}
