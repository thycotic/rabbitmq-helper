using System;
using System.Management;

namespace Thycotic.WindowsService.Bootstraper
{
	public partial interface IWin32Service
	{
		string OriginatingNamespace { get; }
		string ManagementClassName { get; }
		ManagementScope Scope { get; set; }
		bool AutoCommit { get; set; }
		ManagementPath Path { get; set; }
		bool IsAcceptPauseNull { get; }
		bool AcceptPause { get; }
		bool IsAcceptStopNull { get; }
		bool AcceptStop { get; }
		string Caption { get; }
		bool IsCheckPointNull { get; }
		uint CheckPoint { get; }
		string CreationClassName { get; }
		string Description { get; }
		bool IsDesktopInteractNull { get; }
		bool DesktopInteract { get; }
		string DisplayName { get; }
		string ErrorControl { get; }
		bool IsExitCodeNull { get; }
		uint ExitCode { get; }
		bool IsInstallDateNull { get; }
		DateTime InstallDate { get; }
		string Name { get; }
		string PathName { get; }
		bool IsProcessIdNull { get; }
		uint ProcessId { get; }
		bool IsServiceSpecificExitCodeNull { get; }
		uint ServiceSpecificExitCode { get; }
		string ServiceType { get; }
		bool IsStartedNull { get; }
		bool Started { get; }
		string StartMode { get; }
		string StartName { get; }
		string State { get; }
		string Status { get; }
		string SystemCreationClassName { get; }
		string SystemName { get; }
		bool IsTagIdNull { get; }
		uint TagId { get; }
		bool IsWaitHintNull { get; }
		uint WaitHint { get; }
		void CommitObject();
		void CommitObject(PutOptions putOptions);
		void Delete();
		uint Change(bool DesktopInteract, string DisplayName, byte ErrorControl, string LoadOrderGroup, string[] LoadOrderGroupDependencies, string PathName, string[] ServiceDependencies, byte ServiceType, string StartMode, string StartName, string StartPassword);
		uint ChangeStartMode(string StartMode);
		uint Create(bool DesktopInteract, string DisplayName, byte ErrorControl, string LoadOrderGroup, string[] LoadOrderGroupDependencies, string Name, string PathName, string[] ServiceDependencies, byte ServiceType, string StartMode, string StartName, string StartPassword);
		uint Delete0();
		ServiceWin32ReturnCode InterrogateService();
		uint PauseService();
		uint ResumeService();
		ServiceWin32ReturnCode StartService();
		ServiceWin32ReturnCode StopService();
		uint UserControlService(byte ControlCode);
	}
}