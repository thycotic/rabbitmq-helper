using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Thycotic.Logging;
using Thycotic.MemoryMq.Subsystem.Persistance;

namespace Thycotic.MemoryMq.Subsystem
{
    /// <summary>
    /// Exchange that binds topics to queues
    /// </summary>
    public class ExchangeDictionary : IExchangeDictionary
    {
        private readonly ConcurrentDictionary<RoutingSlip, MessageQueue> _data = new ConcurrentDictionary<RoutingSlip, MessageQueue>();

        private readonly ILogWriter _log = Log.Get(typeof(ExchangeDictionary));

        /// <summary>
        /// Gets the mailboxes in the exchange
        /// </summary>
        /// <value>
        /// The mailboxes.
        /// </value>
        public IList<Mailbox> Mailboxes
        {
            get { return _data.Select(kvp => new Mailbox(kvp.Key, kvp.Value)).ToList(); }
        }

        /// <summary>
        /// Gets a value indicating whether this exchange is empty. This empty mailboxes.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is empty; otherwise, <c>false</c>.
        /// </value>
        public bool IsEmpty
        {
            get { return _data.Values.All(q => q.IsEmpty); }
        }

        /// <summary>
        /// Publishes the body to the specified routing slip.
        /// </summary>
        /// <param name="routingSlip">The routing slip.</param>
        /// <param name="body">The body.</param>
        public void Publish(RoutingSlip routingSlip, MemoryMqDeliveryEventArgs body)
        {
            _log.Debug(string.Format("Publishing message to {0}", routingSlip));

            _data.GetOrAdd(routingSlip, s => new MessageQueue());

            _data[routingSlip].Enqueue(body);
        }

        /// <summary>
        /// Acknowledges the specified delivery tag.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="routingSlip">The routing slip.</param>
        /// <exception cref="System.ApplicationException">Delivery tag was not found</exception>
        public void Acknowledge(ulong deliveryTag, RoutingSlip routingSlip)
        {
            _data[routingSlip].Acknowledge(deliveryTag);
        }

        /// <summary>
        /// Negativelies the acknowledge.
        /// </summary>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="routingSlip">The routing slip.</param>
        /// <exception cref="System.ApplicationException">Delivery tag was not found</exception>
        public void NegativelyAcknowledge(ulong deliveryTag, RoutingSlip routingSlip)
        {
            _data[routingSlip].NegativelyAcknoledge(deliveryTag);
        }

        private static string GetPersistPath()
        {
            //TODO: Figure out what better path to have this live under
            return Path.Combine(Directory.GetCurrentDirectory(), "store.json");
        }

        /// <summary>
        /// Restores the persisted messages from disk to memory.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void RestorePersistedMessages()
        {
            try
            {
                var path = GetPersistPath();
                if (!File.Exists(path))
                {
                    //nothing to restore
                    return;
                }

                _log.Info("Restoring messages from disk...");

                CombinedSnapshot snapshot;

                using (var fs = File.Open(path, FileMode.Open))
                using (var sw = new StreamReader(fs))
                using (var jw = new JsonTextReader(sw))
                {
                    var serializer = new JsonSerializer();
                    snapshot = serializer.Deserialize<CombinedSnapshot>(jw);
                }

                if (snapshot != null && snapshot.Exchanges.Length > 0)
                {
                    snapshot.Exchanges.ToList().ForEach(e => e.DeliveryEventArguments.ToList().ForEach(dea => Publish(e.RoutingSlip, dea)));
                }

                //remove the file
                File.Delete(path);
            }
            catch (Exception ex)
            {
                _log.Error("Failed to restore messages", ex);
            }

        }

        /// <summary>
        /// Persists the messages from memory to disk.
        /// </summary>
        public void PersistMessages()
        {
            try
            {
                var path = GetPersistPath();
                if (!File.Exists(path))
                {
                    //delete any previous snapshots
                    File.Delete(path);
                }

                if (IsEmpty)
                {
                    //nothing to persist
                    return;
                }

                _log.Info("There are messages in the exchange. Persisting to disk...");


                var snapshot = GenerateSnapshot();

                using (var fs = File.Open(path, FileMode.Create))
                using (var sw = new StreamWriter(fs))
                using (var jw = new JsonTextWriter(sw))
                {
                    jw.Formatting = Formatting.None;

                    var serializer = new JsonSerializer();
                    serializer.Serialize(jw, snapshot);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Failed to persist messages", ex);
            }
        }

        private CombinedSnapshot GenerateSnapshot()
        {
            var exchangeSnapshotList = new List<ExchangeSnapshot>();

            _data.ToList().ForEach(kvp =>
            {
                var routingSlipSnapshotList = new List<MemoryMqDeliveryEventArgs>();

                var routingSlip = kvp.Key;

                var queue = kvp.Value;

                MemoryMqDeliveryEventArgs deliveryArgs;
                while (queue.TryDequeue(out deliveryArgs))
                {
                    routingSlipSnapshotList.Add(deliveryArgs);
                }

                exchangeSnapshotList.Add(new ExchangeSnapshot
                {
                    RoutingSlip = routingSlip,
                    DeliveryEventArguments = routingSlipSnapshotList.ToArray()
                });

            });

            return new CombinedSnapshot
            {
                Exchanges = exchangeSnapshotList.ToArray()
            };
        }
    }

}