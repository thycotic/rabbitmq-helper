using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using Thycotic.Messages.Common;

namespace Thycotic.Messages.DE.Areas.ActiveDirectory
{
    /// <summary>
    /// Requests a query on AD that returns all user for a domain.
    /// </summary>
    [DataContract]
    public class AllUsersByDomainQueryMessage : BasicConsumableBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="batchId"></param>
        /// <param name="domainInfo"></param>
        public AllUsersByDomainQueryMessage(Guid batchId, DomainInfo domainInfo)
        {
            Contract.Requires<ArgumentNullException>(batchId != null);
            Contract.Requires<ArgumentNullException>(domainInfo != null);

            this.BatchId = batchId;
            this.DomainInfo = domainInfo;
        }
        
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Guid BatchId { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public DomainInfo DomainInfo { get; private set; }
    }
}