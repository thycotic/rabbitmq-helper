using System;
using System.IO;
using System.Threading;
using Thycotic.CLI.Commands;
using Thycotic.Logging;

namespace Thycotic.RabbitMq.Helper.Commands.Installation
{

    internal class PrintDownloadLocationsCommand : CommandBase, IImmediateCommand
    {
        private readonly ILogWriter _log = Log.Get(typeof (PrintDownloadLocationsCommand));

        public override string Area
        {
            get { return CommandAreas.Installation; }
        }

        public override string[] Aliases
        {
            get
            {
                return new[]
                {
                    "pdl"
                };
            }
            set { }
        }

        public override string Description
        {
            get { return "Prints download locations for Erlang and RabbitMq (most helpful when needing to do offline installation)"; }
        }

        public PrintDownloadLocationsCommand()
        {

            Action = parameters =>
            {
                Console.WriteLine("Erlang:  \t{0}", InstallationConstants.Erlang.DownloadUrl);
                Console.WriteLine("RabbitMq:\t{0}", InstallationConstants.RabbitMq.DownloadUrl);
                return 0;
            };
        }
    }
}
