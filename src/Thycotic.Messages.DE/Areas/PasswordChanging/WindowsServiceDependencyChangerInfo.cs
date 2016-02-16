namespace Thycotic.Messages.DE.Areas.PasswordChanging
{
    /// <summary>
    /// Holds info for changing passwords on windows services
    /// </summary>
    public class WindowsServiceDependencyChangerInfo : IDependencyChangerInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string AccountUserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AccountPassword  { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AccountDomainName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool RestartOnPasswordChange { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool ForceStart { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string[] StoppedServices { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PrivilegedUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PrivilegedPassword { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PrivilegedDomainName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MachineName { get; set; }        
        /// <summary>
        /// 
        /// </summary>
        public string OldPassword { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int PasswordTypeId { get; set; }
    }
}
