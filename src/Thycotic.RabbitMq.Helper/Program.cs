using System.Linq;
using System.Reflection;
using System.Threading;
using Autofac;
using Thycotic.CLI;
using Thycotic.CLI.Commands;
using Thycotic.Logging;
using Thycotic.RabbitMq.Helper.Installation;

namespace Thycotic.RabbitMq.Helper
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            Log.Configure();

            var initialCommand = string.Join(" ", args);

            var cli = new CommandLineInterface("Thycotic RabbitMq Helper");

            ConfigureCli(cli);

            cli.BeginInputLoop(initialCommand);

            return 0;
        }

        private static IContainer ConfigureCli(CommandLineInterface cli)
        {
            using (LogContext.Create("CLI configuration"))
            {
                cli.ClearCommands();

                var currentAssembly = Assembly.GetExecutingAssembly();

                // Create the builder with which components/services are registered.
                var builder = new ContainerBuilder();

                builder.Register(context => cli.CancellationToken).As<CancellationToken>().SingleInstance();
                //builder.Register(context => bus).As<IRequestBus>().SingleInstance();
                //builder.Register(context => exchangeNameProvider).As<IExchangeNameProvider>().SingleInstance();

                builder.RegisterAssemblyTypes(currentAssembly)
                    .Where(t => !t.IsAbstract)
                    .Where(t => typeof(ICommand).IsAssignableFrom(t))
                    .Where(t => t != typeof(SystemCommand));

                var container = builder.Build();

                var commands =
                    container.ComponentRegistry.Registrations.Where(
                        r => typeof(ICommand).IsAssignableFrom(r.Activator.LimitType));

                commands.ToList()
                    .ForEach(c => cli.AddCustomCommand((ICommand)container.Resolve(c.Activator.LimitType)));

                return container;
            }
        }


    }
}
