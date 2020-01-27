using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Lib.Backend.Progressing
{
    public class Progress : IProgress
    {
        public event Action<ProgressMessage> OnProgressChanged;

        private List<ProgressMessage>   _progressMessages;
        private volatile bool           _isProgressStopped;
        private int                     _currentRate;

        public int CurrentRate
        {
            get { return _currentRate <= 100 ? _currentRate : 100; }

            private set { _currentRate = value; }
        }

        public EProgressResult ProgressResult { get; private set; }

        public Progress()
        {
            _progressMessages = new List<ProgressMessage>();
            _isProgressStopped = false;
        }

        public IEnumerable<ProgressMessage> GetMessages() =>  _progressMessages.Select(msg => (ProgressMessage)msg.Clone());

        public void Progressing(int currentRate, string title = null, string message = null, DateTime? dt = null)
        {
            ProgressMessage msg = ProgressMessage.CreateMessage(
                currentRate, 
                title, 
                message, 
                dt);

            Progressing(msg);
        }

        protected void Progressing(ProgressMessage message)
        {
            if (_isProgressStopped)
            {
                return;
            }

            _progressMessages.Add(message);

            CurrentRate = message.CurrentRate;

            ProgressResult = message.Result;

            _isProgressStopped = ProgressResult == EProgressResult.Aborted || ProgressResult == EProgressResult.Failed;

            OnProgressChanged?.Invoke(message);
        }

        
        public void Canceled(Exception ex = null, string title = null, string message = null)
        {
            ProgressMessage msg = ProgressMessage.CreateMessage(
                CurrentRate, 
                title ?? ex?.GetType().ToString() ?? string.Empty, 
                message ?? ex?.Message ?? string.Empty, 
                result: EProgressResult.Aborted);

            Progressing(msg);
        }

        public void Completed(string title = null, string message = null)
        {
            ProgressMessage msg = ProgressMessage.CreateMessage(
                CurrentRate >= 100 ? CurrentRate : 100,
                title ?? EProgressResult.Success.ToString(),
                message ?? string.Empty,
                result: EProgressResult.Success);

            Progressing(msg);
        }

        public void Failed(Exception ex, string title = null, string message = null)
        {
            ProgressMessage msg = ProgressMessage.CreateMessage(
                CurrentRate,
                title ?? ex?.GetType().ToString(),
                message ?? ex?.Message,
                result: EProgressResult.Failed);

            Progressing(msg);
        }

        public IProgress SubProgress(int amountOfParentProgressRate)
        {
            Progress childProgress = new Progress();

            childProgress.OnProgressChanged += (message) =>
            {
                int weightedChildRateDiff = amountOfParentProgressRate * (message.CurrentRate / 100);

                message.CurrentRate = CurrentRate + weightedChildRateDiff;

                Progressing(message);
            };

            return childProgress;
        }
    }
}
