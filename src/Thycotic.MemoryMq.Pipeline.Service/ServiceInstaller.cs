using System.ComponentModel;

namespace Thycotic.DistributedEngine
{
    /// <summary>
    /// Service installer
    /// </summary>
    [RunInstaller(true)]
    public partial class ServiceInstaller : System.Configuration.Install.Installer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceInstaller"/> class.
        /// </summary>
        public ServiceInstaller()
        {
            InitializeComponent();
        }
    }
}
