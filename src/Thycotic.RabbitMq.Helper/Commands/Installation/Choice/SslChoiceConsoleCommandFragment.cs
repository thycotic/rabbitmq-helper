using Thycotic.CLI;
using Thycotic.CLI.Fragments;

namespace Thycotic.RabbitMq.Helper.Installation.Choice
{
    internal class SslChoiceConsoleCommandFragment : BinaryCommandFragment
    {
        public SslChoiceConsoleCommandFragment()
        {
            Action = parameters =>
            {
                bool useSsl;
                if (parameters.TryGetBoolean("useSsl", out useSsl) && useSsl)
                {
                    return WhenTrue.Action.Invoke(parameters);
                }
                else
                {
                    return WhenFalse.Action.Invoke(parameters);
                }
            };
        }
    }
}