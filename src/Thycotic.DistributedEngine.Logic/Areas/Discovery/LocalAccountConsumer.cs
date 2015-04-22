using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Thycotic.AppCore;
using Thycotic.Discovery.Core.Results;
using Thycotic.Discovery.Sources.Scanners;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.Discovery.Response;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.Logging;
using Thycotic.Messages.Areas.Discovery.Request;
using Thycotic.Messages.Common;
using Thycotic.SharedTypes.General;

namespace Thycotic.DistributedEngine.Logic.Areas.Discovery
{
    /// <summary>
    /// Local Account Consumer
    /// </summary>
    public class LocalAccountConsumer : IBasicConsumer<ScanLocalAccountMessage>
    {
        private const string TOKEN_SEPARATOR = "%$#@@||@@#$%";
        private readonly int _maxLogByteSize = 4000;
        private readonly IResponseBus _responseBus;
        private readonly IScannerFactory _scannerFactory;
        private readonly ILogWriter _log = Log.Get(typeof(LocalAccountConsumer));

        /// <summary>
        /// Local Account Consumer
        /// </summary>
        /// <param name="responseBus"></param>
        /// <param name="scannerFactory"></param>
        public LocalAccountConsumer(IResponseBus responseBus, IScannerFactory scannerFactory)
        {
            _responseBus = responseBus;
            _scannerFactory = scannerFactory;
        }

        /// <summary>
        /// Scan Local Accounts
        /// </summary>
        /// <param name="request"></param>
        public void Consume(ScanLocalAccountMessage request)
        {
            try
            {
                _log.Info(string.Format("{0} : Scan Local Accounts", request.Input.ComputerName));
                var scanner = _scannerFactory.GetDiscoveryScanner(request.DiscoveryScannerId);
                var result = scanner.ScanComputerForLocalAccounts(request.Input);

                var batchId = Guid.NewGuid();
                var paging = new Paging
                {
                    Total = result.LocalAccounts.Count(),
                    Take = 3
                };
                if (paging.PageCount == 0)
                {
                    _log.Info(string.Format("{0} : {1}", request.Input.NameForLog, string.Join(Environment.NewLine, result.Logs)));

                    var response = new ScanLocalAccountResponse
                    {
                        ComputerId = request.ComputerId,
                        DiscoverySourceId = request.DiscoverySourceId,
                        LocalAccounts = result.LocalAccounts.Skip(paging.Skip).Take(paging.Take).ToArray(),
                        Success = result.Success,
                        ErrorCode = result.ErrorCode,
                        StatusMessages = { },
                        Logs = new List<Thycotic.Discovery.Core.Results.DiscoveryLog>() { },//result.Logs.Skip(paging.Skip).Take(paging.Take).ToList(),
                        ErrorMessage = result.ErrorMessage,
                        BatchId = batchId,
                        Paging = paging
                    };
                    TryReturnResult(request, response, paging);
                }
                Enumerable.Range(0, paging.PageCount).ToList().ForEach(x =>
                {
                    var response = new ScanLocalAccountResponse
                    {
                        ComputerId = request.ComputerId,
                        DiscoverySourceId = request.DiscoverySourceId,
                        LocalAccounts = result.LocalAccounts.Skip(paging.Skip).Take(paging.Take).ToArray(),
                        Success = result.Success,
                        ErrorCode = result.ErrorCode,
                        StatusMessages = { },
                        Logs = new List<Thycotic.Discovery.Core.Results.DiscoveryLog>(){},//result.Logs.Skip(paging.Skip).Take(paging.Take).ToList(),
                        ErrorMessage = result.ErrorMessage,
                        BatchId = batchId,
                        Paging = paging
                    };

                    if (!response.Paging.HasNext)
                    {
                        
                        var logBytes = GetBytes(string.Join(TOKEN_SEPARATOR, result.Logs.Select(log=>log.Message)));
                        if (logBytes.Length < _maxLogByteSize)
                        {
                            response.Logs = result.Logs;
                        }
                        else
                        {
                            var logString = GetString(logBytes);
                            var logs = new StringSplitter().Split(TOKEN_SEPARATOR, logString).ToList();
                            logs.Add("...(Logs truncated due to size limitations)");
                            var logList = logs.Select(log => new DiscoveryLog {Message = log}).ToList();
                            response.Logs = logList;
                        }
                    }

                    TryReturnResult(request, response, paging);
                });
            }
            catch (Exception e)
            {
                _log.Info(string.Format("{0} : Scan Local Accounts Failed", request.Input.ComputerName), e);
            }
        }

        byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, _maxLogByteSize);
            return new string(chars);
        }

        private void TryReturnResult(ScanLocalAccountMessage request, ScanLocalAccountResponse response, Paging paging)
        {
            try
            {
                _log.Info(string.Format("{0}: Send Local Account Results", request.Input.NameForLog));
                _responseBus.Execute(response);
                paging.Skip = paging.NextSkip;
            }
            catch (Exception exception)
            {
                _log.Info(string.Format("{0}: Send Local Account Results Failed", request.Input.NameForLog),
                    exception);
            }
        }
    }
}
