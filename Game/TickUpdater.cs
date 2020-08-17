using System.Collections;
using UnityEngine;
using System;

namespace GameFramework
{
    public class TickUpdater : MonoBehaviour
    {
        private event Action<int> onTick = null;
        private event Action<int> onTickEnd = null;

        private int currentTick = 0;
        public int CurrentTick => currentTick;

        protected float tickInterval = 1 / 30f;   //  sec
        public float TickInterval => tickInterval;

        private float elapsedTime = 0;
        public float ElapsedTime => elapsedTime;

        private bool isSync = false;
        public int SyncTick { get; set; }
        
        private float speed = 1f;

        public void Run(int tick = 0)
        {
            currentTick = tick;
            elapsedTime = tick * TickInterval;

            StopCoroutine("TickLoop");
            StartCoroutine("TickLoop");
        }

        private IEnumerator TickLoop()
        {
            while (true)
            {
                int count = GetProcessibleTick() - CurrentTick;
                for (int i = 0; i < count; ++i)
                {
                    TickBody();
                }

                yield return null;

                AdjustSpeed();

                elapsedTime += (Time.deltaTime * speed);
            }
        }

        private void AdjustSpeed()
        {
            if (isSync)
            {
                float gapTime = (SyncTick - CurrentTick) * TickInterval;

                if (gapTime > 0.2f)
                {
                    speed = 1.5f;
                }
                else if (gapTime > 0.1f)
                {
                    speed = 1.2f;
                }
                else if (gapTime > 0)
                {
                    speed = 1f;
                }
                else if (gapTime > -0.1f)
                {
                    speed = 0.7f;
                }
                else
                {
                    speed = 0.5f;
                }
            }
            else
            {
                speed = 1f;
            }
        }

        private int GetProcessibleTick()
        {
            int processibleTick = (int)(elapsedTime / TickInterval);
            if (isSync)
            {
                //processibleTick = Mathf.Min(processibleTick, SyncTick);       1. SyncTick틱을 대기하거나
                processibleTick = (int)(elapsedTime / TickInterval);        //  2. 먼저 Tick을 수행하거나,
            }

            return processibleTick;
        }

        private void TickBody()
        {
            onTick?.Invoke(currentTick);

            onTickEnd?.Invoke(currentTick);

            currentTick++;
        }

        public void Initialize(float tickInterval, bool isSync, Action<int> onTick, Action<int> onTickEnd)
        {
            this.tickInterval = tickInterval;
            this.isSync = isSync;
            this.onTick = onTick;
            this.onTickEnd = onTickEnd;
        }
    }
}
