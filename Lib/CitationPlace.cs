using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lib {
  public class CitationPlace {
    public RefRange Loc { get; set; }
    public List<CitationIndex> Indices { get; set; }

    public CitationPlace() {
      Indices = new List<CitationIndex>();
    }

    public CitationPlace(RefRange rng, string text) {
      Loc = rng;
      Indices = ParseIndices(text);
    }

    public static List<CitationIndex> ParseIndices(string text) {
      return text.Split(',')
          .Select(s => s.Trim())
          .Where(s => s.Length > 0)
          .Select(int.Parse)
          .Select(s => new CitationIndex { Index = s })
          .ToList();
    }

    public string Format(Func<int, int> map)
    {
      return Indices
        .Select(i => map(i.Index))
        .Aggregate(new StringBuilder(), (b, i) => b.AppendFormat("{0}, ", i), b => b.ToString(0, b.Length - 2));
    }
  }

  public class CitationIndex {
    public int Index { get; set; }
  }
}