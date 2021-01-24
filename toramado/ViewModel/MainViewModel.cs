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
    /// <summary>
    /// エントリ・ポイント・クラス（翻訳ウィンドウについてのロジック）
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// メイン・エントリ・ポイント
        /// </summary>
        public MainViewModel()
        {
            TranslationCommand = new ReturnableAsyncCommand(
                async () => {
                    translatedText = await Translation.OnlineGoogleTranslation(recognizedText);
                }
            );
        }

        #region Propaty - OCRed Text
        private string _recognized_text = "Captured Text will display here.\n（日本語の文字認識エンジンにおいても英字の認識は可能ですが要求を満たせない恐れがあります.）"; 

        /// <summary>
        /// 認識されたテキストの保持／表示プロパティ
        /// </summary>
        public string recognizedText
        {
            get { return _recognized_text; }
            set
            {
                if (_recognized_text == value) return;
                _recognized_text = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Propaty - Translated Text
        private string _translated_text = "翻訳された文章はここに表示されます。";

        /// <summary>
        /// 翻訳されたテキストの保持／表示プロパティ
        /// </summary>
        public string translatedText 
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
