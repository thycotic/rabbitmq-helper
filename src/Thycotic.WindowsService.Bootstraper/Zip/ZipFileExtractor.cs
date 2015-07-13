using System;
using System.Diagnostics.Contracts;
using System.IO;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Thycotic.Logging;

namespace Thycotic.WindowsService.Bootstraper.Zip
{
    /// <summary>
    /// Zip file extractor
    /// </summary>
    public class ZipFileExtractor
    {
        private readonly ILogWriter _log = Log.Get(typeof (ZipFileExtractor));

        //samples from https://github.com/icsharpcode/SharpZipLib/wiki/Zip-Samples#anchorUnpackFull

        private void ExtractZipFile(string archiveFilenameIn, string password, string outFolder)
        {
            ZipFile zf = null;
            try
            {
                var fs = File.OpenRead(archiveFilenameIn);
                zf = new ZipFile(fs);
                if (!String.IsNullOrEmpty(password))
                {
                    zf.Password = password;     // AES encrypted entries are handled automatically
                }
                foreach (ZipEntry zipEntry in zf)
                {
                    if (!zipEntry.IsFile)
                    {
                        continue;           // Ignore directories
                    }
                    var filename = zipEntry.Name;

                    _log.Debug(string.Format("Extracting {0}", filename));

                    // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                    // Optionally match entrynames against a selection list here to skip as desired.
                    // The unpacked length is available in the zipEntry.Size property.

                    var buffer = new byte[4096];     // 4K is optimum
                    var zipStream = zf.GetInputStream(zipEntry);

                    // Manipulate the output filename here as desired.
                    var fullZipToPath = Path.Combine(outFolder, filename);
                    var directoryName = Path.GetDirectoryName(fullZipToPath);
                    if (directoryName.Length > 0)
                        Directory.CreateDirectory(directoryName);

                    // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                    // of the file, but does not waste memory.
                    // The "using" will close the stream even if an exception occurs.
                    using (var streamWriter = File.Create(fullZipToPath))
                    {
                        StreamUtils.Copy(zipStream, streamWriter, buffer);
                    }
                }
            }
            finally
            {
                if (zf != null)
                {
                    zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                    zf.Close(); // Ensure we release resources
                }
            }
        }

        /// <summary>
        /// Extracts the specified zip path.
        /// </summary>
        /// <param name="zipFilePath">The zip path.</param>
        /// <param name="directionPath">The source directory path.</param>
        public void Extract(string zipFilePath, string directionPath)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(zipFilePath));
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(directionPath));

            ExtractZipFile(zipFilePath, string.Empty, directionPath);
        }

    }
}
