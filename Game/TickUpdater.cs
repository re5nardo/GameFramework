using System.Collections;
using UnityEngine;
using System;

namespace GameFramework
{
    public abstract class TickUpdater : MonoBehaviour
    {
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
            while(true)
            {
                var start = DateTime.Now;

                Tick();

                var elapsed = (DateTime.Now - start).TotalSeconds;

                if (elapsed > tickInterval)
                {
                    Debug.LogWarning("elapsed is bigger than tickInterval!, elapsed : " + elapsed);
                }

                yield return new WaitForSeconds((float)(tickInterval - elapsed));

                currentTick++;
            }
        }

        protected virtual void Initialize()
        {
        }

        protected abstract void Tick();
    }
}
