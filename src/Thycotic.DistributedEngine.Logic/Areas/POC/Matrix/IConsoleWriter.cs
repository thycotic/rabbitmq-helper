namespace Thycotic.DistributedEngine.Logic.Areas.POC.Matrix
{
    /// <summary>
    /// Interface for a console writer
    /// </summary>
    public interface IConsoleWriter
    {
        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        void WriteMatrix(char value);
    }
}