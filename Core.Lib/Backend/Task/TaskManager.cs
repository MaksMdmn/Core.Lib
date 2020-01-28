using System;
using System.Collections.Generic;
using System.Text;
using Core.Lib.Management;
using System.Threading;
using System.Threading.Tasks;
using Core.Lib.Backend.Progressing;

namespace Core.Lib.Backend.Common
{
    public class TaskManager : Manager<TaskManager, AsyncTask>
    {
        public AsyncTask CreateNewTask(
            Func<object> func,
            IProgress progress = null,
            CancellationToken? cancellationToken = null,
            string uid = null,
            TaskCreationOptions taskOptions = TaskCreationOptions.LongRunning
            )
        {
            AsyncTask task = null;

            if (cancellationToken == null)
            {
                task = new AsyncTask(
                    new TaskContext(func, taskOptions),
                    uid
                    );
            }
            else
            {
                task = new AsyncTask(
                    new TaskContext(func, (CancellationToken)cancellationToken, taskOptions),
                    uid
                    );
            }

            bool useDefaultProgress = progress == null;

            task.Progress = useDefaultProgress ? new Progress() : progress;

            if (useDefaultProgress)
            {
                task
                    .WhenStarted(() => task.Progress.Progressing(0))
                    .WhenFailed((t) => task.Progress.Progressing(100, t.Exception.Message))
                    .WhenCanceled((t) => task.Progress.Progressing(100, t.Exception.Message))
                    .WhenCompleted((t) => task.Progress.Progressing(100));
            }

            AddItem(task);

            return task;
        }

        public AsyncTask StartNewTask(
            Func<object> func,
            CancellationToken? token = null,
            IProgress progress = null,
            string uid = null,
            TaskCreationOptions taskOptions = TaskCreationOptions.LongRunning
            )
        {
            AsyncTask task = CreateNewTask(func, progress, token, uid, taskOptions);

            task.Run();

            return task;
        }
    }
}
