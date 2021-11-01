using System;
using System.Threading.Tasks;

namespace WpfAsync.Utils
{
    class TaskWorkaround
    {
        public static void Execute(Func<Task> job)
        {
            // Create and start Task that starts given job and awaits synchronously 
            Task.Run(() => job().GetAwaiter().GetResult())
                .Wait(); // Wait Synchronously for task
        }

        public static T Execute<T>(Func<Task<T>> job)
        {
            // Create and start Task that starts given job and awaits synchronously 
            T result = Task.Run(() => job().GetAwaiter().GetResult())
                .Result; // Wait Synchronously for task

            return result;
        }
    }
}
