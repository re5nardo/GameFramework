using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameFramework
{
    public sealed class WaitForDone : CustomYieldInstruction
    {
        public override bool keepWaiting => !WaitForDoneProcess();

        private Func<bool> predicate;
        private float timeout;
        private float startTime;
        private float elapsedTime => Time.time - startTime;

        public WaitForDone(Func<bool> predicate, float timeout)
        {
            this.predicate = predicate;
            this.timeout = timeout;
            this.startTime = Time.time;
        }

        private bool WaitForDoneProcess()
        {
            return elapsedTime >= timeout || predicate();
        }
    }
}
