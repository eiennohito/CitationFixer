using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Lib;
using Microsoft.Office.Interop.Word;

namespace CitationFixer
{
    public class Counter
    {
        public static List<int> SymbolIndices(Range rng, string what)
        {
            rng.Find.ClearFormatting();
            rng.Find.Forward = true;
            rng.Find.Text = what;
            var found = new List<int>();

            rng.Find.Execute();

            Debug.WriteLine("Searching for {0}", new object[] { what });

            while (rng.Find.Found)
            {
                found.Add(rng.Start);
                Debug.WriteLine("found: start {0}, end {1}", rng.Start, rng.End);
                rng.Find.Execute();
            }
            return found;
        } 
    }
}
