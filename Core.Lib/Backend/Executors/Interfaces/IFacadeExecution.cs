using Core.Lib.Backend.Common.Abstract.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Lib.Backend.Executors.Interfaces
{
    public enum EExecutionType
    {
        Sync,
        Async
    }

    public interface IFacadeExecution : IUnique<string>
    {
        Func<object> ExecutableMethod { get; }

        EExecutionType ExecutionType { get; }

        bool IsCompleted { get; set; }

        bool IsSucceeded { get; set; }

        object ReturnedValue { get; set; }

        IFacadeExecutionError Error { get; set; }

        void AwaitExecution(TimeSpan timeWait);
    }
}
