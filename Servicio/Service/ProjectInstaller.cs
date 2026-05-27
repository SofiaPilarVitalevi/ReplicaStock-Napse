using System;
using System.ComponentModel;
using System.Configuration;

namespace Service
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        private String servName = ConfigurationManager.AppSettings.Get("name");
        private String servDesc = ConfigurationManager.AppSettings.Get("desc");

        public ProjectInstaller()
        {
            InitializeComponent();

            this.serviceInstaller1.ServiceName = servName;
            this.serviceInstaller1.DisplayName = servName;
            this.serviceInstaller1.Description = servDesc;
        }
    }
}
