using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Lib;

namespace UI {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {
    public MainWindow() {
      InitializeComponent();
      Shift = new ReferenceShift {Shift = 0};
      Result = false;
    }

    private ObservableCollection<ReferencePosition> positions = new ObservableCollection<ReferencePosition>();

    private void ListBox_Drop(object sender, DragEventArgs e) {
      var item = FindItem(listBox.InputHitTest(e.GetPosition(listBox)));
      var ind = item == null ? positions.Count - 1 : listBox.ItemContainerGenerator.IndexFromContainer(item);
      Debug.WriteLine("Index is {0}", ind);
      var rp = e.Data.GetData(typeof(ReferencePosition)) as ReferencePosition;
      if (rp != null && ind != rp.NewIndex) {
        var old = rp.NewIndex;
        MoveItem(old, ind);
      }
    }

    private void MoveItem(int from, int to)
    {
      var p = positions[from];
      positions.RemoveAt(from);
      positions.Insert(to, p);
      for (int i = 0; i < positions.Count; i++)
      {
        positions[i].NewIndex = i;
      }
    }

    private ListBoxItem FindItem(IInputElement elem) {
      UIElement ui = elem as UIElement;
      while (ui != null) {
        ListBoxItem lbi = ui as ListBoxItem;
        if (lbi != null) {
          return lbi;
        }
        ui = VisualTreeHelper.GetParent(ui) as UIElement;
      }
      return null;
    }

    private void ListBox_DragEnter(object sender, DragEventArgs e) {
    }

    private Point origin;

    private void ListBox_MouseMove(object sender, MouseEventArgs e) {
      if (listBox.SelectedIndex == -1) {
        return;
      }
      if (e.LeftButton == MouseButtonState.Pressed && DragOk(e)) {
        DragDrop.DoDragDrop(
          listBox,
          listBox.SelectedItem,
          DragDropEffects.Move
          );
      }
    }

    private bool DragOk(MouseEventArgs e) {
      var diff = e.GetPosition(listBox) - origin;
      return Math.Abs(diff.X) > 1.5 &&
             Math.Abs(diff.Y) > 1.5;
    }

    private void listBox_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
      origin = e.GetPosition(listBox);
    }

    private void listBox_DragOver(object sender, DragEventArgs e) {
      
    }

    private void Window_Loaded(object sender, RoutedEventArgs e) {
      listBox.DataContext = positions;
      this.DataContext = Shift;
    }

    public ReferenceShift Shift { get; set; }

    public bool Result { get; set; }

    public IEnumerable<ReferencePosition> Positions {
      get { return positions; }
      set {
        positions.Clear();
        foreach (var v in value) {
          positions.Add(v);
          v.Shift = Shift;
        }
      }
    }

    public Action<MainWindow> OnOk { get; set; }

    private void Ok_Click(object sender, RoutedEventArgs e)
    {
      Result = true;
      if (OnOk != null)
      {
        OnOk(this);
      }
      Close();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
      Result = false;
      Close();
    }
  }
}
