using System.IO;
using System.Linq;

namespace Thycotic.RabbitMq.Helper.Logic.IO
{
    /// <summary>
    ///     Locked/log file reader
    /// </summary>
    public class LockedFileReader
    {
        private readonly string _path;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LockedFileReader" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public LockedFileReader(string path)
        {
            _path = path;
        }

        /// <summary>
        ///     Gets the tail lines.
        /// </summary>
        /// <param name="linesToReturn">The lines to return.</param>
        /// <returns>List of the tail lines</returns>
        public string[] GetTailLines(int linesToReturn)
        {
            using (var fileStream = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var streamReader = new StreamReader(fileStream))
                {
                    var contents = streamReader.ReadToEnd();

                    var lines = contents.Split('\n');

                    if (lines.Length > linesToReturn)
                        lines = lines.Skip(lines.Length - linesToReturn).ToArray();

                    return lines;
                }
            }
        }
    }
}