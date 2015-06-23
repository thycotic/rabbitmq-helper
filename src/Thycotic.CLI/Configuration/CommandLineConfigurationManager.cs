using System.Collections.Specialized;

namespace Thycotic.CLI.Configuration
{
    public static class CommandLineConfigurationManager
    {
        static CommandLineConfigurationManager()
        {
            AppSettings = new NameValueCollection();
        }

        public static NameValueCollection AppSettings { get; set; }
    }
}
