using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Lib.Backend.Progressing
{
    public enum EProgressResult
    {
        Aborted,
        Failed,
        InProgress,
        Success
    }

    public interface IProgressMessage : ICloneable
    {
        DateTime TimeUtc { get; set; }

        string Title { get; set; }

        string Description { get; set; }

        int CurrentRate { get; set; }

        EProgressResult Result { get; set; }
    }
}
