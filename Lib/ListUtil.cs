using System.Collections.Generic;
using System.Diagnostics;

namespace Lib {
  public static class ListUtil {
    public static void Swap<T>(this IList<T> list, int left, int right) {
      lock (list) {
        T item = list[left];
        list[left] = list[right];
        list[right] = item;
      }
    }
  }
}