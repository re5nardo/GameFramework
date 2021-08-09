using System.Collections;
using UnityEngine;
using System;

namespace GameFramework
{
    public class TickUpdater : MonoBehaviour
    {
        private event Action<int> onTick = null;
        private event Action<int> onTickEnd = null;
        private event Action<float> onUpdateElapsedTime = null;

        public int CurrentTick { get; private set; } = 0;
        public float TickInterval { get; private set; } = 1 / 30f;      //  sec
        public float ElapsedTime { get; protected set; }                //  sec

        private bool isSync = false;

        private int syncTick;
        public int SyncTick
        {
            get => syncTick;
            set
            {
                syncTick = value;

                AdjustSpeed();
            }
        }
        
        private float speed = 1;
        private float timeOffset = 0;   //  시간 gap (네트워크 Latency등등)을 보상하기 위한 값 (sec)

        public void Run(int tick = 0)
        {
            CurrentTick = tick;

            OnInitElapsedTime();

            StopCoroutine("TickLoop");
            StartCoroutine("TickLoop");
        }

        protected virtual void OnInitElapsedTime()
        {
            ElapsedTime = CurrentTick * TickInterval + timeOffset;
        }

        protected virtual void OnUpdateElapsedTime()
        {
            ElapsedTime += (Time.deltaTime * speed);
        }

        private IEnumerator TickLoop()
        {
            TickBody();

            while (true)
            {
                int processibleTick = GetProcessibleTick();
                while (CurrentTick < processibleTick)
                {
                    CurrentTick++;

                    TickBody();
                }

                yield return null;

                var prevTime = ElapsedTime;

                OnUpdateElapsedTime();

                onUpdateElapsedTime?.Invoke(ElapsedTime - prevTime);
            }
        }

        private void AdjustSpeed()
        {
            if (isSync)
            {
                float syncTime = SyncTick * TickInterval + timeOffset;
                float gapTime = syncTime - ElapsedTime;    //  서버 타임 - 클라 타임 (gapTime이 양수면 서버가 더 빠른 상태, gapTime이 음수면 클라가 더 빠른 상태)

                speed = 1 + 0.4f * Mathf.Pow(gapTime, 3);

                if (speed > 10)
                {
                    speed = 10;
                }
                else if (speed < 0.1f)
                {
                    speed = 0.1f;
                }
            }
            else
            {
                speed = 1;
            }
        }

        private int GetProcessibleTick()
        {
            int processibleTick = (int)(ElapsedTime / TickInterval);
            if (isSync)
            {
                //processibleTick = Mathf.Min(processibleTick, SyncTick);                   1. SyncTick틱을 대기하거나
                processibleTick = (int)(ElapsedTime / TickInterval);        //   2. 먼저 Tick을 수행하거나,
            }

            return processibleTick;
        }

        private void TickBody()
        {
            onTick?.Invoke(CurrentTick);

            onTickEnd?.Invoke(CurrentTick);
        }

        public void Initialize(float tickInterval, bool isSync, float timeOffset, Action<int> onTick, Action<int> onTickEnd, Action<float> onUpdateElapsedTime)
        {
            this.TickInterval = tickInterval;
            this.isSync = isSync;
            this.timeOffset = timeOffset;
            this.onTick = onTick;
            this.onTickEnd = onTickEnd;
            this.onUpdateElapsedTime = onUpdateElapsedTime;
        }
    }
}
