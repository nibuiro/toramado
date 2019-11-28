using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace toramado
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static MainViewModel _main_view_model;
        public static SubViewModel _sub_view_model;

        protected override void OnStartup(StartupEventArgs e)
        {
            //App._main_view_model = new MainViewModel();
            //App._sub_view_model = new SubViewModel();
        }
    }
}
