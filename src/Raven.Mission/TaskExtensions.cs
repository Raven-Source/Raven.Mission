using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Raven.Mission
{
    public static class TaskExtensions
    {
        public static Task<T> WithTimeout<T>(this Task<T> task, TimeSpan timeout)
        {
            if (task.IsCompleted || (timeout == Timeout.InfiniteTimeSpan))
            {
                return task;
            }

            return WithTimeoutInternal(task, timeout);
        }

        public static Task WithTimeout(this Task task, TimeSpan timeout)
        {
            if (task.IsCompleted || (timeout == Timeout.InfiniteTimeSpan))
            {
                return task;
            }

            return WithTimeoutInternal(task, timeout);
        }

        static async Task<T> WithTimeoutInternal<T>(Task<T> task, TimeSpan timeout)
        {
            using (var cts = new CancellationTokenSource())
            {
                if (task == await Task.WhenAny(task, Task.Delay(timeout, cts.Token)))
                {
                    cts.Cancel();
                    return await task;
                }
            }

            throw new TimeoutException();
        }

        static async Task WithTimeoutInternal(Task task, TimeSpan timeout)
        {
            using (var cts = new CancellationTokenSource())
            {
                if (task == await Task.WhenAny(task, Task.Delay(timeout, cts.Token)))
                {
                    cts.Cancel();
                    await task;
                    return;
                }
            }

            throw new TimeoutException();
        }
    }
}
