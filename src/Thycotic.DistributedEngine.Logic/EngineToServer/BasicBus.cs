using System;

namespace Thycotic.DistributedEngine.Logic.EngineToServer
{
    /// <summary>
    /// Basic bus
    /// </summary>
    public abstract class BasicBus
    {
        /// <summary>
        /// Wraps the interaction.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">The function.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">Bus broken down</exception>
        protected static T WrapInteraction<T>(Func<T> func)
        {
            try
            {
                return func.Invoke();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Bus broken down", ex);
            }
        }

        /// <summary>
        /// Wraps the interaction.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <exception cref="System.ApplicationException">Bus broken down</exception>
        protected static void WrapInteraction(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Bus broken down", ex);
            }
        }

    }
}
