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
using System.Windows.Navigation;

using System.Windows.Threading;

using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using Windows.Graphics.Imaging;


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


public class Capture
{
    public static async Task<SoftwareBitmap> CaptureAsSoftwareBitmap(int left, int top, int height, int width)
    {
        /**
        left = (int)Application.Current.MainWindow.Left;
        top = (int)Application.Current.MainWindow.Top;
        height = (int)Application.Current.MainWindow.Height;
        width = (int)Application.Current.MainWindow.Width;
        **/
        //Bitmapの作成
        Bitmap partial_capture = new Bitmap(width, height); /// - 3, - 57
        //Graphicsの作成
        Graphics g = Graphics.FromImage(partial_capture);
        //画面全体をコピーする
        g.CopyFromScreen(
            left,/// + 1
            top, /// + 27
            0, 0,
            partial_capture.Size
        );
        //解放
        g.Dispose();

        MemoryStream ms = new MemoryStream();
        partial_capture.Save(ms, ImageFormat.Bmp);

        SoftwareBitmap softwareBitmap;

        using (var randomAccessStream = new UwpInMemoryRandomAccessStream())
        {
            using (var outputStream = randomAccessStream.GetOutputStreamAt(0))
            using (var writer = new UwpDataWriter(outputStream))
            {
                writer.WriteBytes(ms.ToArray());
                await writer.StoreAsync();
                await outputStream.FlushAsync();
            }

            // IRandomAccessStreamをSoftwareBitmapに変換
            // （ここはUWP APIのデコーダーを使う）
            var decoder = await UwpBitmapDecoder.CreateAsync(randomAccessStream);
            softwareBitmap = await decoder.GetSoftwareBitmapAsync(UwpBitmapPixelFormat.Bgra8, UwpBitmapAlphaMode.Premultiplied);
        }

        return softwareBitmap;
        ///await RecognizeBitmapAsync(softwareBitmap);
    }
}
