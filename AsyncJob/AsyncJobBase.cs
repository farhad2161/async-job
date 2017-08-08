using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncJob
{
    public class AsyncJobBase
    {
        public AsyncJobBase(int tickTimer)
        {
            this.tickTimer = tickTimer;
        }

        public AsyncJobBase(int hour, int minute)
        {
            this.startTime = new TimeSpan(hour, minute, 0);
        }

        public string jobName = "Job Name";
        public int OperationTypeID = 0;
        CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        int tickTimer;
        TimeSpan startTime;

        public StatusType currentStatus = StatusType.Stop;

        public List<TimeRange> sleepTimeRange = new List<TimeRange>
        {
            new TimeRange() {from=new TimeSpan(22,0,0) },
            new TimeRange() {to=new TimeSpan(2,0,0) }
        };

        public async void Run(AsyncStatus asyncStatus)
        {
            try
            {
                if (currentStatus != StatusType.Stop) return;
                currentStatus = StatusType.Waiting;
                asyncStatus.OnJobStart(jobName);
                cancelTokenSource = new CancellationTokenSource();

                while (true)
                {
                    setTickTimerFromStartTime();
                    await Task.Delay(tickTimer * 1000, cancelTokenSource.Token);

                    if (IsInSleepTimeRange()) continue;

                    currentStatus = StatusType.Working;
                    asyncStatus.OnWorkStart(jobName);
                    string workStartTime = GetTime();

                    try
                    {
                        await Task.Run(() => { DoWork(); });
                    }
                    catch (Exception ex)
                    {
                        asyncStatus.OnJobException(jobName, ex, OperationTypeID, workStartTime, GetTime());
                    }

                    currentStatus = StatusType.Waiting;
                    asyncStatus.OnWorkFinish(jobName, OperationTypeID, workStartTime, GetTime());
                }

            }
            catch (TaskCanceledException ex)
            {

            }
            catch (Exception ex)
            {
                asyncStatus.OnJobException(jobName, ex, OperationTypeID, GetTime(), GetTime());
            }
            finally
            {
                currentStatus = StatusType.Stop;
                asyncStatus.OnJobFinish(jobName);
            }
        }

        public void Stop()
        {
            cancelTokenSource.Cancel();
        }

        public string GetTime()
        {
            return DateTime.Now.ToString();
        }

        private bool IsInSleepTimeRange()
        {
            foreach (TimeRange sleepTime in sleepTimeRange)
            {
                if (
                    !sleepTime.from.Equals(TimeSpan.Zero)
                    && !sleepTime.to.Equals(TimeSpan.Zero)
                    && DateTime.Now.TimeOfDay.CompareTo(sleepTime.from) >= 0
                    && DateTime.Now.TimeOfDay.CompareTo(sleepTime.to) <= 0
                    )
                    return true;

                else if (
                    !sleepTime.from.Equals(TimeSpan.Zero)
                    && sleepTime.to.Equals(TimeSpan.Zero)
                    && DateTime.Now.TimeOfDay.CompareTo(sleepTime.from) >= 0
                    )
                    return true;

                else if (
                    sleepTime.from.Equals(TimeSpan.Zero)
                    && !sleepTime.to.Equals(TimeSpan.Zero)
                    && DateTime.Now.TimeOfDay.CompareTo(sleepTime.to) <= 0
                    )
                    return true;
                else if (
                    !sleepTime.from.Equals(TimeSpan.Zero)
                    && !sleepTime.to.Equals(TimeSpan.Zero)
                    && sleepTime.from.CompareTo(sleepTime.to) >= 0)
                {
                    if (DateTime.Now.TimeOfDay.CompareTo(sleepTime.from) >= 0) return true;
                    if (DateTime.Now.TimeOfDay.CompareTo(sleepTime.to) <= 0) return true;
                }
            }
            return false;
        }

        public virtual void DoWork()
        {
            // Dummy stuff
            Thread.Sleep(5000);
        }

        private void setTickTimerFromStartTime()
        {
            if (startTime.Equals(TimeSpan.Zero)) return;

            if (DateTime.Now.TimeOfDay.CompareTo(startTime) > 0)
                tickTimer = (int)startTime
                    .Add(new TimeSpan(24, 0, 0))
                    .Subtract(DateTime.Now.TimeOfDay)
                    .TotalSeconds;
            else
                tickTimer = (int)startTime
                    .Subtract(DateTime.Now.TimeOfDay)
                    .TotalSeconds;
        }

        public class TimeRange
        {
            public TimeSpan from;
            public TimeSpan to;
        }
        public enum StatusType : int
        {
            Stop = 0,
            Waiting = 1,
            Working = 2
        }
    }
}
