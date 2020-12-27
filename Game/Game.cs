using UnityEngine;
using System.Collections;

namespace GameFramework
{
    public abstract class Game : MonoBehaviour
    {
        [SerializeField] protected TickUpdater tickUpdater = null;

        public static Game Current { get; protected set; } = null;

        public int CurrentTick => tickUpdater.CurrentTick;
        public int SyncTick => tickUpdater.SyncTick;
        public float TickInterval => tickUpdater.TickInterval;
        public float GameTime => tickUpdater.ElapsedTime;
        public bool Initialized { get; protected set; } = false;
        
        public abstract IEnumerator Initialize();
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
