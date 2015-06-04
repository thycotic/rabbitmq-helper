using Thycotic.CLI;

namespace Thycotic.RabbitMq.Helper.Installation.Choice
{
    internal class ErlangLicenseChoiceConsoleCommandFragment : BinaryConsoleCommandFragment
    {
        public ErlangLicenseChoiceConsoleCommandFragment()
        {
            var oldAction = Action;
            Action = parameters =>
            {
                bool agree;
                if (parameters.TryGetBoolean("agreeErlangLicense", out agree) &&
                    agree)
                {
                    return WhenTrue.Action.Invoke(parameters);
                }

                return oldAction.Invoke(parameters);
            };
        }
    }
}