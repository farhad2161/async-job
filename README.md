# Async Job
A C# asynchronous job management.

You need to call some of your methods at a specific time or period? Async job will do it for you.

## How to use
Create a new class and extend ```AsyncJobBase```. Call the ```Run``` method.

1.Extend AsyncJobBase

```
public class JobOne : AsyncJobBase
{
    public JobOne(int tickTimer) : base(tickTimer)
    {
    	// Your job name
        jobName = "Job 1";
    }

    public JobOne(int hour, int minute) : base(hour, minute)
    {

    }

    public override void DoWork()
    {
        // Do your work here.
    }
}
```

2.Make a new instance and call ```Run``` method.

```
JobOne job = new JobOne(5);// Run this job every 5 seconds

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
```

For more information you can see the ```Program.cs``` in Consumer project.

