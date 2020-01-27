using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Lib.Backend.Progressing;
using NLog;
using Utils;

namespace Core.Lib.Backend.Common
{
    public class AsyncTask : IAsyncTask
    {
        private volatile bool _isExpired;

        protected CancellationTokenSource   _cancellationSource;
        protected TaskContext               _taskContext;
        protected Task<object>              _rootTask;
        protected Task                      _whenStartedTask;

        protected ILogger                   _logger = LogManager.GetCurrentClassLogger();

        public string Uid { get; set; }

        public TaskStatus Status { get; }

        public ETaskState State { get; }

        public DateTime StartTime { get; }

        public DateTime FinishTime { get; }

        public Exception Exception { get; }

        public bool IsExpired => _isExpired;

        public IProgress Progress { get; set; }

        internal AsyncTask(TaskContext context, string uid = null)
        {
            _isExpired = false;

            Uid = uid ?? Guid.NewGuid().ToString();

            _taskContext = context;

            if (_taskContext.IsCancelable)
            {
                _cancellationSource = CancellationTokenSource.CreateLinkedTokenSource(_taskContext.CancellationToken);

                _rootTask = new Task<object>(
                    context.Func,
                    _taskContext.CancellationToken,
                    _taskContext.CreationOptions
                );
            }
            else
            {
                _cancellationSource = null;

                _rootTask = new Task<object>(
                     context.Func,
                     _taskContext.CreationOptions
                 );
            }

            
        }


        public void Cancel()
        {
            if (_cancellationSource == null)
            {
                throw new InvalidOperationException($"Task [ID: {Uid}] does not supporet cancellation.");
            }

            _cancellationSource?.Cancel();
        }

        public IAsyncTask WhenStarted(Action newTask)
        {
            _whenStartedTask = new Task(newTask);

            return this;
        }

        public IAsyncTask WhenCompleted(Action<Task> newTask)
        {
            _rootTask.ContinueWith(newTask, TaskContinuationOptions.OnlyOnRanToCompletion);

            return this;
        }

        public IAsyncTask WhenFailed(Action<Task> newTask)
        {
            _rootTask.ContinueWith(newTask, TaskContinuationOptions.OnlyOnFaulted);

            return this;
        }

        public IAsyncTask WhenCanceled(Action<Task> newTask)
        {
            _rootTask.ContinueWith(newTask, TaskContinuationOptions.OnlyOnCanceled);

            return this;
        }

        public IAsyncTask WhenExpiredCancel(TimeSpan awaitTime)
        {
            Task
                .Delay(awaitTime, _cancellationSource.Token)
                .ContinueWith(t =>
                    {
                        _cancellationSource.Cancel();
                        _isExpired = true;
                    }, 
                    TaskContinuationOptions.OnlyOnRanToCompletion
                );


            return this;
        }

        public void Run()
        {
            _rootTask.Start();
            _whenStartedTask?.Start();
        }

        public bool WaitForCompletion(TimeSpan awaitTime)
        {
            DateTime timeStart = DateTime.Now;

            while (!_rootTask.IsCompleted
                   && !_taskContext.CancellationToken.IsCancellationRequested
                   && !TimeSpanHelper.IsTimeoutPassed(timeStart, awaitTime))
            {
                // NOP
            }

            return _rootTask.IsCompleted;
        }

        public T GetResult<T>() where T : class => _rootTask.Result as T;
    }
}
