using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace Lib {
  public class Reference {
    public int Index { get; set; }
    public string Ref { get; set; }

    public Reference() {}
    public Reference(int idx, string reference) {
      Index = idx;
      Ref = Normalize(reference);
    }

    private static Regex re = new Regex(@"^\d+\.[ \t]*");

    public static string Normalize(string reference) {
      var match = re.Match(reference);
      if (match.Success) {
        return reference.Substring(match.Index + match.Length);
      }
      return reference;
    }

    public static List<Reference> ParseReferences(string text) {
      var en = text.Split('\r')
        .Select(s => s.Trim());
      return en
        .Where(s => s.Length > 1)
        .Select((s, i) => new Reference(i, s))
        .ToList();
    }
  }

  public class ReferenceShift : INotifyPropertyChanged
  {
    private int _shift;
    public int Shift
    {
      get { return _shift; }
      set
      {
        _shift = value;
        OnPropertyChanged("Shift");
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged(string name)
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null) handler(this, new PropertyChangedEventArgs(name));
    }
  }

  public class ReferencePosition : INotifyPropertyChanged {
    public Reference Item { get; set; }
    private int newIndex;
    public int NewIndex {
      get { return newIndex; }
      set { 
        newIndex = value;
        OnPropertyChanged("NewIndex");
        OnPropertyChanged("NewIndexValue");
      }
    }

    private ReferenceShift _shift;
    public ReferenceShift Shift
    {
      get { return _shift; }
      set
      {
        if (_shift != null)
        {
          _shift.PropertyChanged -= ShiftChanged;
        }
        _shift = value;
        OnPropertyChanged("NewIndexValue");
        _shift.PropertyChanged += ShiftChanged;
      }
    }

    private void ShiftChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName.Equals("Shift"))
      {
        OnPropertyChanged("NewIndexValue");
      }
    }

    public int NewIndexValue
    {
      get { 
        int val = newIndex;
        return _shift == null ? val : val + _shift.Shift;
      }
    }

    public int Index { get { return Item.Index; } }
    public string Ref { get { return Item.Ref; } }

    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged(string name) {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null) handler(this, new PropertyChangedEventArgs(name));
    }
  }
}