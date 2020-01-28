using Core.Lib.Backend.Common.Abstract;
using Core.Lib.Backend.Executors.Interfaces;
using Core.Lib.Backend.Progressing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Core.Lib.Backend.Executors
{
    public class ExternalExecution : IFacadeExecution
    {
        public static ExternalExecution Create<TProgress, TReturnType>(
            Func<TProgress, CancellationToken?, TReturnType> facadeMethod, 
            EExecutionType executionType,
            TProgress progress,
            CancellationToken? token,
            string uid = null
            )
            where TProgress : IProgress, new()
            where TReturnType : DtoBase

        {
            return new ExternalExecution(() => facadeMethod.Invoke(progress, token), executionType, uid);
        }

        private ExternalExecution(Func<object> facadeMethod, EExecutionType executionType, string uid = null)
        {
            ExecutableMethod = facadeMethod;
            ExecutionType = executionType;
            Uid = uid ?? Guid.NewGuid().ToString();
        }

        public Func<object> ExecutableMethod { get; }

        public EExecutionType ExecutionType { get; }

        public bool IsCompleted { get; set; }

        public bool IsSucceeded { get; set; }

        public object ReturnedValue { get; set; }

        public IFacadeExecutionError Error { get; set; }

        public string Uid { get; set; }

        public void AwaitExecution(TimeSpan timeWait)
        {
            throw new NotImplementedException();
        }
    }
;
}
