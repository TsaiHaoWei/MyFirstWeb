using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyFirstWeb.Controllers.UtilityFolder
{
    public  static class Sort
    {
        public static void RadixSort(int[] arr)
        {
            int i, j;
            int[] tmp = new int[arr.Length];
            for (int shift = 31; shift > -1; --shift)
            {
                j = 0;
                for (i = 0; i < arr.Length; ++i)
                {
                    bool move = (arr[i] << shift) >= 0;
                    if (shift == 0 ? !move : move)
                        arr[i - j] = arr[i];
                    else
                        tmp[j++] = arr[i];
                }
                Array.Copy(tmp, 0, arr, arr.Length - j, j);
            }
        }
        public static List<T> RadixSort<T>(List<T> list, string sortColumn) {
            return new List<T>();
        }
    }

}