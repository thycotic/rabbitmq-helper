using System;

namespace Thycotic.MemoryMq.SiteConnector.Service
{
    class ReleaseInformationHelper
    {
        public static Version Version
        {
            get
            {
                return typeof(SiteConnectorService).Assembly.GetName().Version;
            }
        }
    }
}
