using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace toramado
{
    public class CaptureCommand : ICommand
    {
        #region Fields

        private Action<object> execute;
        private Predicate<object> canExecute;

        #endregion

        private MainViewModel vm;

        public CaptureCommand(MainViewModel viewmodel)
        {
            vm = viewmodel;
            Text = "Capture !!!";

            Gesture = new KeyGesture(Key.C, ModifierKeys.Shift);
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
        
        public void Execute(object parameter)
        {
            this.execute(/* Capture() */);
        }
        ///### For Hotkey Identification #####################
        public KeyGesture Gesture { get; set; }
        public string GestureText
        {
            get { return Gesture.GetDisplayStringForCulture(CultureInfo.CurrentUICulture); }
        }

        public string Text { get; set; }

        ///###################################################
    }
}
