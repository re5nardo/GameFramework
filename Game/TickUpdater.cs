using System.Collections;
using UnityEngine;
using System;

namespace GameFramework
{
    public class TickUpdater : MonoBehaviour
    {
        private event Action<int> onTick = null;
        private event Action<int> onTickEnd = null;
        private event Action onFrameUpdate = null;

        public int CurrentTick { get; private set; } = 0;
        public double TickInterval { get; private set; } = 1 / 30f;         //  sec
        public double ElapsedTime { get; protected set; }                   //  sec
        public double deltaTime { get; private set; }                       //  sec
        public int deltaTick { get; private set; }

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
        private double timeOffset = 0;   //  시간 gap (네트워크 Latency등등)을 보상하기 위한 값 (sec)

        public void Run(int tick = 0)
        {
            CurrentTick = tick;

            OnInitElapsedTime();

            StopCoroutine("TickLoop");
            StartCoroutine("TickLoop");
        }

        protected virtual void OnInitElapsedTime()
        {
            ElapsedTime = CurrentTick * TickInterval;
        }

        protected virtual void OnUpdateElapsedTime()
        {
            ElapsedTime += (Time.deltaTime * speed);
        }

        private IEnumerator TickLoop()
        {
            while (true)
            {
                int processibleTick = GetProcessibleTick();
                var tickCount = 0;

                while (CurrentTick <= processibleTick)
                {
                    TickBody();
                    tickCount++;
                }

                yield return null;

                var prevTime = ElapsedTime;

                OnUpdateElapsedTime();

                deltaTime = ElapsedTime - prevTime;
                deltaTick = tickCount;

                onFrameUpdate?.Invoke();
            }
        }

        private void AdjustSpeed()
        {
            if (isSync)
            {
                double syncTime = SyncTick * TickInterval + timeOffset;
                double gapTime = syncTime - ElapsedTime;    //  서버 타임 - 클라 타임 (gapTime이 양수면 서버가 더 빠른 상태, gapTime이 음수면 클라가 더 빠른 상태)

                speed = 1 + 0.4f * Mathf.Pow((float)gapTime, 3);

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

            CurrentTick++;
        }

        public void Initialize(double tickInterval, bool isSync, double timeOffset, Action<int> onTick, Action<int> onTickEnd, Action onFrameUpdate)
        {
            this.TickInterval = tickInterval;
            this.isSync = isSync;
            this.timeOffset = timeOffset;
            this.onTick = onTick;
            this.onTickEnd = onTickEnd;
            this.onFrameUpdate = onFrameUpdate;
        }
    }
}
