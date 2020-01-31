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

        public ExternalExecution PrepareExecution(Action facadeMethod, EExecutionType executionType)
        {
            return PrepareExecution<Progress, DtoBase>(
                (p, ct) => { facadeMethod.Invoke(); return null; },
                executionType
                );
        }

        public ExternalExecution PrepareExecution<TReturnType>(Func<TReturnType> facadeMethod, EExecutionType executionType)
            where TReturnType : DtoBase
        {
            return PrepareExecution<Progress, TReturnType>(
                (p, ct) => facadeMethod.Invoke(),
                executionType
                );

        }

        public ExternalExecution PrepareExecution<TProgress>(Action<TProgress> facadeMethod, EExecutionType executionType)
            where TProgress : IProgress, new()
        {
            return PrepareExecution<TProgress, DtoBase>(
                (p, ct) => { facadeMethod.Invoke(p); return null; },
                executionType
                );
        }

        public ExternalExecution PrepareExecution<TProgress, TReturnType>(Func<TProgress, TReturnType> facadeMethod, EExecutionType executionType)
            where TProgress : IProgress, new()
            where TReturnType : DtoBase
        {
            return PrepareExecution<TProgress, TReturnType>(
                (p, ct) => facadeMethod.Invoke(p),
                executionType
                );
        }

        public ExternalExecution PrepareExecution(Action<CancellationToken?> facadeMethod, EExecutionType executionType)
        {
            return PrepareExecution<Progress, DtoBase>(
                    (p, ct) => { facadeMethod.Invoke(ct); return null; },
                    executionType
                    );
        }

        public ExternalExecution PrepareExecution<TReturnType>(Func<CancellationToken?, TReturnType> facadeMethod, EExecutionType executionType)
            where TReturnType : DtoBase
        {
            return PrepareExecution<Progress, TReturnType>(
                (p, ct) => facadeMethod.Invoke(ct),
                executionType
                );

        }

            public ExternalExecution PrepareExecution<TProgress>(Action<TProgress, CancellationToken?> facadeMethod, EExecutionType executionType)
            where TProgress : IProgress, new()
        {
            return PrepareExecution<TProgress, DtoBase>(
                    (p, ct) => { facadeMethod.Invoke(p, ct); return null; },
                    executionType
                    );
        }

        public ExternalExecution PrepareExecution<TProgress, TReturnType>(Func<TProgress, CancellationToken?, TReturnType> facadeMethod, EExecutionType executionType)
            where TProgress : IProgress, new()
            where TReturnType : DtoBase
        {
            string executionUid = Guid.NewGuid().ToString();

            TProgress progress = new TProgress();

            CancellationToken? token = new CancellationToken();

            ExternalExecution execution = ExternalExecution.Create(
                facadeMethod,
                executionType,
                progress,
                token,
                executionUid
                );

            _executionContexts.TryAdd(
                executionUid,
                new ExternalExecutionContext(execution) { Progress = progress, Token = token }
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
