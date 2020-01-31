using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Lib.Backend.Executors.Interfaces
{
    public interface IFacadeExecutionError
    {
        Exception Exception { get; set; }

        string TaskUid { get; set; }

        void Throw();
    }
}
