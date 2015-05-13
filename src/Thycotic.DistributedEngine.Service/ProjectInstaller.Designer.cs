namespace Thycotic.DistributedEngine.Service
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._serviceInstaller = new System.ServiceProcess.ServiceInstaller();
            this._serviceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            // 
            // _serviceInstaller
            // 
            this._serviceInstaller.DisplayName = "Thycotic.DistributedEngine.Service";
            this._serviceInstaller.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this._serviceProcessInstaller});
            this._serviceInstaller.ServiceName = "Thycotic.DistributedEngine.Service";
            this._serviceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // _serviceProcessInstaller
            // 
            this._serviceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this._serviceProcessInstaller.Password = null;
            this._serviceProcessInstaller.Username = null;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this._serviceInstaller,
            this._serviceProcessInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller _serviceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller _serviceInstaller;
    }
}