﻿using System;

namespace Hi3Helper.Http
{
    public enum MultisessionState
    {
        Idle, WaitingOnSession,
        Downloading, Merging,
        Finished, FinishedNeedMerge,
        FailedMerging, FailedDownloading,
        CancelledMerging, CancelledDownloading
    }

    public partial class Http
    {
        // Download Progress Event Handler
        public event EventHandler<DownloadEvent> DownloadProgress;

        // Updadte Progress of the Download internally
        private void UpdateProgress(DownloadEvent Event) => DownloadProgress?.Invoke(this, Event);
    }

    public class DownloadEvent
    {
        public DownloadEvent(long SizeLastDownloaded, long SizeDownloaded, long SizeToBeDownloaded,
            long Read, double TotalSecond, MultisessionState state)
        {
            this.Speed = (long)(SizeLastDownloaded / TotalSecond);
            this.SizeDownloaded = SizeDownloaded;
            this.SizeToBeDownloaded = SizeToBeDownloaded;
            this.Read = Read;
            this.State = state;
        }

        public long SizeDownloaded { get; private set; }
        public long SizeToBeDownloaded { get; private set; }
        public double ProgressPercentage => Math.Round((SizeDownloaded / (double)SizeToBeDownloaded) * 100, 2);
        public long Read { get; private set; }
        public long Speed { get; private set; }
        public TimeSpan TimeLeft => checked(TimeSpan.FromSeconds((SizeToBeDownloaded - SizeDownloaded) / UnZeroed(Speed)));
        private long UnZeroed(long Input) => Math.Max(Input, 1);
        public MultisessionState State { get; set; }
    }
}
