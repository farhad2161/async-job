using AsyncJob;
using System;
using System.Collections.Generic;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            List<AsyncJobBase> jobs = new List<AsyncJobBase>
            {
                new JobOne(5),// Run every 5 seconds
                new JobTwo(7),// Run every 7 seconds
                new JobThree(11)// Run every 11 seconds
            };

            foreach (AsyncJobBase job in jobs)
            {
                job.Run(new AsyncStatus()
                {
                    OnJobStart = (jobName) =>
                    {
                        Console.WriteLine(jobName + "\tOnJobStart");
                    },
                    OnJobFinish = (jobName) =>
                    {
                        Console.WriteLine(jobName + "\tOnJobFinish");
                    },
                    OnJobException = (jobName, exception, operationTypeId, startTime, endTime) =>
                    {
                        Console.WriteLine(jobName + "\tOnJobException");
                    },
                    OnWorkStart = (jobName) =>
                    {
                        Console.WriteLine(jobName + "\tOnWorkStart");
                    },
                    OnWorkFinish = (jobName, operationTypeId, startTime, endTime) =>
                    {
                        Console.WriteLine(jobName + "\tOnWorkFinish");
                    },
                });
            }


            Console.ReadLine();
            foreach (AsyncJobBase job in jobs)
            {
                job.Stop();
            }
        }
    }
}
