using Thycotic.CLI;

namespace Thycotic.RabbitMq.Helper.Installation.Choice
{
    internal class RabbitMqLicenseChoiceConsoleCommandFragment : BinaryConsoleCommandFragment
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