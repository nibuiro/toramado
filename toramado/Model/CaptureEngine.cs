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


/// <summary>
/// キャプチャ関数コレクション
/// </summary>
public class Capture
{
    /// <summary>
    /// 指定範囲のキャプチャ画像をSoftwareBitmap型として得る
    /// </summary>
    /// <param name="left">キャプチャ範囲の左端端x座標</param>
    /// <param name="top">キャプチャ範囲の左端端y座標</param>
    /// <param name="height">キャプチャ範囲の高さ</param>
    /// <param name="width">キャプチャ範囲の幅</param>
    /// <returns>指定された範囲のキャプチャ画像</returns>
    public static async Task<SoftwareBitmap> CaptureAsSoftwareBitmap(int left, int top, int height, int width)
    {
        //ビットマップの保持領域を確保
        Bitmap partialCapture = new Bitmap(width, height);
        //描画インターフェイスの設定
        Graphics draw = Graphics.FromImage(partialCapture);

        //画面全体をコピーする
        draw.CopyFromScreen(
            left,
            top,
            0, 0,
            partialCapture.Size
        );

        //解放
        draw.Dispose();

        MemoryStream memStream = new MemoryStream();
        partialCapture.Save(memStream, ImageFormat.Bmp);

        SoftwareBitmap softwareBitmap;

        using (var randomAccessStream = new UwpInMemoryRandomAccessStream())
        {
            using (var outputStream = randomAccessStream.GetOutputStreamAt(0))
            using (var writer = new UwpDataWriter(outputStream))
            {
                writer.WriteBytes(memStream.ToArray());
                await writer.StoreAsync();
                await outputStream.FlushAsync();
            }

            // IRandomAccessStreamをSoftwareBitmapに変換
            // （ここはUWP APIのデコーダーを使う）
            var decoder = await UwpBitmapDecoder.CreateAsync(randomAccessStream);
            softwareBitmap = await decoder.GetSoftwareBitmapAsync(UwpBitmapPixelFormat.Bgra8, UwpBitmapAlphaMode.Premultiplied);
        }

        return softwareBitmap;
    }
}
