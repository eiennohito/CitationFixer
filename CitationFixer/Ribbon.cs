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
      
      var text = doc.Application.Selection.Text;
      var listValue = doc.Application.Selection.Range.ListFormat.ListValue;
      var references = Reference.ParseReferences(text);

      var refPos = references.Select(r => new ReferencePosition {Item = r, NewIndex = r.Index});
      var f = new MainWindow();
      f.Topmost = true;
      f.Positions = refPos;
      f.Show();
      f.OnOk = FormClosed;
    }

    private void FormClosed(MainWindow form)
    {
      if (!form.Result) { return; }

      var positions = form.Positions.OrderBy(p => p.Index).ToList();
      Document doc = Globals.ThisAddIn.Application.ActiveDocument;

      Range litRange = doc.Application.Selection.Range;
      var lits = form.Positions.OrderBy(p => p.NewIndexValue).ToList();
      litRange.Text = lits.Select(p => p.Ref).Aggregate((s1, s2) => string.Format("{0}\r{1}", s1, s2));
      var template = doc.Application.ListGalleries[WdListGalleryType.wdNumberGallery].ListTemplates[1];
      template.ListLevels[1].StartAt = lits[0].NewIndexValue;
      litRange.ListFormat.ApplyListTemplate(template);

      var lb = Counter.SymbolIndices(doc.Range(), "[");
      var rb = Counter.SymbolIndices(doc.Range(), "]");
      var ranges = new Coupler().Couple(lb, rb).ToList();

      var places = ranges
        .Select(r => Tuple.Create(r, doc.Range(r.Begin + 1, r.End)))
        .Select(t => new CitationPlace(t.Item1, t.Item2.Text))
        .OrderByDescending(p => p.Loc.Begin).ToList();

      foreach (var p in places)
      {
        var range = doc.Range(p.Loc.Begin + 1, p.Loc.End);
        range.Text = p.Format(i => positions[i].NewIndexValue);
      }
    }
  }
}
