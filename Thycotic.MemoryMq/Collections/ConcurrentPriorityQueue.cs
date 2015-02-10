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

        private void ValidateQueueInvariant()
        {
            if ((_head != null) && (_tail == null))
            {
                throw new ApplicationException("Invalid queue");
            }

            if ((_head == null) && (_tail != null))
            {
                throw new ApplicationException("Invalid queue");
            }
        }

        /// <summary>
        /// Enqueues the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Enqueue(T item)
        {
            ValidateQueueInvariant();

            //adds to the tail
            //tail moves back

            if (_head.Node == null)
            {
                var node = new QueueNode { Item = item, Next = null };
                _head.Node = node;
                _tail.Node = node;

            }
            else
            {
                var oldTailNode = _tail.Node;

                var node = new QueueNode { Item = item, Next = null };
                oldTailNode.Next = new QueueNodePointer(node);

                _tail.Node = node;
            }
        }

        /// <summary>
        /// Priorities the enqueue.
        /// </summary>
        /// <param name="item">The item.</param>
        public void PriorityEnqueue(T item)
        {
            ValidateQueueInvariant();

            //resets the head
            //whatever the last head was new head has it as its next

            if (_head.Node == null)
            {
                var node = new QueueNode { Item = item, Next = null };
                _head.Node = node;
                _tail.Node = node;

            }
            else
            {
                var node = new QueueNode { Item = item, Next = _head };
                _head.Node = node;
            }
        }

        /// <summary>
        /// Tries the dequeue.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public bool TryDequeue(out T result)
        {
            ValidateQueueInvariant();

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
                if (_head.Node.Next != null)
                {
                    _head.Node = _head.Node.Next.Node;
                }
                else
                {
                    _head.Node = null;
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
