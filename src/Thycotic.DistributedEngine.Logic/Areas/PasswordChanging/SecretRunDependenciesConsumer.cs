using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.Dependency.Response;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.General;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.Logging;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.Messages.Common;
using Thycotic.Messages.PasswordChanging.Request;
using Thycotic.PasswordChangers;
using Thycotic.PasswordChangers.DependencyChangers;
using Thycotic.SharedTypes.Dependencies;

namespace Thycotic.DistributedEngine.Logic.Areas.PasswordChanging
{
    /// <summary>
    /// Secret  change password request
    /// </summary>
    public class SecretRunDependenciesConsumer : IBasicConsumer<SecretChangeDependencyMessage>
    {
        private readonly IExchangeNameProvider _exchangeNameProvider;
        private readonly IResponseBus _responseBus;
        private readonly ILogWriter _log = Log.Get(typeof(SecretChangePasswordConsumer));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="responseBus"></param>
        /// <param name="exchangeNameProvider"></param>
        public SecretRunDependenciesConsumer(IResponseBus responseBus, IExchangeNameProvider exchangeNameProvider)
        {
            _responseBus = responseBus;
            _exchangeNameProvider = exchangeNameProvider;
        }

        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public void Consume(SecretChangeDependencyMessage request)
        {
            _log.Info(string.Format("Got a change dependency request for Secret Id {0}:", request.SecretId));

            try
            {
                // TODO: Fix 30 second wire-up problem.  This needs to be part of the individual message as well?
                var guid = Guid.NewGuid();
                for (int index = 0; index < request.DependencyChangeInfos.Length; index++)
                {
                    var info = request.DependencyChangeInfos[index];
                    var response = new DependencyChangeResponse();
                    var messages = new List<DependencyChangeResponseMessageToLocalize>();
                    response.SecretId = request.SecretId;
                    response.SecretDependencyId = info.SecretDependencyId;
                    response.TransactionGuid = guid;

                    Thread.Sleep(info.WaitBeforeSeconds*1000);
                    messages.Add(GetDependencyStartedLogEntry(info));
                    var result = new DependencyChangeDispatcher(30).ExecuteDependencyAction(info);
                    messages.Add(GetDependencyFinishedLogEntry(info, result));

                    response.Success = result.Success;
                    response.StatusMessages = result.Errors.Select(s => s.DetailedMessage).ToArray();
                    response.LogMessages = messages.ToArray();
                    response.Last = index == request.DependencyChangeInfos.Length - 1;
                    try
                    {
                        _responseBus.Execute(response);
                    }
                    catch (Exception)
                    {
                        _log.Error("Failed to record the change dependency response back to server");
                        //TODO: Retry?
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error("Handle specific error here", ex);
                // don't throw, instead report it.
                throw;
            }
        }

        private DependencyChangeResponseMessageToLocalize GetDependencyStartedLogEntry(IDependencyChangeInfo info)
        {
            var hasMachine = !string.IsNullOrEmpty(info.MachineName);
            return !hasMachine
                    ? new DependencyChangeResponseMessageToLocalize {MessageName = "DependencyStartedRunningOnMachineOnExchange", Params = new object[] { info.ServiceName, info.MachineName, _exchangeNameProvider.GetCurrentExchange() }}
                    : new DependencyChangeResponseMessageToLocalize {MessageName = "DependencyStartedRunningOnExchange", Params = new object[] { info.ServiceName, _exchangeNameProvider.GetCurrentExchange() }};
           
        }

        private DependencyChangeResponseMessageToLocalize GetDependencyFinishedLogEntry(IDependencyChangeInfo info, OperationResult result)
        {
            var machineName = (info.MachineName ?? "").Trim();
            return !string.IsNullOrEmpty(machineName)
                ? new DependencyChangeResponseMessageToLocalize { Success = result.Success, MessageName = "SuccessfullyUpdatedDependencyOnMachine", Params = new object[] { info.ServiceName, machineName } }
                : new DependencyChangeResponseMessageToLocalize {Success = result.Success, MessageName = result.Success ? "SuccessfullyUpdatedDependency" : "FailedToUpdateDependency", Params = new object[] {info.ServiceName}};
        }
    }
}
