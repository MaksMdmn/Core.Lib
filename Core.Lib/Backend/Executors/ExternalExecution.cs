using Core.Lib.Backend.Executors.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Lib.Backend.Executors
{
    public class ExternalExecution : IFacadeExecution
    {
        public static ExternalExecution Create(Action facadeMethod, EExecutionType executionType, string uid = null)
        {
            return Create(
                () => { facadeMethod.Invoke(); return null; },
                executionType,
                uid
                );
        }

        public static ExternalExecution Create(Func<object> facadeMethod, EExecutionType executionType, string uid = null)
        {
            return new ExternalExecution(facadeMethod, executionType, uid);
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
