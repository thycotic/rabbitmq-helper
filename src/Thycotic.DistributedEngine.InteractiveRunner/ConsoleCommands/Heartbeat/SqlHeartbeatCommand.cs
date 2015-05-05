using System;
using System.Collections.Generic;
using Thycotic.Logging;
using Thycotic.MessageQueue.Client;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.Messages.Heartbeat.Request;

namespace Thycotic.DistributedEngine.InteractiveRunner.ConsoleCommands.Heartbeat
{
    class SqlHeartbeatCommand : ConsoleCommandBase
    {
        private readonly IRequestBus _bus;
        private readonly ILogWriter _log = Log.Get(typeof(SqlHeartbeatCommand));

        public override string Name
        {
            get { return "heartbeat_sql"; }
        }

        public override string Area {
            get { return CommandAreas.TestFunctions; }
        }

        public override string Description
        {
            get { return "Posts a SQL Heartbeat message to the exchange"; }
        }

        public SqlHeartbeatCommand(IRequestBus bus, IExchangeNameProvider exchangeNameProvider)
        {
            _bus = bus;


            Action = parameters =>
            {
                _log.Info("Posting message to exchange");

                //string server;
                //string username;
                //string password;
                //if (!parameters.TryGet("server", out server)) return;
                //if (!parameters.TryGet("username", out username)) return;
                //if (!parameters.TryGet("password", out password)) return;

                //var message = new SecretHeartbeatMessage
                //{
                //    PasswordInfoProvider =
                //        new GenericPasswordInfoProvider
                //        {
                //            PasswordTypeName = "Thycotic.AppCore.Federator.SqlAccountFederator",
                //            PasswordTypeId = 2
                //        }
                //};

                //var itemValues = new Dictionary<string, string>();
                //itemValues["server"] = server;
                //itemValues["username"] = username;
                //itemValues["password"] = password;

                //message.PasswordInfoProvider.ItemValues = itemValues;
                //message.PasswordInfoProvider["server"] = server;
                //message.PasswordInfoProvider["username"] = username;
                //message.PasswordInfoProvider["password"] = password;
                //try
                //{

                //    _bus.BasicPublish(exchangeNameProvider.GetCurrentExchange(), message);
                //    //if (!response.Success)
                //    //{
                //    //    _log.Error(response.StatusMessages.First());
                //    //}
                //}
                //catch (Exception ex)
                //{
                //    _log.Error("Heartbeat failed", ex);
                //}


                _log.Info("Posting completed");

            };
        }

        //private class InteractivePasswordInfoProvider : IPasswordInfoProvider
        //{
        //    private ArrayList _rowList = new ArrayList();

        //    public InteractivePasswordInfoProvider()
        //    {
        //        InfoDictionary = new Dictionary<string, string>();
        //    }

        //    public string GetValueFromDictionaryOrItems(string passwordFieldName)
        //    {
        //        if (InfoDictionary.ContainsKey(passwordFieldName))
        //        {
        //            return InfoDictionary[passwordFieldName];
        //        }
        //        if (ItemValues.ContainsKey(passwordFieldName))
        //        {
        //            return ItemValues[passwordFieldName];
        //        }
        //        return null;
        //    }

        //    public string GetValueFromItemsThenDictionary(string passwordFieldName)
        //    {
        //        if (ItemValues.ContainsKey(passwordFieldName))
        //        {
        //            string value = ItemValues[passwordFieldName];
        //            if (!string.IsNullOrEmpty(value))
        //            {
        //                return value;
        //            }
        //        }
        //        if (InfoDictionary.ContainsKey(passwordFieldName))
        //        {
        //            return InfoDictionary[passwordFieldName];
        //        }
        //        return null;
        //    }

        //    public IPasswordInfoProvider TranslateInfoDictionary()
        //    {
        //        (from x in InfoDictionary.Where(info => !string.IsNullOrEmpty(info.Key) && !string.IsNullOrEmpty(info.Value))
        //         join y in ItemValues.Where(info => !string.IsNullOrEmpty(info.Key) && !string.IsNullOrEmpty(info.Value)) on new { fld = x.Value.ToLower() } equals new { fld = "$" + y.Key.ToLower() }
        //         select new { x.Key, y.Value }).ToList().ForEach((x) =>
        //         {
        //             InfoDictionary[x.Key] = x.Value;
        //         });
        //        return this;
        //    }

        //    public IPasswordInfoProvider TranslateInfoDictionary(IDictionary<string, Func<string, string>> customScrubber)
        //    {
        //        (from info in InfoDictionary
        //         join item in ItemValues on new
        //         {
        //             fld = !string.IsNullOrEmpty(info.Value) ? info.Value.ToLower() : string.Empty
        //         } equals new
        //         {
        //             fld = "$" + (!string.IsNullOrEmpty(item.Key) ? item.Key.ToLower() : string.Empty)

        //         } into y_g
        //         from item in y_g.DefaultIfEmpty()
        //         join scrubber in customScrubber.Where(x => !string.IsNullOrEmpty(x.Key) && x.Value != null) on info.Key.ToLower() equals scrubber.Key.ToLower() into a_g
        //         from scrubber in a_g.DefaultIfEmpty()
        //         let unscrubbedValue = !string.IsNullOrEmpty(item.Value) ? item.Value : info.Value
        //         let replacementValue = scrubber.Value != null ? scrubber.Value.Invoke(unscrubbedValue) : unscrubbedValue
        //         where info.Value != replacementValue
        //         select new
        //         {
        //             info.Key,
        //             Value = replacementValue
        //         }).ToList().ForEach((x) =>
        //         {
        //             InfoDictionary[x.Key] = x.Value;
        //         });
        //        return this;
        //    }

        //    public Dictionary<string, string> ItemValues { get; set; }
        //    public string PasswordTypeName { get; set; }
        //    public string NewPassword { get; set; }
        //    public int PasswordTypeId { get; set; }
        //    public string LogName { get; set; }
        //    public string WebScript { get; set; }
        //    public string FederatorHelperName { get; set; }

        //    public string this[string passwordFieldName]
        //    {
        //        get
        //        {
        //            if (InfoDictionary.ContainsKey(passwordFieldName))
        //            {
        //                return InfoDictionary[passwordFieldName];
        //            }
        //            foreach (DataRow row in _rowList)
        //            {
        //                if (!row.IsNull("PasswordFieldName") && ((string)row["PasswordFieldName"]).ToUpper() == passwordFieldName.ToUpper())
        //                {
        //                    return row["ItemValue"] as string;
        //                }
        //            }
        //            return null;
        //        }
        //        set
        //        {
        //            InfoDictionary[passwordFieldName] = value;
        //        }
        //    }

        //    public List<IPasswordInfoProvider> AssociatedInfos { get; set; }
        //    public IPasswordInfoProvider PrivilegedAccountInfo { get; set; }
        //    public List<ICustomCommand> CustomCommands { get; set; }
        //    public LineEnding LineEnding { get; set; }
        //    public ILdapConnectionSettings LdapConnectionSettings { get; set; }
        //    public Dictionary<string, string> InfoDictionary { get; set; }
        //    public string CustomSetting { get; set; }
        //    public bool UseUsernameAndPassword { get; set; }
        //    public bool FipsEnabled { get; set; }
        //    public bool IgnoreSSL { get; set; }
        //    public SSHCredentials SSHCredentialsForReset { get; set; }
        //    public SSHCredentials SSHCredentialsForHeartbeat { get; set; }
        //    public PowerShellScriptInfo PowershellScriptInfoforReset { get; set; }
        //    public PowerShellScriptInfo PowershellScriptInfoforHeartBeat { get; set; }
        //    public SSHPublicKeyDigest SSHPublicKeyDigest { get; set; }
        //}
    }
}
