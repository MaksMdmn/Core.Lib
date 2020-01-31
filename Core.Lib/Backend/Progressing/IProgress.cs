using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Lib.Backend.Progressing
{
    public interface IProgress
    {
        event Action<ProgressMessage> OnProgressChanged;

        int CurrentRate { get; }

        EProgressResult ProgressResult { get; }

        void Progressing(int currentRate, string title = null, string description = null, DateTime ? dt = null);

        IEnumerable<ProgressMessage> GetMessages();

        void Completed(string title = null, string message = null);

        void Failed(Exception ex, string title = null, string message = null);

        void Canceled(Exception ex = null, string title = null, string message = null);

        IProgress SubProgress(int amountOfParentProgressRate);
    }
}
 