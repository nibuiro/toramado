using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

/// <summary>
/// ユーティリティ
/// </summary>
class Utils
{
    /// <summary>
    /// HTTP GETメソッドのハンドリング
    /// </summary>
    /// <param name="uri">リクエストUri</param>
    /// <returns>HTTPレスポンスボディ</returns>
    public static async Task<string> GetHtmlAsync(Uri uri)
    {
        using (HttpClient client = new HttpClient())
        {
            // タイムアウトをセット
            client.Timeout = TimeSpan.FromSeconds(5.0);

            try
            {
                // GET
                return await client.GetStringAsync(uri);
            }
            catch (HttpRequestException e)
            {
                // 404エラーや、名前解決失敗など
                toramado.App._sub_view_model.Info = e.Message;
                // InnerExceptionも含めて、再帰的に例外メッセージを表示する
                Exception ex = e;
                while (ex != null)
                {
                    toramado.App._sub_view_model.Info = ex.Message;
                    ex = ex.InnerException;
                }
            }
            catch (TaskCanceledException e)
            {
                // タスクがキャンセルされたとき
                toramado.App._sub_view_model.Info = e.Message;
            }
            return null;
        }
    }
}

