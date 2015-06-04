using Thycotic.CLI;

namespace Thycotic.RabbitMq.Helper.Installation.Choice
{
    internal class SslChoiceConsoleCommandFragment : BinaryConsoleCommandFragment
    {
        public SslChoiceConsoleCommandFragment()
        {
            var oldAction = Action;
            Action = parameters =>
            {
                bool useSsl;
                if (parameters.TryGetBoolean("useSsl", out useSsl) && useSsl)
                {
                    return WhenTrue.Action.Invoke(parameters);
                }

                bool noSsl;
                if (parameters.TryGetBoolean("noSsl", out noSsl) && noSsl)
                {
                    return WhenFalse.Action.Invoke(parameters);
                }

                return oldAction.Invoke(parameters);
            };
        }
    }
}