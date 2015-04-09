using System;

namespace Thycotic.Logging
{
    /// <summary>
    /// Interface for a log writer
    /// </summary>
    public interface ILogWriterFactory
    {
        /// <summary>
        /// Gets the log writer for the specified type
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        ILogWriter GetLogWriter(Type type);
    }

   
}