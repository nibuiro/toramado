using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace toramado
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool MaximizeWindow = false, recWindowState = false;
        private double nonMaxHeight, nonMaxHidth, nonMaxTop, nonMaxLeft;
        SubWindow win = new SubWindow();

        public MainWindow()
        {

            App._main_view_model = new MainViewModel();
            this.DataContext = App._main_view_model;

            InitializeComponent();

            this.Left = Properties.Settings.Default.MainLeftRec;
            this.Top = Properties.Settings.Default.MainTopRec;
            this.Width = Properties.Settings.Default.MainWidthRec;
            this.Height = Properties.Settings.Default.MainHeightRec;

            this.Loaded += MainWindow_Loaded;
            this.Topmost = false;
        }
        
        private void MainWindow_Loaded(object sender, RoutedEventArgs ea)
        {
            win.Owner = this;
            win.Show();
        } // END SubWindow_Loaded

        private void Exit(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.MainLeftRec = (int)this.Left;
            Properties.Settings.Default.MainTopRec = (int)this.Top;
            Properties.Settings.Default.MainWidthRec = (int)this.Width;
            Properties.Settings.Default.MainHeightRec = (int)this.Height;
            Properties.Settings.Default.Save();
            Close();
        }
        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CommandBinding_Executed_1(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }
        private void CommandBinding_Executed_3(object sender, ExecutedRoutedEventArgs e)
        {
            if (MaximizeWindow == false)
            {
                nonMaxHeight = this.Height;
                nonMaxHidth = this.Width;
                nonMaxTop = this.Top;
                nonMaxLeft = this.Left;

                this.Top = 0;
                this.Left = 0;
                this.Height = SystemParameters.FullPrimaryScreenHeight + 23;
                this.Width = SystemParameters.FullPrimaryScreenWidth;
            }
            else
            {
                this.Height = nonMaxHeight + 23;
                this.Width = nonMaxHidth;
                Top = nonMaxTop; Left = nonMaxLeft;
            }
            MaximizeWindow = !MaximizeWindow;
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }
    }
}
