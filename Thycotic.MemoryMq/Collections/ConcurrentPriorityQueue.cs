using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thycotic.MemoryMq.Collections
{
    /// <summary>
    /// Concurrent priority queue follows the interface of <see cref="ConcurrentQueue{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConcurrentPriorityQueue<T>
    {
        private readonly QueueNodePointer _head = new QueueNodePointer();
        private readonly QueueNodePointer _tail = new QueueNodePointer();

        private object _syncRoot = new object();

        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is empty; otherwise, <c>false</c>.
        /// </value>
        public bool IsEmpty
        {
            get
            {
                lock (_syncRoot)
                {
                    return _head.Node == null;
                }
            }
        }

        /// <summary>
        /// Enqueues the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Enqueue(T item)
        {
            lock (_syncRoot)
            {
                //adds to the tail
                //tail moves back

                if (_head.Node == null)
                {
                    var node = new QueueNode {Item = item, Next = null};
                    _head.Node = _tail.Node = node;

                }
                else
                {
                    var oldTailNode = _tail.Node;

                    var node = new QueueNode {Item = item, Next = null};
                    oldTailNode.Next = new QueueNodePointer(node);

                    _tail.Node = node;
                }
            }
        }

        /// <summary>
        /// Priorities the enqueue.
        /// </summary>
        /// <param name="item">The item.</param>
        public void PriorityEnqueue(T item)
        {
            lock (_syncRoot)
            {
                //resets the head
                //whatever the last head was new head has it as its next

                if (_head.Node == null)
                {
                    Enqueue(item);
                }
                else
                {
                    var node = new QueueNode {Item = item, Next = new QueueNodePointer(_head.Node)};
                    _head.Node = node;
                }
            }
        }

        /// <summary>
        /// Tries the dequeue.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public bool TryDequeue(out T result)
        {
            lock (_syncRoot)
            {
                //empty queue
                if (_head.Node == null)
                {
                    result = default(T);
                    return false;
                }

                //no empty queue, set result
                result = _head.Node.Item;

                //last node
                if (_tail.Node == _head.Node)
                {
                    _head.Node = _tail.Node = null;
                }
                else
                {
                    _head.Node = _head.Node.Next.Node;
                }
            }
            return true;
        }

        private class QueueNode
        {
            public T Item { get; set; }
            public QueueNodePointer Next { get; set; }
        }

        private class QueueNodePointer
        {
            public QueueNode Node { get; set; }

            public QueueNodePointer()
            {
            }

            public QueueNodePointer(QueueNode node)
            {
                Node = node;
            }

        }
    }
}
