using AsyncJob;
using System;
using System.Threading;

namespace Consumer
{
    public class JobThree : AsyncJobBase
    {
        public JobThree(int tickTimer) : base(tickTimer)
        {
            jobName = "Job 3";
        }

        public JobThree(int hour, int minute) : base(hour, minute)
        {

        }

        public override void DoWork()
        {
            Console.WriteLine(jobName + "\tWorking");
            Thread.Sleep(5000);
        }
    }
}
