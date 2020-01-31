using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Lib.Backend.Common
{
    public class TaskContext
    {
        public CancellationToken CancellationToken { get; }

        public TaskCreationOptions CreationOptions { get; }

        public Func<object> Func { get; }

        public bool IsCancelable { get; }

        internal TaskContext(
            Func<object> func,
            CancellationToken cancellationToken,
            TaskCreationOptions creationOptions = TaskCreationOptions.LongRunning
            )
        {
            IsCancelable = true;
            Func = func;
            CancellationToken = cancellationToken;
            CreationOptions = creationOptions;
        }

        internal TaskContext(
            Func<object> func,
            TaskCreationOptions creationOptions = TaskCreationOptions.LongRunning
            )
        {
            IsCancelable = false;
            Func = func;
            CreationOptions = creationOptions;
        }
    }
}
