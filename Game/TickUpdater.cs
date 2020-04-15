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

        public void Run(int tick = 0)
        {
            currentTick = tick;

            StopCoroutine("TickLoop");
            StartCoroutine("TickLoop");
        }

        private IEnumerator TickLoop()
        {
            while (true)
            {
                var start = DateTime.Now;

                onTick?.Invoke(currentTick);

                onTickEnd?.Invoke(currentTick);

                float elapsed = (float)(DateTime.Now - start).TotalSeconds;

                if (elapsed > tickInterval)
                {
                    Debug.LogError("elapsed is bigger than tickInterval!, elapsed : " + elapsed);
                    yield break;
                }

                if (isSync)
                {
                    if (SyncTick <= currentTick)    //  +- buffer??
                    {
                        yield return new WaitUntil(() => SyncTick > currentTick);
                    }
                    else if (SyncTick - currentTick > (int)(tolerance / tickInterval))
                    {
                        //  No wait
                    }
                    else
                    {
                        while (true)
                        {
                            float nextTime = elapsed + Time.deltaTime;

                            if (nextTime > tickInterval)
                            {
                                float nextGap = nextTime - tickInterval;
                                float currentGap = tickInterval - elapsed;

                                if (nextGap > currentGap)
                                {
                                    break;
                                }

                                yield return null;

                                elapsed += Time.deltaTime;
                            }
                            else
                            {
                                yield return null;

                                elapsed += Time.deltaTime;
                            }
                        }
                    }
                }
                else
                {
                    while (true)
                    {
                        float nextTime = elapsed + Time.deltaTime;

                        if (nextTime > tickInterval)
                        {
                            float nextGap = nextTime - tickInterval;
                            float currentGap = tickInterval - elapsed;

                            if (nextGap > currentGap)
                            {
                                //  No wait
                                break;
                            }

                            yield return null;

                            elapsed += Time.deltaTime;
                        }
                        else
                        {
                            yield return null;

                            elapsed += Time.deltaTime;
                        }
                    }
                }

                currentTick++;
            }
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
