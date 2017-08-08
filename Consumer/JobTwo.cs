using AsyncJob;
using System;
using System.Threading;

namespace Consumer
{
    public class JobTwo : AsyncJobBase
    {
        public JobTwo(int tickTimer) : base(tickTimer)
        {
            jobName = "Job 2";
        }

        public JobTwo(int hour, int minute) : base(hour, minute)
        {

        }

        public override void DoWork()
        {
            Console.WriteLine(jobName + "\tWorking");
            Thread.Sleep(5000);
        }
    }
}
