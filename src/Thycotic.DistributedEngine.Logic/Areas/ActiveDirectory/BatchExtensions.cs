using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Thycotic.DistributedEngine.Logic.Areas.ActiveDirectory
{
    /// <summary>
    /// IEnumerable extensions
    /// </summary>
    public static class BatchExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="size"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> Batch<T>(
            this IEnumerable<T> source, int size)
        {
            Contract.Ensures(Contract.Result<IEnumerable<IEnumerable<T>>>() != null);

            T[] bucket = null;
            var count = 0;

            foreach (var item in source)
            {
                if (bucket == null)
                    bucket = new T[size];

                bucket[count++] = item;

                if (count != size)
                    continue;

                yield return bucket.Select(x => x);

                bucket = null;
                count = 0;
            }

            // Return the last bucket with all remaining elements
            if (bucket != null && count > 0)
                yield return bucket.Take(count);
        }
    }
}