using UnityEngine;

namespace GameFramework
{
    public class Game : MonoBehaviour
    {
        public int CurrentTick { get { return tickUpdater.CurrentTick; } }
        public float TickInterval { get { return tickUpdater.TickInterval; } }
        public float GameTime { get { return tickUpdater.CurrentTick * tickUpdater.TickInterval; } }
    
        private TickUpdater tickUpdater = null;

        protected virtual void Initialize()
        {
            tickUpdater = gameObject.AddComponent<TickUpdater>();
        }

        public void Run()
        {
            Initialize();

            tickUpdater.Run();
        }
    }
}
