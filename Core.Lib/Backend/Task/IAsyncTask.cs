using Core.Lib.Backend.Common.Abstract.Interfaces;
using Core.Lib.Backend.Progressing;
using System;
using System.Threading.Tasks;

namespace Core.Lib.Backend.Common
{
    public enum ETaskState
    {
        Starting,
        Running,
        Stopping,
        Stopped
    }

    public interface IAsyncTask : IUnique<string>
    {
        TaskStatus Status { get; }

        ETaskState State { get; }

        DateTime StartTime { get; }

        DateTime FinishTime { get; }

        Exception Exception { get; }

        IProgress Progress { get; set; }

        bool IsExpired { get; }

        void Run();

        void Cancel();

        IAsyncTask WhenStarted(Action newTask);

        IAsyncTask WhenCompleted(Action<Task> newTask);

        IAsyncTask WhenCanceled(Action<Task> newTask);

        IAsyncTask WhenFailed(Action<Task> newTask);

        IAsyncTask WhenExpiredCancel(TimeSpan awaitItme);

        T GetResult<T>() where T : class;

        bool WaitForCompletion(TimeSpan awaitItme);
    }
}