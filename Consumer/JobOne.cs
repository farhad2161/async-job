using AsyncJob;
using System;
using System.Threading;

namespace Consumer
{
    public class JobOne : AsyncJobBase
    {
        public JobOne(int tickTimer) : base(tickTimer)
        {
            jobName = "Job 1";
        }

        public JobOne(int hour, int minute) : base(hour, minute)
        {

        }

        public override void DoWork()
        {
            Console.WriteLine(jobName + "\tWorking");
            Thread.Sleep(5000);
        }
    }
}
