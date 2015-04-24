using System;

namespace Thycotic.WindowsService.Bootstraper
{
	public partial interface IWin32Service : IDisposable
	{
		IWin32Service[] GetDependentServices();
		void Refresh();
		ServiceWin32ReturnCode StopServiceSynchronous(int maxWait);
		ServiceWin32ReturnCode StartServiceSynchronous(int maxWait);
	}
}