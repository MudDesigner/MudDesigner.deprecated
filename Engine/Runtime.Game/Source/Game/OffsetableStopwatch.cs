﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MudDesigner.Runtime.Game
{
    public class OffsetableStopwatch : IStopwatch
    {
        private Stopwatch internalStopwatch;
        private TimeSpan? elapsedOffset;

        public OffsetableStopwatch()
        {
            this.internalStopwatch = new Stopwatch();
        }

        public OffsetableStopwatch(TimeSpan offset) : this()
        {
            this.elapsedOffset = offset;
        }

        public void Reset() => this.internalStopwatch.Reset();

        public void Start() => this.internalStopwatch.Start();

        public void Stop() => this.internalStopwatch.Stop();

        public long GetHours()
        {
            TimeSpan elapsedTime = internalStopwatch.Elapsed;
            if (this.elapsedOffset.HasValue)
            {
                elapsedOffset += this.elapsedOffset.Value;
            }

            return (long)elapsedTime.TotalHours;
        }

        public long GetMinutes()
        {
            TimeSpan elapsedTime = internalStopwatch.Elapsed;
            if (this.elapsedOffset.HasValue)
            {
                elapsedTime += this.elapsedOffset.Value;
            }

            return (long)elapsedTime.TotalMinutes;
        }

        public long GetSeconds()
        {
            TimeSpan elapsedTime = internalStopwatch.Elapsed;
            if (this.elapsedOffset.HasValue)
            {
                elapsedTime += this.elapsedOffset.Value;
            }

            return (long)elapsedTime.TotalSeconds;
        }

        public long GetMilliseconds()
        {
            TimeSpan elapsedTime = internalStopwatch.Elapsed;
            if (this.elapsedOffset.HasValue)
            {
                elapsedTime += this.elapsedOffset.Value;
            }

            return (long)Math.Floor(elapsedTime.TotalMilliseconds);
        }
    }
}