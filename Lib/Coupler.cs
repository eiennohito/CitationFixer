using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lib {

  public class RefRange {
    public int Begin { get; set; }
    public int End { get; set; }
  }

  public class Coupler {
    public IEnumerable<RefRange> Couple(List<int> begs, List<int> ends ) {
      int begi = 0, endi = 0;
      while (true) {
        if (begi >= begs.Count || endi >= ends.Count) {
          break;
        }
        int b = begs[begi], e = ends[endi];
        if (b >= e) {
          ++endi;
          continue;
        }
        if (e - b < 10) {
          yield return new RefRange {Begin = b, End = e};
          ++begi;
          ++endi;
          continue;
        }
        ++begi;
      }
    }
  }
}
