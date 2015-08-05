using System.IO;
using System.Linq;

namespace Thycotic.RabbitMq.Helper.Commands.Management
{
    /// <summary>
    /// Locked/log file reader
    /// </summary>
    public class LockedFileReader
    {
        private readonly string _path;

        public LockedFileReader(string path)
        {
            _path = path;
        }

        public string[] GetTailLines(int linesToReturn)
        {
            using (var fileStream = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var streamReader = new StreamReader(fileStream))
                {
                    var contents = streamReader.ReadToEnd();

                    var lines = contents.Split('\n');

                    if (lines.Length > linesToReturn)
                    {
                        lines = lines.Skip(lines.Length - linesToReturn).ToArray();
                    }

                    return lines;
                }
            }
        }
    }
}