using Core.Lib.Backend.Common;
using Core.Lib.Backend.Common.Abstract;
using Core.Lib.Backend.Common.Pattern;
using Core.Lib.Backend.Executors.Interfaces;
using Core.Lib.Backend.Progressing;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Core.Lib.Backend.Executors
{
    public class ExternalFacadeExecutor : Singleton<ExternalFacadeExecutor>, IFacadeExecutor<DtoBase, ExternalExecution>
    {
        private ConcurrentDictionary<string, ExternalExecutionContext> _executionContexts;


        protected ExternalFacadeExecutor()
        {

        }

        public ExternalExecution PrepareExecution(Action facadeAction, EExecutionType executionType, bool supportCancel = false)
            => PrepareExecution<Progress>(facadeAction, executionType, supportCancel);

        public ExternalExecution PrepareExecution<TReturnType>(Func<TReturnType> facadeFunc, EExecutionType executionType, bool supportCancel = false) where TReturnType : DtoBase
            => PrepareExecution<Progress, TReturnType>(facadeFunc, executionType, supportCancel);

        public ExternalExecution PrepareExecution<TProgress>(Action facadeAction, EExecutionType executionType, bool supportCancel = false) 
            where TProgress : IProgress, new()
        {
            return PrepareExecution<TProgress, DtoBase>(
                () => { facadeAction.Invoke(); return default; },
                executionType,
                supportCancel
                );
        }

        public ExternalExecution PrepareExecution<TProgress, TReturnType>(Func<TReturnType> facadeFunc, EExecutionType executionType, bool supportCancel = false)
            where TProgress : IProgress, new()
            where TReturnType : DtoBase
        {
            ExternalExecution execution = ExternalExecution.Create(
                facadeFunc,
                executionType,
                Guid.NewGuid().ToString()
                );

            _executionContexts.TryAdd(
                execution.Uid,
                supportCancel
                    ? new ExternalExecutionContext(execution) { Progress = new TProgress(), Token = new CancellationToken() }
                    : new ExternalExecutionContext(execution) { Progress = new TProgress() }
                    );

            return execution;
        }

        public ExternalExecution Execute<TReturnType>(ExternalExecution execution) where TReturnType : DtoBase
        {
            AsyncTask task = null;
            ExternalExecutionContext context = _executionContexts[execution.Uid];

            if (context.IsCancelSupported)
            {
                task = TaskManager.Instance.CreateNewTask(
                    execution.ExecutableMethod,
                    _executionContexts[execution.Uid].Progress,
                    context.Token
                    );
            }
            else
            {
                task = TaskManager.Instance.CreateNewTask(
                    execution.ExecutableMethod,
                    _executionContexts[execution.Uid].Progress
                    );
            }

            task.Run();

            if (execution.ExecutionType == EExecutionType.Sync)
            {
                task.WaitForCompletion(Timeout.InfiniteTimeSpan);
            }

            return context.Execution;
        }


        private class ExternalExecutionContext
        {
            public ExternalExecution Execution { get; }

            public IProgress Progress { get; set; } 

            public CancellationToken? Token { get; set; }

            public bool IsCancelSupported => Token != null;

            public ExternalExecutionContext(ExternalExecution execution)
            {
                Execution = execution;
            }
        }
    }
}
