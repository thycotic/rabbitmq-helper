using System;

namespace Thycotic.SecretServerEngine2.LogViewer
{
    public class LogEntry
    {
        public DateTime Date { get; set; }
        public string Level { get; set; }
        public Guid Correlation { get; set; }
        public string Context { get; set; }
        public string Message { get; set; }
    }
}