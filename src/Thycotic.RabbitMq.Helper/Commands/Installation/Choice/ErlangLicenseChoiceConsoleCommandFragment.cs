using Thycotic.CLI.Fragments;

namespace Thycotic.RabbitMq.Helper.Commands.Installation.Choice
{
    internal class ErlangLicenseChoiceConsoleCommandFragment : BinaryCommandFragment
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