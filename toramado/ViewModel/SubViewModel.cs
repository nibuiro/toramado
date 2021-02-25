using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using Windows.Graphics.Imaging;

using System.Net.Http;
using System.Threading;

using System.Runtime.CompilerServices;


// UWP の OCR
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


namespace toramado

{
    /// <summary>
    ///  エントリ・ポイント・クラス（キャプチャウィンドウについてのロジック）
    /// </summary>
    public sealed class SubViewModel : INotifyPropertyChanged
    {
        public delegate void GetWindowPositionDelegate(out int left, out int top, out int height, out int width);
        public GetWindowPositionDelegate GetWindowPosition;
        private SoftwareBitmap capturedImage;
        private OCR ocrCollection = new OCR();
        private DispatcherTimer _timer;

        /// <summary>
        /// サブ（メイン2）・エントリ・ポイント
        /// </summary>
        public SubViewModel()
        {
            SetupTimer();


            //文字認識及び翻訳を実行
            QuickTranslationCommand = new ReturnableAsyncCommand(
                async () =>
                {
                    if (null == _selectedItem) {
                        Info = "検出言語を選択してください";

                        return;
                    }

                    GetWindowPosition(out int left, out int top, out int height, out int width);

                    capturedImage = await Capture.CaptureAsSoftwareBitmap(
                        (int)(left),
                        (int)(top),
                        (int)(height - 20), //メニューバー分を引いている
                        (int)(width)
                    );

                    App._main_view_model.recognizedText = await ocrCollection.UwpOcrEngineWithPostProcessing(capturedImage, SelectedItem as UwpLanguage);

                    App._main_view_model.translatedText = await Translation.OnlineGoogleTranslation(App._main_view_model.recognizedText, SelectedItem.LanguageTag);
                }
            );

        }

        /// <summary>
        /// イベント用文字認識メソッド
        /// </summary>
        private async void PreprocessingAndScreenOCR(object sender, EventArgs e)
        {
            if (null == _selectedItem)
            {
                Info = "検出言語を選択してください";
            } 
            else 
            {

                GetWindowPosition(out int left, out int top, out int height, out int width);

                capturedImage = await Capture.CaptureAsSoftwareBitmap(
                    (int)(left),
                    (int)(top),
                    (int)(height - 20), //メニューバー分を引いている
                    (int)(width)
                );

                App._main_view_model.recognizedText = await ocrCollection.UwpOcrEngineWithPostProcessing(capturedImage, SelectedItem as UwpLanguage);

            }
        }


        /// <summary>
        /// 定期実行処理を登録
        /// </summary>
        private void SetupTimer()
        {
            // タイマのインスタンスを生成
            _timer = new DispatcherTimer(); // 優先度はDispatcherPriority.Background
                                            // インターバルを設定
            _timer.Interval = new TimeSpan(0, 0, 1);
            // タイマメソッドを設定
            _timer.Tick += new EventHandler(PreprocessingAndScreenOCR);
            // タイマを開始
            _timer.Start();
        }

        /// <summary>
        /// 定期実行処理の解除
        /// </summary>
        private void StopTimer()
        {
            _timer.Stop();
        }


        /// <summary>
        /// 定期実行処理の解除をウィンドウ不在によるエラー回避のため行う
        /// </summary>
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            StopTimer();
        }




        #region QuickTranslationCommandSet
        public IAsyncCommand QuickTranslationCommand { get; private set; }
        public string QuickTranslationText { get; set; } = "QuickTranslation";
        public string QuickTranslationGestureText
        {
            get { return QuickTranslationGesture.GetDisplayStringForCulture(CultureInfo.CurrentUICulture); }
        }
        public KeyGesture QuickTranslationGesture { get; set; } = new KeyGesture(Key.Q, ModifierKeys.Control);
        #endregion


        #region SetLanguageList
        public IReadOnlyList<UwpLanguage> LangList
        {
            get { return UwpOcrEngine.AvailableRecognizerLanguages as IReadOnlyList<UwpLanguage>; }
        }

        public string DisplayName
        {
            get { return nameof(UwpLanguage.DisplayName); }
        }
        public string LangTag
        {
            get { return nameof(UwpLanguage.LanguageTag); }
        }
        public string UserProfileLang
        {
            set {}
            get { return UwpOcrEngine.TryCreateFromUserProfileLanguages().RecognizerLanguage.LanguageTag; }
        }

        private static UwpLanguage _selectedItem;
        public UwpLanguage SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                Info = _selectedItem.LanguageTag;

                OnPropertyChanged();
            }
        }

        #endregion


        #region Statusbar Info

        private static string _info;
        public string Info
        {
            get { return _info; }
            set
            {
                _info = value;

                OnPropertyChanged();
            }
        }

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
