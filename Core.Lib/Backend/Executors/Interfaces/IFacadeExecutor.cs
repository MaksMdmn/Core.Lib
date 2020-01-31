using Core.Lib.Backend.Progressing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Lib.Backend.Executors.Interfaces
{
    public interface IFacadeExecutor<TBaseType, TExecutionType> where TBaseType : class
                                                                where TExecutionType : IFacadeExecution
    {
        TExecutionType Execute<TReturnType>(TExecutionType execution) where TReturnType : TBaseType;
    }
}
