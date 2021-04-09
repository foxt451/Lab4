using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LZWArchiver
{
    class ListComparer<T> : IEqualityComparer<List<T>>
    {
        public bool Equals(List<T> x, List<T> y)
        {
            return x.SequenceEqual(y);
        }

        public int GetHashCode([DisallowNull] List<T> obj)
        {
            
            int hash = obj.Count;
            foreach (T item in obj)
            {
                hash = hash * 16 + item.GetHashCode() * 2;
            }
            return hash;
        }
    }
}
