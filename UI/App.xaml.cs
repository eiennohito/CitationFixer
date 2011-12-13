using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Lib;

namespace UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
      private void App_OnStartup(object sender, StartupEventArgs e) {
        var mf = new MainWindow();
        mf.Positions = SomeData();
        mf.Show();
      }

      private List<ReferencePosition> SomeData() {
        List<ReferencePosition> poss = new List<ReferencePosition>();

        poss.Add(new ReferencePosition { Item = new Reference(0, "1"), NewIndex = 0 });
        poss.Add(new ReferencePosition { Item = new Reference(1, "2"), NewIndex = 1 });

        return poss;
      }
    }
}
