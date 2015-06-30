using System;
using System.IO;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Thycotic.InstallerGenerator.Core.Steps;
using Thycotic.Logging;

namespace Thycotic.InstallerGenerator.Core.Zip
{
    /// <summary>
    /// 
    /// </summary>
    public class ZipFileWriter
    {
        private readonly ILogWriter _log = Log.Get(typeof(ZipFileWriter));

        /// <summary>
        /// The no compression level
        /// </summary>
        public const int NoCompressionLevel = 0;

        /// <summary>
        /// The maximum compression level
        /// </summary>
        public const int MaxCompressionLevel = 9;

        //samples from https://github.com/icsharpcode/SharpZipLib/wiki/Zip-Samples#anchorUnpackFull

        private void CompressFolder(string path, ZipOutputStream zipStream, int folderOffset)
        {

            var files = Directory.GetFiles(path);

            foreach (var filename in files)
            {
                _log.Debug(string.Format("Adding {0}", filename));

                var fi = new FileInfo(filename);

                var entryName = filename.Substring(folderOffset); // Makes the name in zip based on the folder
                entryName = ZipEntry.CleanName(entryName); // Removes drive from name and fixes slash direction
                var newEntry = new ZipEntry(entryName)
                {
                    DateTime = fi.LastWriteTime, 
                    Size = fi.Length
                };
                // Note the zip format stores 2 second granularity

                // Specifying the AESKeySize triggers AES encryption. Allowable values are 0 (off), 128 or 256.
                // A password on the ZipOutputStream is required if using AES.
                //   newEntry.AESKeySize = 256;

                // To permit the zip to be unpacked by built-in extractor in WinXP and Server2003, WinZip 8, Java, and other older code,
                // you need to do one of the following: Specify UseZip64.Off, or set the Size.
                // If the file may be bigger than 4GB, or you do not need WinXP built-in compatibility, you do not need either,
                // but the zip will be in Zip64 format which not all utilities can understand.
                //   zipStream.UseZip64 = UseZip64.Off;

                zipStream.PutNextEntry(newEntry);

                // Compress the file in buffered chunks
                // the "using" will close the stream even if an exception occurs
                var buffer = new byte[4096];
                using (var streamReader = File.OpenRead(filename))
                {
                    StreamUtils.Copy(streamReader, zipStream, buffer);
                }
                zipStream.CloseEntry();
            }
            var folders = Directory.GetDirectories(path);
            foreach (var folder in folders)
            {
                CompressFolder(folder, zipStream, folderOffset);
            }
        }

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
        /// Zips the specified source directory path.
        /// </summary>
        /// <param name="sourcePath">The source directory path.</param>
        /// <param name="zipFilePath">The zip file path.</param>
        /// <param name="compressionLevel">The compression level.</param>
        public void Compress(string sourcePath, string zipFilePath, int compressionLevel = 3)
        {

            using (var fsOut = File.Create(zipFilePath))
            using (var zipStream = new ZipOutputStream(fsOut))
            {

                _log.Debug(string.Format("Compressing at level {0}", compressionLevel));

                zipStream.SetLevel(compressionLevel); //0-9, 9 being the highest level of compression

                //zipStream.Password = password; // optional. Null is the same as not setting. Required if using AES.

                // This setting will strip the leading part of the folder path in the entries, to
                // make the entries relative to the starting folder.
                // To include the full path for each entry up to the drive root, assign folderOffset = 0.
                var folderOffset = sourcePath.Length + (sourcePath.EndsWith("\\") ? 0 : 1);

                CompressFolder(sourcePath, zipStream, folderOffset);

                zipStream.IsStreamOwner = true; // Makes the Close also Close the underlying stream
                zipStream.Close();
            }
        }

        
        /// <summary>
        /// Extracts the specified zip path.
        /// </summary>
        /// <param name="zipFilePath">The zip path.</param>
        /// <param name="directionPath">The source directory path.</param>
        public void Extract(string zipFilePath, string directionPath)
        {
            ExtractZipFile(zipFilePath, string.Empty, directionPath);
        }

       
    }
}
