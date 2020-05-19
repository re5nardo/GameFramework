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
        public int CurrentTick
        {
            get { return currentTick; }
        }

        protected float tickInterval = 1 / 30f;   //  sec
        public float TickInterval
        {
            get { return tickInterval; }
        }

        private bool isSync = false;
        public int SyncTick { get; set; }

        private float tolerance;
        private float elapsedTime = 0;
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
                int toleranceTick = (int)(tolerance / TickInterval);
                int gap = SyncTick - currentTick;

                if (gap > toleranceTick)
                {
                    speed = 2f;
                }
                else if (gap > 0)
                {
                    speed = 1;
                }
                else
                {
                    speed = 0;
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
                processibleTick = Mathf.Min(processibleTick, SyncTick);
            }

            return processibleTick;
        }

        private void TickBody()
        {
            onTick?.Invoke(currentTick);

            onTickEnd?.Invoke(currentTick);

            currentTick++;
        }

        public void Initialize(float tickInterval, bool isSync, Action<int> onTick, Action<int> onTickEnd, float tolerance = 0.2f)
        {
            this.tickInterval = tickInterval;
            this.isSync = isSync;
            this.onTick = onTick;
            this.onTickEnd = onTickEnd;
            this.tolerance = tolerance;
        }
    }
}
