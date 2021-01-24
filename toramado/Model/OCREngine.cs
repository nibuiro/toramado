using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;

using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using Windows.Graphics.Imaging;

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


/// <summary>
/// 文字認識関数コレクション
/// </summary>
class OCR
{
    /// <summary>
    /// WindowsのOCRエンジンによる文字認識及び, 後処理を実行
    /// </summary>
    /// <param name="softwareBitmap">ビットマップ</param>
    /// <param name="languageCode">言語コード</param>
    /// <returns>テキスト</returns>
    public static async Task<string> UwpOcrEngineWithPostProcessing(SoftwareBitmap softwareBitmap, UwpLanguage languageCode)
    {
        string resultText = string.Empty;
        var ocrEngine = UwpOcrEngine.TryCreateFromLanguage(languageCode);

        UwpOcrResult ocrResult = 
            await ocrEngine.RecognizeAsync(softwareBitmap);

        foreach (var ocrLine in ocrResult.Lines)
            resultText += (ocrLine.Text + " ");

        return resultText.Replace("- ", "");
    }
}

