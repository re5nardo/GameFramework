using UnityEngine;

namespace GameFramework
{
    public abstract class Game : MonoBehaviour
    {
        public static Game Current = null;

        public int CurrentTick { get { return tickUpdater.CurrentTick; } }
        public float TickInterval { get { return tickUpdater.TickInterval; } }
        public float GameTime { get { return tickUpdater.CurrentTick * tickUpdater.TickInterval; } }

        protected TickUpdater tickUpdater = null;

        protected abstract void Initialize();

        public void Run()
        {
            Current = this;

            Initialize();

            tickUpdater.Run();
        }

        protected virtual void OnDestroy()
        {
            if (Current == this)
            {
                Current = null;
            }
        }
    }
}
