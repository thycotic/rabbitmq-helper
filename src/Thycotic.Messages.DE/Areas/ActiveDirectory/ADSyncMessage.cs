using System.Collections.Generic;
using Thycotic.Messages.Common;

namespace Thycotic.Messages.DE.Areas.ActiveDirectory
{
    /// <summary>
    /// ADSync Message
    /// </summary>
    public class ADSyncMessage : BasicConsumableBase
    {

        private List<DomainInfo> _domains = new List<DomainInfo>();
        
        /// <summary>
        /// Domain and Group Information
        /// </summary>
        public List<DomainInfo> Domains
        {
            get
            {
                return _domains;
            }
            set
            {
                _domains = value;
            }
        }

        /// <summary>
        /// Number of groups to ask AD about at one time
        /// </summary>
        public int BatchSize { get; set; }
    }
}