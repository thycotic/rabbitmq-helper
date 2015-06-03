@echo off

echo Working inside of %~dp0%

SET CWD = %cd%
SET ERLANG_INSTALLERPATH=%~dp0%\otp_win64_17.5.exe
SET ERLANG_INSTALLPATH=%programfiles%\erl6.4
SET ERLANG_UNINSTALLERPATH=%programfiles%\erl6.4\uninstall.exe
SET RABBITMQ_INSTALLERPATH=%~dp0%\rabbitmq-server-3.5.3.exe
SET RABBITMQ_INSTALLPATH=%programfiles(x86)%\RabbitMQ Server\rabbitmq_server-3.5.3
SET RABBITMQ_BINPATH=%programfiles(x86)%\RabbitMQ Server\rabbitmq_server-3.5.3\sbin
SET RABBITMQ_UNINSTALLERPATH=%programfiles(x86)%\RabbitMQ Server\uninstall.exe

SET RABBITMQ_BASE=%ProgramFiles%\Thycotic Software Ltd\RabbitMq Site Connector

SET /P useSsl=Would you be using encryption? [y/N] || Set useSsl=N

IF EXIST "%RABBITMQ_UNINSTALLERPATH%" (
	echo Uninstalling previous instance of RabbitMq
	cmd /C "%RABBITMQ_UNINSTALLERPATH%" /S
)

IF EXIST "%RABBITMQ_INSTALLPATH%" (
	echo Deleting RabbitMq install directory
	rd /s /q "%RABBITMQ_INSTALLPATH%"
)

IF EXIST "%ERLANG_UNINSTALLERPATH%" (
	echo Uninstalling previous instance of Erlang
	cmd /C "%ERLANG_UNINSTALLERPATH%" /S
)

echo Installing Erlang runtime
cmd /C "%ERLANG_INSTALLERPATH%" /S
echo Done

IF NOT EXIST "%RABBITMQ_BASE%" (
	echo Creating %RABBITMQ_BASE%
	mkdir "%RABBITMQ_BASE%"
)

echo Setting system environmental variable RABBITMQ_BASE to %RABBITMQ_BASE%
setx -m RABBITMQ_BASE "%RABBITMQ_BASE%"


echo Installing RabbitMq
cmd /C "%RABBITMQ_INSTALLERPATH%" /S
echo Done

IF %useSsl% == y (
	IF NOT EXIST "%RABBITMQ_BASE%\rabbitmq.config" (
		echo Copying RabbitMq config file
		move "%RABBITMQ_INSTALLPATH%\etc\rabbitmq.config.example" "%RABBITMQ_BASE%\rabbitmq.config"
	) ELSE (
		echo Rabbit Mq config file already exists.
	)

	echo Encryption settings require changes to the RabbitMq configuration file. 
	echo The config file will be opened next...
	pause

	echo Editing RabbitMq config file
	cmd /C notepad.exe "%RABBITMQ_BASE%\rabbitmq.config"

	echo Configuration changes require rabbit service installation. 
	echo Please, press OK when the RabbitMq installer asks you to reinstall.
	pause

	echo Re-installing RabbitMq
	
	cmd /C "%RABBITMQ_INSTALLERPATH%" /S

) ELSE (
	echo Skipping encryption support.
)

echo Installing management plug-in
cd "%RABBITMQ_BINPATH%"
cmd /C "rabbitmq-plugins enable rabbitmq_management"

cd %CWD%

echo Done


