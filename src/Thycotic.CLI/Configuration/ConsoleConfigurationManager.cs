using System.Collections.Specialized;

namespace Thycotic.CLI.Configuration
{
    public static class ConsoleConfigurationManager
    {
        static ConsoleConfigurationManager()
        {
            AppSettings = new NameValueCollection();
        }

        public static NameValueCollection AppSettings { get; set; }
    }
}
