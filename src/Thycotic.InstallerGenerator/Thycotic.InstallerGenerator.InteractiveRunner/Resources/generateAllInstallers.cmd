@echo off

SET ig=start /wait cmd /c %~dp0\..\Thycotic.InstallerGenerator.InteractiveRunner.exe
SET artifactPath=C:\Users\dobroslav.kolev\Desktop\bits
SET illidanMapPath=M:\development\repos\distributedengine

SET pfxPath=%illidanMapPath%\src\Thycotic.InstallerGenerator\Thycotic.InstallerGenerator.InteractiveRunner\Examples\Signing\SSDESPC.pfx
SET pfxPassword=password1

echo Generating generic installers/updaters...
echo 64Bit Distributed Engine
REM %ig% generateGenericDistributedEngineServiceMsi -ArtifactName=gdesvc.msi -ArtifactPath=%artifactPath% -SourcePath.Recipes=%illidanMapPath%\src\Thycotic.DistributedEngine.Service.Wix -SourcePath.Binaries=%illidanMapPath%\src\Thycotic.DistributedEngine.Service\bin\Release -Installer.Version=5.0.0.0 -Signing.PfxPath=%pfxPath% -Signing.PfxPassword=%pfxPassword%
echo 32Bit Distributed Engine
REM %ig% generateGenericDistributedEngineServiceMsi -Is32Bit=true -ArtifactName=gdesvc.32Bit.msi -ArtifactPath=%artifactPath% -SourcePath.Recipes=%illidanMapPath%\src\Thycotic.DistributedEngine.Service.Wix.32Bit -SourcePath.Binaries=%illidanMapPath%\src\Thycotic.DistributedEngine.Service\bin\Release.32Bit -Installer.Version=5.0.0.0 -Signing.PfxPath=%pfxPath% -Signing.PfxPassword=%pfxPassword%
echo 64Bit Memory Mq site connector
REM %ig% generateGenericMemoryMqSiteConnectorServiceMsi  -ArtifactName=gmmqsvc.msi  -ArtifactPath=%artifactPath% -SourcePath.Recipes=%illidanMapPath%\src\Thycotic.MemoryMq.SiteConnector.Service.Wix -SourcePath.Binaries=%illidanMapPath%\src\Thycotic.MemoryMq.SiteConnector.Service\bin\Release -Installer.Version=5.0.0.0 -Signing.PfxPath=%pfxPath% -Signing.PfxPassword=%pfxPassword%
echo 32Bit Legacy Agent Update
REM %ig% generateGenericLegacyAgentServiceZip -ArtifactName=glasvc.zip -ArtifactPath=%artifactPath% -SourcePath.Binaries=-SourcePath.Binaries=%illidanMapPath%\src\Thycotic.DistributedEngine.Service\bin\Release.32Bit -Installer.Version=5.0.0.0 
echo 32Bit Installer Runner
%ig% generateGenericInstallerRunnerZip -ArtifactName=girapp.zip -ArtifactPath=%artifactPath% -SourcePath.Binaries=-SourcePath.Binaries=%illidanMapPath%\src\Thycotic.InstallerGenerator\Thycotic.InstallerRunner\bin\Release.32Bit -Installer.Version=5.0.0.0 -Signing.PfxPath=%pfxPath% -Signing.PfxPassword=%pfxPassword%

echo Generating configured installers/updaters...
echo 64Bit Memory Mq site connector
REM %ig% generateConfiguredMemoryMqSiteConnectorServiceZip -ArtifactName=cmmqsvc.zip  -ArtifactPath=%artifactPath% -SourcePath.MSI=%artifactPath%\gmmqsvc.msi -Installer.Version=5.0.0.0 -Signing.PfxPath=%pfxPath% -Signing.PfxPassword=%pfxPassword%