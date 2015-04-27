using System.Collections.Generic;

namespace Thycotic.DistributedEngine.Logic
{
    class StringSplitter
    {
        public string[] Split(string separator, string toBeSplit)
        {
            if (separator == "" || toBeSplit == "")
            {
                return new[] { "" };
            }
            var list = new List<string>();
            int index = 0;
            while (index < toBeSplit.Length)
            {
                int indexOf = toBeSplit.IndexOf(separator, index);
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