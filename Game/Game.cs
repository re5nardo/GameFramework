using UnityEngine;
using System.Collections;

namespace GameFramework
{
    public abstract class Game : MonoBehaviour
    {
        public static Game Current { get; protected set; }

        public int CurrentTick { get { return tickUpdater.CurrentTick; } }
        public int SyncTick { get { return tickUpdater.SyncTick; } }
        public float TickInterval { get { return tickUpdater.TickInterval; } }
        public float GameTime { get { return tickUpdater.CurrentTick * tickUpdater.TickInterval; } }

        protected TickUpdater tickUpdater = null;

        public abstract IEnumerator Initialize();
        protected virtual void Clear() {}

        public void Run(int tick = 0)
        {
            OnBeforeRun();

            tickUpdater.Run(tick);
        }

        protected virtual void OnBeforeRun()
        {
        }

        protected virtual void OnDestroy()
        {
            if (Current == this)
            {
                Current = null;
            }

            Clear();
        }

        public void SetSyncTick(int tick)
        {
            tickUpdater.SyncTick = tick;
        }
    }
}
