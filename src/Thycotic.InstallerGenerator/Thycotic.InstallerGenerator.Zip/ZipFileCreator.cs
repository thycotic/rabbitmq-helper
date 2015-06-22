using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;

namespace Thycotic.InstallerGenerator.Zip
{
    public class AgentContentZipCreator
    {
        private IApplicationPathProvider _applicationPathProvider;

        public AgentContentZipCreator(IApplicationPathProvider pathProvider)
        {
            _applicationPathProvider = pathProvider;
        }

        public AgentContentZipCreator()
            : this(ServiceLocator.ApplicationPathProvider)
        {
        }

        public void AddAllDllsToContentZip()
        {
            string applicationPath = _applicationPathProvider.GetApplicationPath();
            DeleteOldAgentFiles(applicationPath);

            bool shouldAddDlls;
            string mainContent = Path.Combine(applicationPath, @"agentinstallerfiles\Content.zip");
            string upgradeMainContent = Path.Combine(applicationPath, @"agentinstallerfiles\ContentUpgrade.zip");
            using (FileStream stream = File.Open(mainContent, FileMode.Open))
            {
                ZipFile zipFile = new ZipFile(stream);
                ZipEntry entry = zipFile.GetEntry("Thycotic.Data.dll");
                shouldAddDlls = entry == null;
            }
            if (shouldAddDlls)
            {
                File.Copy(mainContent, upgradeMainContent, true);
                //TKBK - Create Content.zip for fresh installs
                CreateContentZipStream(mainContent, applicationPath);
                //TKBK - Create ContentUpgrade.zip for agent upgrades (exactly the same, except includes ICSharpCode.SharpZipLib.dll)
                CreateUpgradeContentZipStream(mainContent, upgradeMainContent, applicationPath);
            }
        }

        private void CreateContentZipStream(string contentZip, string applicationPath)
        {
            List<string> fileNames;
            List<byte[]> filesInZip;
            List<long> fileSizes;
            GetFilesInZip(contentZip, out filesInZip, out fileNames, out fileSizes);
            using (ZipOutputStream s = new ZipOutputStream(File.Open(contentZip, FileMode.Open)))
            {
                for (int i = 0; i < filesInZip.Count; i++)
                {
                    AddFileToZip(s, filesInZip[i], fileNames[i], fileSizes[i]);
                }
                AddDllToZip(s, Path.Combine(applicationPath, @"bin\Thycotic.Data.dll"));
                AddDllToZip(s, Path.Combine(applicationPath, @"bin\Castle.Core.dll"));
                AddDllToZip(s, Path.Combine(applicationPath, @"bin\Castle.DynamicProxy2.dll"));
                AddDllToZip(s, Path.Combine(applicationPath, @"bin\Castle.MicroKernel.dll"));
                AddDllToZip(s, Path.Combine(applicationPath, @"bin\Castle.Windsor.dll"));
                AddDllToZip(s, Path.Combine(applicationPath, @"bin\ConnectionProvider.Interface.dll"));
                AddDllToZip(s, Path.Combine(applicationPath, @"bin\eWorld.UI.dll"));
                AddDllToZip(s, Path.Combine(applicationPath, @"bin\Jscape.Ssh.dll"));
                AddDllToZip(s, Path.Combine(applicationPath, @"bin\BouncyCastle.Crypto.dll"));
                if (File.Exists(Path.Combine(applicationPath, @"bin\Sybase.AdoNet2.AseClient.dll")))
                {
                    AddDllToZip(s, Path.Combine(applicationPath, @"bin\Sybase.AdoNet2.AseClient.dll"));
                }
                AddDllToZip(s, Path.Combine(applicationPath, @"bin\System.Web.DataVisualization.dll"));
                AddDllToZip(s, Path.Combine(applicationPath, @"bin\Thycotic.AppCore.dll"));
                AddDllToZip(s, Path.Combine(applicationPath, @"bin\Thycotic.Foundation.Charting.dll"));
                AddDllToZip(s, Path.Combine(applicationPath, @"bin\Thycotic.Foundation.dll"));
                AddDllToZip(s, Path.Combine(applicationPath, @"bin\Thycotic.Foundation.WebControls.dll"));
                AddDllToZip(s, Path.Combine(applicationPath, @"bin\Thycotic.ihawu.Base.dll"));
                AddDllToZip(s, Path.Combine(applicationPath, @"bin\Thycotic.ihawu.Business.dll"));
                AddDllToZip(s, Path.Combine(applicationPath, @"bin\Thycotic.Licensing.Common.dll"));
                AddDllToZip(s, Path.Combine(applicationPath, @"bin\Thycotic.Licensing.Public.dll"));
                AddDllToZip(s, Path.Combine(applicationPath, @"bin\Win32Security.dll"));
                AddDllToZip(s, Path.Combine(applicationPath, @"bin\TaskScheduler.dll"));
                AddDllToZip(s, Path.Combine(applicationPath, @"bin\Interop.TaskScheduler.dll"));
                AddDllToZip(s, Path.Combine(applicationPath, @"bin\ComPlusCLRx32.xdll"));
                AddDllToZip(s, Path.Combine(applicationPath, @"bin\ComPlusCLRx64.xdll"));
                AddDllToZip(s, Path.Combine(applicationPath, @"bin\Thycotic.ihawu.JavaScriptWrappers.dll"));
                AddDllToZip(s, Path.Combine(applicationPath, @"bin\HtmlAgilityPack.dll"));
                AddDllToZip(s, Path.Combine(applicationPath, @"bin\PhantomJS.exe"));
                AddDllToZip(s, Path.Combine(applicationPath, @"bin\SecureBlackbox.dll"));
                AddDllToZip(s, Path.Combine(applicationPath, @"bin\SecureBlackbox.SSHClient.dll"));
                AddDllToZip(s, Path.Combine(applicationPath, @"bin\SecureBlackbox.SSHCommon.dll"));
                AddDllToZip(s, Path.Combine(applicationPath, @"bin\SecureBlackbox.SSHCommon.dll"));
                AddDllToZip(s, Path.Combine(applicationPath, @"bin\Thycotic.PasswordChangers.dll"));
                if (File.Exists(Path.Combine(applicationPath, @"bin\sapnco.dll")))
                {
                    AddDllToZip(s, Path.Combine(applicationPath, @"bin\sapnco.dll"));
                }
                if (File.Exists(Path.Combine(applicationPath, @"bin\sapnco_utils.dll")))
                {
                    AddDllToZip(s, Path.Combine(applicationPath, @"bin\sapnco_utils.dll"));
                }

                //Add Phantomjs files
                string phantomPath = Path.Combine(applicationPath, @"bin\PhantomJSScripts");
                string binPath = Path.Combine(applicationPath, @"bin");
                AddDirectoryFiles(binPath, phantomPath, s);

                s.Finish();
                s.Close();
            }
        }

        private void CreateUpgradeContentZipStream(string contentZip, string upgradeContentZip, string applicationPath)
        {
            List<string> fileNames;
            List<byte[]> filesInZip;
            List<long> fileSizes;
            GetFilesInZip(contentZip, out filesInZip, out fileNames, out fileSizes);
            using (ZipOutputStream s = new ZipOutputStream(File.Open(upgradeContentZip, FileMode.Open)))
            {
                for (int i = 0; i < filesInZip.Count; i++)
                {
                    AddFileToZip(s, filesInZip[i], fileNames[i], fileSizes[i]);
                }

                AddDllToZip(s, Path.Combine(applicationPath, @"bin\ICSharpCode.SharpZipLib.dll"));

                s.Finish();
                s.Close();
            }
        }

        private void DeleteOldAgentFiles(string applicationPath)
        {
            string agentInstallerPath = Path.Combine(applicationPath, "agentinstallerfiles");
            List<string> oldAgentFiles = new List<string>() { "AgentInstallerAction.dll", "banner.jpg", "ICSharpCode.SharpZipLib.dll", "SecretServerAgentInstaller.msi", "icon.ico" };

            foreach (var oldAgentFile in oldAgentFiles)
            {
                string filePath = Path.Combine(agentInstallerPath, oldAgentFile);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        private void AddDirectoryFiles(string rootFolder, string currentFolder, ZipOutputStream zStream)
        {

            string relativePath = currentFolder.Substring(rootFolder.Length) + "\\";
            if (relativePath.StartsWith("\\"))
            {
                relativePath = relativePath.Substring(1);
            }

            foreach (string file in Directory.GetFiles(currentFolder))
            {
                AddFileToZipWithPath(zStream, relativePath, file);
            }

            string[] subFolders = Directory.GetDirectories(currentFolder);
            foreach (string folder in subFolders)
            {
                AddDirectoryFiles(rootFolder, folder, zStream);
            }
        }

        private static void AddFileToZipWithPath(ZipOutputStream zStream, string relativePath, string file)
        {
            byte[] buffer = new byte[4096];
            string fileRelativePath = (relativePath.Length > 1 ? relativePath : string.Empty) + Path.GetFileName(file);
            ZipEntry entry = new ZipEntry(fileRelativePath);
            entry.DateTime = DateTime.Now;
            entry.Size = new FileInfo(file).Length;
            zStream.PutNextEntry(entry);
            using (FileStream fs = File.OpenRead(file))
            {
                int sourceBytes;
                do
                {
                    sourceBytes = fs.Read(buffer, 0, buffer.Length);
                    zStream.Write(buffer, 0, sourceBytes);

                } while (sourceBytes > 0);
            }
        }

        private void GetFilesInZip(string path, out List<byte[]> filesInZip, out List<string> fileNames, out List<long> fileSizes)
        {
            Stream stream = File.Open(path, FileMode.Open);
            filesInZip = new List<byte[]>();
            fileNames = new List<string>();
            fileSizes = new List<long>();
            byte[] data = new byte[4096];
            using (ZipInputStream s = new ZipInputStream(stream))
            {
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    List<byte> buffer = new List<byte>();
                    if (theEntry.IsFile)
                    {
                        int size = s.Read(data, 0, data.Length);
                        while (size > 0)
                        {
                            for (int i = 0; i < size; i++)
                            {
                                buffer.Add(data[i]);
                            }
                            size = s.Read(data, 0, data.Length);
                        }
                        filesInZip.Add(buffer.ToArray());
                        fileNames.Add(theEntry.Name);
                        fileSizes.Add(theEntry.Size);
                    }
                }
                s.Close();
            }
        }

        private void AddFileToZip(ZipOutputStream s, byte[] bytes, string fileName, long fileSize)
        {
            ZipEntry entry = new ZipEntry(fileName);
            entry.DateTime = DateTime.Now;
            entry.Size = fileSize;
            s.PutNextEntry(entry);
            s.Write(bytes, 0, bytes.Length);
        }

        private void AddDllToZip(ZipOutputStream s, string path)
        {
            ZipEntry dllEntry = new ZipEntry(Path.GetFileName(path));
            dllEntry.DateTime = DateTime.Now;
            dllEntry.Size = new FileInfo(path).Length;
            s.PutNextEntry(dllEntry);
            byte[] dllBytes = File.ReadAllBytes(path);
            s.Write(dllBytes, 0, dllBytes.Length);
        }

        public bool ShouldAddAgentDllsToContentZip()
        {
            string agentFilesDirectory = Path.Combine(_applicationPathProvider.GetApplicationPath(),
              "agentinstallerfiles");
            string contentZipPath = Path.Combine(agentFilesDirectory, "Content.zip");
            FileInfo contentZipFileInfo = new FileInfo(contentZipPath);
            return contentZipFileInfo.Length / 1024 < 1000;
        }
    }
}
