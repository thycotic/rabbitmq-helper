using Thycotic.CLI.Fragments;

namespace Thycotic.RabbitMq.Helper.Commands.Installation.Choice
{
    internal class RabbitMqLicenseChoiceConsoleCommandFragment : BinaryCommandFragment
    {
        public RabbitMqLicenseChoiceConsoleCommandFragment()
        {
            var oldAction = Action;
            Action = parameters =>
            {
                bool agree;
                if (parameters.TryGetBoolean("agreeRabbitMqLicense", out agree) &&
                    agree)
                {
                    return WhenTrue.Action.Invoke(parameters);
                }

                return oldAction.Invoke(parameters);
            };
        }
    }
}