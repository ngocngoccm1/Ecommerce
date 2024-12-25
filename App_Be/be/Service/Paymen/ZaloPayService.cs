using System;
using System.Net.Http;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ZaloPay.Helper.Crypto;
using ZaloPay.Helper;
using Microsoft.AspNetCore.Authentication;
using App.Models;

public class ZaloPayService
{
    private readonly string AppId = "2554"; // Thay bằng AppID của bạn
    private readonly string Key1 = "sdngKKJmqEMzvh5QQcdD2A9XBSKUNaYn";     // Thay bằng Key1 của bạn
    private readonly string Endpoint = "https://sb-openapi.zalopay.vn/v2/create"; // Endpoint (dùng sandbox để test)
    private readonly string queryOrderUrl = "	https://sandbox.zalopay.com.vn/v001/tpe/getstatusbyapptransid"; // Endpoint (dùng sandbox để test)

    public async Task<Dictionary<string, object>> CreateOrderAsync(int orderId, string description, int amount)
    {
        string orderIdsrt = orderId.ToString();
        var appTransId = $"{DateTime.Now:yyMMdd}_{orderIdsrt}"; // Mã giao dịch theo ngày
        var embedData = new { redirecturl = "http://localhost:5065/" }; // Dữ liệu tuỳ chỉnh nếu cần
        var items = new[]{
                new { }
            };   // Danh sách sản phẩm (JSON string)

        var param = new Dictionary<string, string>
        {
            { "app_id", AppId },
            { "app_user", "demo" },
            { "app_time",  DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString() },
            { "amount", amount.ToString() },
            { "app_trans_id", DateTime.Now.ToString("yyMMdd") + "_" + appTransId }, // mã giao dich có định dạng yyMMdd_xxxx
            { "embed_data", JsonConvert.SerializeObject(embedData) },
            { "item", JsonConvert.SerializeObject(items) },
            { "description", description.ToString() },
            { "bank_code", "" }
            // { "callback_url", " https://f4e1-118-69-13-146.ngrok-free.app/Callback" }
        };

        var data = $"{AppId}|{param["app_trans_id"]}|{param["app_user"]}|{param["amount"]}|{param["app_time"]}|{param["embed_data"]}|{param["item"]}";

        // Tạo signature
        // string dataToHash = $"{AppId}|{appTransId}|{data.amount}|{data.app_user}|{data.app_time}|{data.embed_data}|{data.item}";
        param.Add("mac", HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, Key1, data));



        var result = await HttpHelper.PostFormAsync(Endpoint, param);
        result.Add("app_trans_id", param["app_trans_id"]);

        return result; // Trả về JSON response từ ZaloPay
    }


    public async Task<string> StatusPayment(string apptransid)
    {
        var param = new Dictionary<string, string>
        {
            { "appid", AppId },
            { "apptransid", apptransid }
        };


        var data = $"{AppId}|{param["apptransid"]}|{Key1}";
        param.Add("mac", CreateHmacSha256(data, Key1));


        var result = await PostFormAsync(queryOrderUrl, param);


        return result;
    }
    private async Task<string> PostFormAsync(string url, Dictionary<string, string> parameters)
    {
        using (var client = new HttpClient())
        {
            var content = new FormUrlEncodedContent(parameters);
            var response = await client.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }
    }

    public string CreateHmacSha256(string data, string key)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
        byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }
}
