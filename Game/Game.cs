using UnityEngine;
using System.Collections;
using System.Threading.Tasks;

namespace GameFramework
{
    public abstract class Game : MonoBehaviour
    {
        public static Game Current { get; protected set; } = null;

        public int CurrentTick => tickUpdater.CurrentTick;
        public int SyncTick => tickUpdater.SyncTick;
        public double TickInterval => tickUpdater.TickInterval;
        public double GameTime => tickUpdater.ElapsedTime;
        public bool Initialized { get; protected set; } = false;

        protected TickUpdater tickUpdater = null;

        public abstract Task Initialize();
        protected virtual void Clear() {}

        private void Awake()
        {
            Current = this;
        }

        protected virtual void OnDestroy()
        {
            if (Current == this)
            {
                Current = null;
            }

            Clear();
        }

        public void Run(int tick = 0)
        {
            OnBeforeRun();

            tickUpdater.Run(tick);
        }

        protected virtual void OnBeforeRun()
        {
        }

        public void SetSyncTick(int tick)
        {
            tickUpdater.SyncTick = tick;
        }
    }
}
