using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
        private readonly ILogWriter _log = Log.Get(typeof(SecretRunDependenciesConsumer));

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
            _log.Info(string.Format("Got a change dependency request for Secret Id {0}", request.SecretId));

            var guid = Guid.NewGuid();
            var messages = new List<DependencyChangeResponseMessageToLocalize>();
            for (int index = 0; index < request.DependencyChangeInfos.Length; index++)
            {
                var response = new DependencyChangeResponse();
                try
                {
                    messages = new List<DependencyChangeResponseMessageToLocalize>();
                    var info = request.DependencyChangeInfos[index];
                    response.SecretId = request.SecretId;
                    response.SecretDependencyId = info.SecretDependencyId;
                    response.TransactionGuid = guid;

                    Thread.Sleep(info.WaitBeforeSeconds * 1000);
                    messages.Add(GetDependencyStartedLogEntry(info));
                    var result = new DependencyChangeDispatcher(request.WmiTimeout).ExecuteDependencyAction(info);
                    messages.Add(GetDependencyFinishedLogEntry(info, result));

                    response.Success = result.Success;
                    response.StatusMessages = result.Errors.Select(s => s.DetailedMessage).ToArray();
                    response.LogMessages = messages.ToArray();
                    response.Last = index == request.DependencyChangeInfos.Length - 1;

                    _responseBus.ExecuteAsync(response);
                }
                catch (Exception ex)
                {
                    response = new DependencyChangeResponse
                    {
                        Success = false,
                        SecretId = request.SecretId,
                        TransactionGuid = guid,
                        StatusMessages = new[] { ex.ToString() },
                        Last = index == request.DependencyChangeInfos.Length - 1,
                        LogMessages = messages.ToArray(),
                    };
                    _responseBus.ExecuteAsync(response);
                }
                finally
                {
                    _log.Info(string.Format("Completed processing dependency request for Secret Id {0}. Successful: {1}", request.SecretId, response.Success ? "Yes" : "No"));
                }
            }
        }

        private DependencyChangeResponseMessageToLocalize GetDependencyStartedLogEntry(IDependencyChangeInfo info)
        {
            var hasMachine = !string.IsNullOrWhiteSpace(info.MachineName);
            return !hasMachine
                    ? new DependencyChangeResponseMessageToLocalize { MessageName = "DependencyStartedRunningOnMachineOnSite", Params = new object[] { info.ServiceName, info.MachineName, _exchangeNameProvider.GetCurrentExchange() } }
                    : new DependencyChangeResponseMessageToLocalize { MessageName = "DependencyStartedRunningOnSite", Params = new object[] { info.ServiceName, _exchangeNameProvider.GetCurrentExchange() } };

        }

        private DependencyChangeResponseMessageToLocalize GetDependencyFinishedLogEntry(IDependencyChangeInfo info, OperationResult result)
        {
            var machineName = (info.MachineName ?? "").Trim();
            return !string.IsNullOrWhiteSpace(machineName)
                ? new DependencyChangeResponseMessageToLocalize { Success = result.Success, MessageName = result.Success ? "SuccessfullyUpdatedDependencyOnMachine" : "FailedToUpdateDependencyOnMachine", Params = new object[] { info.ServiceName, machineName } }
                : new DependencyChangeResponseMessageToLocalize { Success = result.Success, MessageName = result.Success ? "SuccessfullyUpdatedDependency" : "FailedToUpdateDependency", Params = new object[] { info.ServiceName } };
        }

        /// <summary>
        /// Objects the invariant.
        /// </summary>
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this._log != null);
        }
    }
}
