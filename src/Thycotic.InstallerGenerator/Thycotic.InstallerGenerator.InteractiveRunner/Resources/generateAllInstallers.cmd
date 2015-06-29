@echo off

REM reset colors
color

SET ig=%~dp0\..\Thycotic.InstallerGenerator.InteractiveRunner.exe
SET artifactPath=C:\Users\dobroslav.kolev\Desktop\bits
SET illidanMapPath=M:\development\repos\distributedengine

SET pfxPath=%illidanMapPath%\src\Thycotic.InstallerGenerator\Thycotic.InstallerGenerator.InteractiveRunner\Examples\Signing\SSDESPC.pfx
SET pfxPassword=password1

echo Generating generic installers/updaters...

echo 64Bit Distributed Engine
%ig% generateGenericDistributedEngineServiceMsi -ArtifactName=gdesvc.msi -ArtifactPath=%artifactPath% -SourcePath.Recipes=%illidanMapPath%\src\Thycotic.DistributedEngine.Service.Wix -SourcePath.Binaries=%illidanMapPath%\src\Thycotic.DistributedEngine.Service\bin\Release -Installer.Version=5.0.0.0 -Signing.PfxPath=%pfxPath% -Signing.PfxPassword=%pfxPassword%
if errorlevel 1 goto end

echo 32Bit Distributed Engine
%ig% generateGenericDistributedEngineServiceMsi -Is32Bit=true -ArtifactName=gdesvc.32Bit.msi -ArtifactPath=%artifactPath% -SourcePath.Recipes=%illidanMapPath%\src\Thycotic.DistributedEngine.Service.Wix.32Bit -SourcePath.Binaries=%illidanMapPath%\src\Thycotic.DistributedEngine.Service\bin\Release.32Bit -Installer.Version=5.0.0.0 -Signing.PfxPath=%pfxPath% -Signing.PfxPassword=%pfxPassword%
if errorlevel 1 goto end

echo 64Bit Memory Mq site connector
%ig% generateGenericMemoryMqSiteConnectorServiceMsi  -ArtifactName=gmmqsvc.msi  -ArtifactPath=%artifactPath% -SourcePath.Recipes=%illidanMapPath%\src\Thycotic.MemoryMq.SiteConnector.Service.Wix -SourcePath.Binaries=%illidanMapPath%\src\Thycotic.MemoryMq.SiteConnector.Service\bin\Release -Installer.Version=5.0.0.0 -Signing.PfxPath=%pfxPath% -Signing.PfxPassword=%pfxPassword%
if errorlevel 1 goto end

echo 32Bit Legacy Agent Update
%ig% generateGenericLegacyAgentServiceZip -ArtifactName=glasvc.zip -ArtifactPath=%artifactPath% -SourcePath.Binaries=-SourcePath.Binaries=%illidanMapPath%\src\Thycotic.DistributedEngine.Service\bin\Release.32Bit -Installer.Version=5.0.0.0 -Signing.PfxPath=%pfxPath% -Signing.PfxPassword=%pfxPassword%
if errorlevel 1 goto end

echo 32Bit Installer Runner
%ig% generateGenericInstallerRunnerZip -Is32Bit=true -ArtifactName=girapp.32Bit.zip -ArtifactPath=%artifactPath% -SourcePath.Binaries=-SourcePath.Binaries=%illidanMapPath%\src\Thycotic.InstallerGenerator\Thycotic.InstallerRunner\bin\Release.32Bit -Installer.Version=5.0.0.0 -Signing.PfxPath=%pfxPath% -Signing.PfxPassword=%pfxPassword%
if errorlevel 1 goto end

echo 64Bit Installer Runner
%ig% generateGenericInstallerRunnerZip -ArtifactName=girapp.zip -ArtifactPath=%artifactPath% -SourcePath.Binaries=-SourcePath.Binaries=%illidanMapPath%\src\Thycotic.InstallerGenerator\Thycotic.InstallerRunner\bin\Release -Installer.Version=5.0.0.0 -Signing.PfxPath=%pfxPath% -Signing.PfxPassword=%pfxPassword%
if errorlevel 1 goto end

echo.
echo NO SIGNING PAST THIS POINT
echo.

echo Generating configured installers/updaters...
echo 64Bit Memory Mq site connector
%ig% generateConfiguredMemoryMqSiteConnectorServiceZip -ArtifactName=cmmqsvc.zip  -ArtifactPath=%artifactPath% -SourcePath.MSI=%artifactPath%\gmmqsvc.msi -SourcePath.RunnerZip=%artifactPath%\girapp.zip -Installer.Version=5.0.0.0 -Pipeline.ConnectionString=net.tcp://illidan:8671 -Pipeline.UseSsl=false -Pipeline.Thumbprint=invalid

echo 64Bit Distributed Engine
%ig% generateConfiguredDistributedEngineServiceZip -ArtifactName=cdesvc.zip  -ArtifactPath=%artifactPath% -SourcePath.MSI=%artifactPath%\gdesvc.msi -SourcePath.RunnerZip=%artifactPath%\girapp.zip -Installer.Version=5.0.0.0 -E2S.ConnectionString=http://illidan/ihawu -E2S.UseSsl=false -E2S.SiteId=3 -E2S.OrganizationId=1

echo 32Bit Distributed Engine
%ig% generateConfiguredDistributedEngineServiceZip -Is32Bit=true -ArtifactName=cdesvc.32Bit.zip  -ArtifactPath=%artifactPath% -SourcePath.MSI=%artifactPath%\gdesvc.32Bit.msi -SourcePath.RunnerZip=%artifactPath%\girapp.zip -Installer.Version=5.0.0.0 -E2S.ConnectionString=http://illidan/ihawu -E2S.UseSsl=false -E2S.SiteId=3 -E2S.OrganizationId=1

if errorlevel 1 goto end

echo.
echo Generation successful!

:end
if errorlevel 1 (
	REM dark red
	color 04
	echo Generation failed with code %errorlevel%
	exit /b %errorlevel%
)
