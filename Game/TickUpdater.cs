using System.Collections;
using UnityEngine;
using System;

namespace GameFramework
{
    public abstract class TickUpdater : MonoBehaviour
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

        public void Run()
        {
            StopCoroutine("TickLoop");
            StartCoroutine("TickLoop");
        }

        private IEnumerator TickLoop()
        {
            while (true)
            {
                var start = DateTime.Now;

                onTick?.Invoke(currentTick);

                var elapsed = (DateTime.Now - start).TotalSeconds;

                if (elapsed > tickInterval)
                {
                    Debug.LogWarning("elapsed is bigger than tickInterval!, elapsed : " + elapsed);
                }

                yield return new WaitForSeconds((float)(tickInterval - elapsed));

                onTickEnd?.Invoke(currentTick);

                currentTick++;
            }
        }

        public void Initialize(float tickInterval, Action<int> onTick, Action<int> onTickEnd)
        {
            this.tickInterval = tickInterval;

            this.onTick = onTick;
            this.onTickEnd = onTickEnd;
        }
    }
}
