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


class OCR
{
    public static async Task<string> ScreenOCR(int left, int top, int height, int width, UwpLanguage selected_item, CancellationToken token = new CancellationToken())
    {
        string result_text = string.Empty;
        

        var ocrEngine = UwpOcrEngine.TryCreateFromLanguage(selected_item);
        UwpOcrResult ocrResult = 
            await ocrEngine.RecognizeAsync(
                await Capture.CaptureAsSoftwareBitmap(left, top, height, width)　//　ここのawaitの結果を外身が受け取っているかどうか。また、キャプチャ範囲が正確か否か。
            );

        foreach (var ocrLine in ocrResult.Lines)
            result_text += (ocrLine.Text + " ");


        return result_text.Replace("- ", "");
    }
}

