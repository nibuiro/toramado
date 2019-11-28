using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Net.Http;


class Translation
{
    static async Task<string> GetHtmlAsync(Uri uri)
    {
        using (HttpClient client = new HttpClient())
        {
            // タイムアウトをセット（オプション）
            client.Timeout = TimeSpan.FromSeconds(10.0);

            try
            {
                // Webページを取得するのは、事実上この1行だけ
                return await client.GetStringAsync(uri);
            }
            catch (HttpRequestException e)
            {
                // 404エラーや、名前解決失敗など
                Console.WriteLine("\n例外発生!");
                // InnerExceptionも含めて、再帰的に例外メッセージを表示する
                Exception ex = e;
                while (ex != null)
                {
                    Console.WriteLine("例外メッセージ: {0} ", ex.Message);
                    ex = ex.InnerException;
                }
            }
            catch (TaskCanceledException e)
            {
                // タスクがキャンセルされたとき（一般的にタイムアウト）
                Console.WriteLine("\nタイムアウト!");
                Console.WriteLine("例外メッセージ: {0} ", e.Message);
            }
            return null;
        }
    }

    public static async Task<string> OnlineGoogleTranslation(string translation_tgt_txt = "", string translation_src_lang = "en", string translation_tgt_lang = "ja")
    {    
        string translation_query = $"https://script.google.com/macros/s/AKfycbx2abM1bYS-QkJJTwvYayYqUA5ua71SDY6gCCjO3csYT7h7uZZO/exec?text={translation_tgt_txt}&source={translation_src_lang}&target={translation_tgt_lang}";
        string html = await GetHtmlAsync(new Uri(translation_query));

        return html;
    }
}