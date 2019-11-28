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
    public sealed class SubViewModel : INotifyPropertyChanged
    {
        public delegate void GetWindowPositionDelegate(out int left, out int top, out int height, out int width);
        public GetWindowPositionDelegate GetWindowPosition;

        public SubViewModel()
        {
            ScreenOCRCommand = new ReturnableAsyncCommand(
                async () => 
                {
                    if (null == _selected_item) {
                        Info = "検出言語を選択してください";

                        return;
                    }

                    GetWindowPosition(out int left, out int top, out int height, out int width);

                    App._main_view_model.OCRedText = await OCR.ScreenOCR(
                        (int)(left), 
                        (int)(top), 
                        (int)(height - 20), //メニューバー分を引いている
                        (int)(width), 
                        SelectedItem as UwpLanguage
                    );
                }
            );

            QuickTranslationCommand = new ReturnableAsyncCommand(
                async () =>
                {
                    if (null == _selected_item) {
                        Info = "検出言語を選択してください";

                        return;
                    }

                    GetWindowPosition(out int left, out int top, out int height, out int width);

                    App._main_view_model.OCRedText = await OCR.ScreenOCR(
                        (int)(left),
                        (int)(top),
                        (int)(height - 20),
                        (int)(width),
                        SelectedItem as UwpLanguage
                    );

                    App._main_view_model.TranslatedText = await Translation.OnlineGoogleTranslation(App._main_view_model.OCRedText);
                }
            );

        }
        

        #region ScreenOCRCommandSet
        public IAsyncCommand ScreenOCRCommand { get; private set; }
        public string ScreenOCR_Text { get; set; } = "ScreenOCR";
        public string ScreenOCR_GestureText
        {
            get { return ScreenOCR_Gesture.GetDisplayStringForCulture(CultureInfo.CurrentUICulture); }
        }
        public KeyGesture ScreenOCR_Gesture { get; set; } = new KeyGesture(Key.K, ModifierKeys.Control);
        #endregion


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

        private static object _selected_item;
        public object SelectedItem
        {
            get { return _selected_item; }
            set
            {
                _selected_item = value;
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
