using Core.Lib.Backend.Progressing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Lib.Backend.Executors.Interfaces
{
    public interface IFacadeExecutor<TBaseType, TExecutionType> where TBaseType : class
                                                                where TExecutionType : IFacadeExecution
    {
        TExecutionType PrepareExecution(Action facadeAction, EExecutionType executionType, bool supportCancel = false);

        TExecutionType PrepareExecution<TReturnType>(Func<TReturnType> facadeFunc, EExecutionType executionType, bool supportCancel = false)
            where TReturnType : TBaseType;

        TExecutionType PrepareExecution<TProgress>(Action facadeAction, EExecutionType executionType, bool supportCancel = false) 
            where TProgress : IProgress, new();

        TExecutionType PrepareExecution<TProgress, TReturnType>(Func<TReturnType> facadeFunc, EExecutionType executionType, bool supportCancel = false) 
            where TReturnType : TBaseType 
            where TProgress : IProgress, new();

        TExecutionType Execute<TReturnType>(TExecutionType execution) where TReturnType : TBaseType;
    }
}
