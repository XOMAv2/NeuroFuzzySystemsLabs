using System;
using System.Collections.Generic;
using System.Text;

namespace Neuro.Helpers
{
    public static class IListExtension
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            Random rand = new Random();
            int n = list.Count;

            while (n > 1)
            {
                int k = rand.Next(n);
                n--;

                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
