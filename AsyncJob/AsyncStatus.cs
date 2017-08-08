using System;

namespace AsyncJob
{
    public class AsyncStatus
    {
        public AsyncStatus()
        {
            OnJobStart = (jobName) => { };
            OnJobFinish = (jobName) => { };
            OnJobException = (jobName, exception, operationTypeId, startTime, endTime) => { };

            OnWorkStart = (jobName) => { };
            OnWorkFinish = (jobName, operationTypeId, startTime, endTime) => { };
        }
        public Action<string> OnJobStart { get; set; }
        public Action<string> OnJobFinish { get; set; }
        public Action<string, Exception, int, string, string> OnJobException { get; set; }

        public Action<string> OnWorkStart { get; set; }
        public Action<string, int, string, string> OnWorkFinish { get; set; }
    }
}
