using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Net.Http;
using System.Threading;
using System.Globalization;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Windows.Input;



namespace toramado
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            TranslationCommand = new ReturnableAsyncCommand(
                async () => {
                    TranslatedText = await Translation.OnlineGoogleTranslation(OCRedText);
                }
            );
        }

        #region Propaty - OCRed Text
        private string _ocred_text = "Captured Text will display here."; //クラス内インターフェイス
        public string OCRedText //外部（インスタンス）インターフェイス
        {
            get { return _ocred_text; }
            set
            {
                if (_ocred_text == value) return;
                _ocred_text = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Propaty - Translated Text
        private string _translated_text = "翻訳された文章はここに表示されます。"; //クラス内インターフェイス
        public string TranslatedText //外部（インスタンス）インターフェイス
        {
            get { return _translated_text; }
            set
            {
                if (_translated_text == value) return;
                _translated_text = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region TranslationCommandSet
        public IAsyncCommand TranslationCommand { get; private set; }
        public string TranslationText { get; set; } = "Translate";
        public string TranslationGestureText
        {
            get { return TranslationGesture.GetDisplayStringForCulture(CultureInfo.CurrentUICulture); }
        }
        public KeyGesture TranslationGesture { get; set; } = new KeyGesture(Key.T, ModifierKeys.Control);
        #endregion
        

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
