using System.Collections;
using UnityEngine;
using System;

namespace GameFramework
{
    public class TickUpdater : MonoBehaviour
    {
        private event Action<int> onTick = null;
        private event Action<int> onTickEnd = null;

        public int CurrentTick { get; private set; }
        public float TickInterval { get; private set; } = 1 / 30f;      //  sec
        public float ElapsedTime { get; private set; }                  //  sec

        private bool isSync = false;
        public int SyncTick { get; set; }
        
        private float speed = 1;
        private float timeOffset = 0;   //  시간 gap (네트워크 Latency등등)을 보상하기 위한 값 (sec)

        public void Run(int tick = 0)
        {
            CurrentTick = tick;
            ElapsedTime = tick * TickInterval + timeOffset;

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

                ElapsedTime += (Time.deltaTime * speed);
            }
        }

        private void AdjustSpeed()
        {
            if (isSync)
            {
                float syncTime = SyncTick * TickInterval + timeOffset;
                float gapTime = syncTime - ElapsedTime;    //  서버 타임 - 클라 타임 (gapTime이 양수면 서버가 더 빠른 상태, gapTime이 음수면 클라가 더 빠른 상태)

                if (gapTime < -0.12f)       
                {
                    speed = 0.01f;
                }
                else if (gapTime > 0.12f)
                {
                    speed = (syncTime - ElapsedTime) / Time.deltaTime;
                }
                else
                {
                    if (gapTime < 0)
                    {
                        speed = Mathf.SmoothStep(0, 1, (gapTime + 0.12f) / 0.12f);  //  -0.12 ~ 0 
                    }
                    else
                    {
                        speed = Mathf.SmoothStep(2, 1, 1 - gapTime / 0.12f);        //  0 ~ 0.12
                    }
                }
            }
            else
            {
                speed = 1;
            }
        }

        private int GetProcessibleTick()
        {
            int processibleTick = Mathf.RoundToInt(ElapsedTime / TickInterval);
            if (isSync)
            {
                //processibleTick = Mathf.Min(processibleTick, SyncTick);                   1. SyncTick틱을 대기하거나
                processibleTick = Mathf.RoundToInt(ElapsedTime / TickInterval);        //   2. 먼저 Tick을 수행하거나,
            }

            return processibleTick;
        }

        private void TickBody()
        {
            onTick?.Invoke(CurrentTick);

            onTickEnd?.Invoke(CurrentTick);

            CurrentTick++;
        }

        public void Initialize(float tickInterval, bool isSync, float timeOffset, Action<int> onTick, Action<int> onTickEnd)
        {
            this.TickInterval = tickInterval;
            this.isSync = isSync;
            this.timeOffset = timeOffset;
            this.onTick = onTick;
            this.onTickEnd = onTickEnd;
        }
    }
}
