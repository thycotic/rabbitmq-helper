using System;

namespace Thycotic.DistributedEngine.Service
{
    class ReleaseInformationHelper
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
