using System;

namespace Thycotic.DistributedEngine.Service
{
    // A Temporary class to return the version of the Service.
    // After we fix the real ReleaseInformationHelper this class will go away and calls to it will use the ReleaseInformationHelper instead.
    class TempReleaseInformationHelper
    {
        public static Version Version
        {
            get
            {
                return typeof(EngineService).Assembly.GetName().Version;
            }
        }
    }
}
