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

        private int initTick = 0;

        public void Run(int tick = 0)
        {
            initTick = currentTick = tick;
            elapsedTime = 0;

            StopCoroutine("TickLoop");
            StartCoroutine("TickLoop");
        }

        private IEnumerator TickLoop()
        {
            while (true)
            {
                int count = GetCountToProcess();

                for (int i = 0; i < count; ++i)
                {
                    TickBody();
                }

                yield return null;

                elapsedTime += Time.deltaTime;
            }
        }

        private int GetCountToProcess()
        {
            int count = 0;
            int gap = GetProcessibleTick() - currentTick;
            int toleranceTick = (int)(tolerance / TickInterval);

            if (gap > toleranceTick)
            {
                count = 2;
            }
            else if (gap > 0)
            {
                count = 1;
            }

            return count;
        }

        private int GetProcessibleTick()
        {
            return isSync ? SyncTick : (int)(elapsedTime / TickInterval) + initTick;
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
