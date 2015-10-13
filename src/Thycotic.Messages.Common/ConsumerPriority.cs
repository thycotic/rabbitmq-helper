using System;

namespace Thycotic.Messages.Common
{
    /// <summary>
    /// Attribute to specify a per-Consumer priority.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ConsumerPriority : Attribute
    {
        /// <summary>
        /// Priority
        /// </summary>
        public Priority Priority { get; set; }

        /// <summary>
        /// Creates a ConsumerPriority class
        /// </summary>
        /// <param name="priority"></param>
        public ConsumerPriority(Priority priority)
        {
            Priority = priority;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum Priority
    {
        /// <summary>
        /// BelowNormal
        /// </summary>
        BelowNormal = 1,

        /// <summary>
        /// Normal
        /// </summary>
        Normal = 2,

        /// <summary>
        /// AboveNormal
        /// </summary>
        AboveNormal = 3,
        
        /// <summary>
        /// Highest
        /// </summary>
        Highest = 4
    }
}
