using System;

namespace Thycotic.DistributedEngine.LogViewer.Models
{
    public class LogEntry
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public string ServiceRole { get; set; }
        public string Correlation { get; set; }
        public string Context { get; set; }
        public string Thread { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
    }
}