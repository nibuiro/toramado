using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Net.Http;


/// <summary>
/// 翻訳関数コレクション
/// </summary>
class Translation
{
    /// <summary>
    /// オンラインのGoogle翻訳APIによる翻訳
    /// </summary>
    /// <param name="sourceText">翻訳対象のテキスト</param>
    /// <param name="sourceLanguageCode">翻訳対象の言語コード</param>
    /// <param name="targetLanguageCode">翻訳先の言語コード</param>
    /// <remarks>
    /// エラー回避のため targetLanguageCode が常に"ja"であるとの前提の元, 前処理として sourceLanguageCode が"ja"の場合"en"に変更します.
    /// </remarks>
    /// <returns>翻訳されたテキスト</returns>
    public static async Task<string> OnlineGoogleTranslation(string sourceText = "", string sourceLanguageCode = "en", string targetLanguageCode = "ja")
    {
        if ("ja" == sourceLanguageCode)
        {
            sourceLanguageCode = "en";
        }
        string translationQuery = $"https://script.google.com/macros/s/AKfycbx2abM1bYS-QkJJTwvYayYqUA5ua71SDY6gCCjO3csYT7h7uZZO/exec?text={sourceText}&source={sourceLanguageCode}&target={targetLanguageCode}";
        string translatedText = await Utils.GetHtmlAsync(new Uri(translationQuery));

        return translatedText;
    }
}