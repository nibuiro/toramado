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
using System.Windows.Navigation;
using System.Windows.Threading;


using UwpOcrEngine = Windows.Media.Ocr.OcrEngine;
using UwpOcrResult = Windows.Media.Ocr.OcrResult;
using UwpLanguage = Windows.Globalization.Language;
using Imaging = System.Windows.Media.Imaging;

using UwpSoftwareBitmap = Windows.Graphics.Imaging.SoftwareBitmap;
using UwpBitmapDecoder = Windows.Graphics.Imaging.BitmapDecoder;
using UwpBitmapPixelFormat = Windows.Graphics.Imaging.BitmapPixelFormat;
using UwpBitmapAlphaMode = Windows.Graphics.Imaging.BitmapAlphaMode;
using UwpInMemoryRandomAccessStream = Windows.Storage.Streams.InMemoryRandomAccessStream;
using UwpDataWriter = Windows.Storage.Streams.DataWriter;
using System.Globalization;

namespace toramado
{
    /// <summary>
    /// SubWindow.xaml の相互作用ロジック
    /// </summary>
    /// 


    public partial class SubWindow : Window
    {
        public bool MaximizeWindow = false, recWindowState = false;
        private double nonMaxHeight, nonMaxHidth, nonMaxTop, nonMaxLeft;

        public SubWindow()
        {
            InitializeComponent();

            App._sub_view_model = new SubViewModel();
            App._sub_view_model.GetWindowPosition = window_info;

            DataContext = App._sub_view_model;

            this.Left = Properties.Settings.Default.SubLeftRec;
            this.Top = Properties.Settings.Default.SubTopRec;
            this.Width = Properties.Settings.Default.SubWidthRec;
            this.Height = Properties.Settings.Default.SubHeightRec;

            this.Loaded += SubWindow_Loaded;
            this.Topmost = true;
        }

        private void SubWindow_Loaded(object sender, RoutedEventArgs ea)
        {
        } // END SubWindow_Loaded


        public void window_info(out int left, out int top, out int height, out int width)
        {
            var position = this.PointToScreen(new Point(0.0d, 0.0d));

            left = (int)position.X;
            top = (int)position.Y;
            height = (int)ActualHeight;
            width = (int)ActualWidth;
        }
        
        private void LangComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.SubLeftRec = (int)this.Left;
            Properties.Settings.Default.SubTopRec = (int)this.Top;
            Properties.Settings.Default.SubWidthRec = (int)this.Width;
            Properties.Settings.Default.SubHeightRec = (int)this.Height;
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
            } else {
                this.Height = nonMaxHeight + 23;
                this.Width = nonMaxHidth;

                Top = nonMaxTop;
                Left = nonMaxLeft;
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
