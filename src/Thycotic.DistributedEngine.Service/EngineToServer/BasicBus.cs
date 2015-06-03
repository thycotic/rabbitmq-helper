using System;
using Thycotic.DistributedEngine.EngineToServerCommunication;
using Thycotic.DistributedEngine.Logic.Update;
using Thycotic.Logging;

namespace Thycotic.DistributedEngine.Service.EngineToServer
{
    /// <summary>
    /// Basic bus
    /// </summary>
    public abstract class BasicBus
    {
        private readonly IEngineToServerConnectionManager _engineToServerConnectionManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="engineToServerConnectionManager"></param>
        protected BasicBus(IEngineToServerConnectionManager engineToServerConnectionManager)
        {
            _engineToServerConnectionManager = engineToServerConnectionManager;
        }

        /// <summary>
        /// Wraps the interaction.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        protected T WrapInteraction<T>(Func<IEngineToServerCommunicationWcfService, T> func, IEngineToServerCommunicationCallback callback)
        {
            try
            {
                using (var channel = _engineToServerConnectionManager.OpenLiveChannel(callback))
                {
                    return func.Invoke(channel);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Bus broken down", ex);
            }
        }

        /// <summary>
        /// Wraps the interaction
        /// </summary>
        /// <param name="action"></param>
        /// <param name="callback"></param>
        protected void WrapInteraction(Action<IEngineToServerCommunicationWcfService> action, IEngineToServerCommunicationCallback callback)
        {
            try
            {
                using (var channel = _engineToServerConnectionManager.OpenLiveChannel(callback))
                {
                    action.Invoke(channel);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Bus broken down", ex);
            }
        }

        /// <summary>
        /// Wraps the interaction
        /// </summary>
        /// <param name="action"></param>
        protected void WrapInteraction(Action<IUpdateWebClient> action)
        {
            try
            {
                var channel = _engineToServerConnectionManager.OpenLiveUpdateWebClient();                
                action.Invoke(channel);                
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Bus broken down", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        protected T WrapInteraction<T>(Func<IUpdateWebClient, T> func)
        {
            try
            {
                var webClient = _engineToServerConnectionManager.OpenLiveUpdateWebClient();                
                return func.Invoke(webClient);                
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Bus broken down", ex);
            }
        }

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
