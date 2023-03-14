using System;
using System.Collections.Generic;

namespace Plugins
{
    public static class BinarySearch
    {
        public static int SearchIndexByKey<TItem>(List<TItem> items, int key, Func<int, int> getValueByIndex)
        {
            var index = 0;
            var left = 0;
            var right = items.Count - 1;

            while (left <= right)
            {
                var mid = left + (right - left) / 2;

                if (getValueByIndex(mid) == key)
                {
                    return mid;
                }

                if (index == -1 || Math.Abs(getValueByIndex(mid) - key) < Math.Abs(getValueByIndex(index) - key))
                {
                    index = mid;
                }

                if (getValueByIndex(mid) > key)
                {
                    right = mid - 1;
                }
                else
                {
                    left = mid + 1;
                }
            }

            return index;
        }
    }
}