using System.Collections.Generic;

namespace Thycotic.MemoryMq.Collections
{
    public class ConcurrentHashSet<T>
    {
        private readonly HashSet<T> _data = new HashSet<T>();

        public bool Add(T item)
        {
            lock (_data)
            {
                return _data.Add(item);
            }
        }

        public bool Remove(T item)
        {
            lock (_data)
            {
                return _data.Remove(item);
            }
        }
    }
}