using UnityEngine;

namespace GameFramework
{
    public abstract class Game : MonoBehaviour
    {
        public static Game Current { get; private set; }

        public int CurrentTick { get { return tickUpdater.CurrentTick; } }
        public int SyncTick { get { return tickUpdater.SyncTick; } }
        public float TickInterval { get { return tickUpdater.TickInterval; } }
        public float GameTime { get { return tickUpdater.CurrentTick * tickUpdater.TickInterval; } }

        protected TickUpdater tickUpdater = null;

        public abstract void Initialize();

        public void Run(int tick = 0)
        {
            Current = this;

            tickUpdater.Run(tick);
        }

        protected virtual void OnDestroy()
        {
            if (Current == this)
            {
                Current = null;
            }
        }

        public void SetSyncTick(int tick)
        {
            tickUpdater.SyncTick = tick;
        }
    }
}
