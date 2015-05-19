using System.IO;

namespace Thycotic.Utility.IO
{
    /// <summary>
    /// Directory copier
    /// </summary>
    public class DirectoryCopier
    {
        /// <summary>
        /// Copies the specified source path.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="destinationPath">The destination path.</param>
        /// <param name="recursive">if set to <c>true</c> [recursive].</param>
        /// <param name="overwrite">if set to <c>true</c> [overwrite].</param>
        /// <exception cref="System.IO.DirectoryNotFoundException">Source directory does not exist or could not be found:
        /// + sourcePath</exception>
        public void Copy(string sourcePath, string destinationPath, bool recursive, bool overwrite = false)
        {

            // get the subdirectories for the specified directory.
            var dir = new DirectoryInfo(sourcePath);
            var dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourcePath);
            }

            // if the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            // get the files in the directory and copy them to the new location.
            var files = dir.GetFiles();
            foreach (var file in files)
            {
                var temppath = Path.Combine(destinationPath, file.Name);
                file.CopyTo(temppath, overwrite);
            }

            // if copying subdirectories, copy them and their contents to new location. 
            if (!recursive) return;

            foreach (var subdir in dirs)
            {
                //skips the destination path
                if (subdir.FullName == destinationPath)
                {
                    continue;
                }

                var temppath = Path.Combine(destinationPath, subdir.Name);

                Copy(subdir.FullName, temppath, true, overwrite);
            }
        }

    }
}
