using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Lib.Backend.Progressing
{
    public class ProgressMessage : IProgressMessage
    {
        public DateTime TimeUtc { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int CurrentRate { get; set; }

        public EProgressResult Result { get; set; }

        private ProgressMessage()
        {
            
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public static ProgressMessage CreateMessage(
            int currentRate, 
            string title = null, 
            string description = null, 
            DateTime? dt = null,
            EProgressResult result = EProgressResult.InProgress)
        {
            return new ProgressMessage
            {
                CurrentRate = currentRate,
                Title = title,
                TimeUtc = dt ?? DateTime.UtcNow,
                Description = description ?? $"Progress result: {result}",
                Result = result
            };
        }
    }
}
