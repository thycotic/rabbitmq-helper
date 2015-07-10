using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Thycotic.DistributedEngine.Logic
{
    class StringSplitter
    {
        public string[] Split(string separator, string toBeSplit)
        {
            Contract.Requires<ArgumentNullException>(separator != null);
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(separator));
            Contract.Requires<ArgumentNullException>(toBeSplit != null);

            if (string.IsNullOrWhiteSpace(toBeSplit))
            {
                return new[] { "" };
            }

            Contract.Assume(separator != null);
            Contract.Assume(toBeSplit != null);

            var list = new List<string>();
            var index = 0;
            while (index < toBeSplit.Length)
            {
                int indexOf = toBeSplit.IndexOf(separator, index, StringComparison.Ordinal);
                if (indexOf == -1)
                {
                    list.Add(toBeSplit.Substring(index));
                    break;
                }
                if (index < indexOf)
                {
                    list.Add(toBeSplit.Substring(index, indexOf - index));
                }
                index = indexOf + separator.Length;
            }
            return list.ToArray();
        }
    }
}