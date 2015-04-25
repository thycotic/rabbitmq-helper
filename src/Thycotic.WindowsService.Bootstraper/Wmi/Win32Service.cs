using System;
using System.Linq;
using System.Management;

namespace Thycotic.WindowsService.Bootstraper.Wmi
{
	internal partial class Win32Service
	{

		public class States
		{
			public const string Stopped = "Stopped";
			public const string StartPending = "Start Pending";
			public const string StopPending = "Stop Pending";
			public const string Running = "Running";
			public const string ContinuePending = "Continue Pending";
			public const string PausePending = "Pause Pending";
			public const string Paused = "Paused";
			public const string Unknown = "Unknown";
		}

		partial void PreInitialize()
		{
		}

		public virtual IWin32Service[] GetDependentServices()
		{
			var query = new WqlObjectQuery(String.Format("Associators of {{Win32_Service.Name='{0}'}} Where AssocClass=Win32_DependentService Role=Antecedent", Name));
			using (var managementObjectSearcher = new ManagementObjectSearcher(Scope, query))
			{
				var managementObjects = managementObjectSearcher.Get().OfType<ManagementObject>().ToArray();
			    return Array.ConvertAll(managementObjects, m => (IWin32Service) new Win32Service(m));
			}
		}

		public virtual void Refresh()
		{
			PrivateLateBoundObject.Get();
		}

		public ServiceWin32ReturnCode StopServiceSynchronous(int maxWaitSeconds)
		{
			var returnCode = StopService();
			if (returnCode != ServiceWin32ReturnCode.Success)
			{
				return returnCode;
			}
			var secondsElapsed = 0;
			do
			{
				if (maxWaitSeconds < secondsElapsed)
				{
					return ServiceWin32ReturnCode.ServiceRequestTimeout;
				}
				Refresh();
				secondsElapsed++;
			}
			while (State != States.Stopped);
			return returnCode;
		}

		public ServiceWin32ReturnCode StartServiceSynchronous(int maxWaitSeconds)
		{
			var returnCode = StartService();
			if (returnCode != ServiceWin32ReturnCode.Success)
			{
				return returnCode;
			}
			var secondsElapsed = 0;
			do
			{
				if (maxWaitSeconds < secondsElapsed)
				{
					return ServiceWin32ReturnCode.ServiceRequestTimeout;
				}
				Refresh();
				secondsElapsed++;
			}
			while (State != States.Running);
			return returnCode;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				PrivateLateBoundObject.Dispose();
			}
			base.Dispose(disposing);
		}

	    public static string GetLocalServiceManagementPath(string serviceName)
	    {
	        return string.Format("\\\\{0}\\root\\cimv2:Win32_Service.Name='{1}'", Environment.MachineName, serviceName);
	    }
	}
}
