using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Lib;
using Microsoft.Office.Interop.Word;
using Microsoft.Office.Tools.Ribbon;
using UI;

namespace CitationFixer {
  public partial class Ribbon {
    private void Ribbon_Load(object sender, RibbonUIEventArgs e) {

    }

    private void fixUpClick(object sender, RibbonControlEventArgs e) {
      //new FixupData(GatherData()) {OnOk = OnOkAction}.Show();
      Document doc = Globals.ThisAddIn.Application.ActiveDocument;
      Range range = doc.Range();

      GatherData();
    }

    private void GatherData() {
      Document doc = Globals.ThisAddIn.Application.ActiveDocument;

      var lb = Counter.SymbolIndices(doc.Range(), "[");
      var rb = Counter.SymbolIndices(doc.Range(), "]");
      var ranges = new Coupler().Couple(lb, rb).ToList();

      var places = ranges
        .Select(r => Tuple.Create(r, doc.Range(r.Begin + 1, r.End)))
        .Select(t => new CitationPlace(t.Item1, t.Item2.Text)).ToList();
      
      var text = doc.Application.Selection.Text;
      var references = Reference.ParseReferences(text);
      var listValue = doc.Application.Selection.Range.ListFormat.ListValue;

      var refPos = references.Select(r => new ReferencePosition {Item = r, NewIndex = r.Index});
      var f = new MainWindow();
      f.Positions = refPos;
      f.ShowDialog();
      var newPos = f.Positions.ToList();

    }

    private void OnOkAction(string obj) {

    }
  }
}
