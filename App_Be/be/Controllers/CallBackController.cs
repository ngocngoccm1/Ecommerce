using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ZaloPayExample.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CallbackController : ControllerBase
    {
        private readonly string _key2 = "trMrHtvjo6myautxDUiAcYsVtaeQ8nhf"; // Key2 của bạn

        [HttpPost]
        public IActionResult Post([FromBody] dynamic cbdata)
        {
            var result = new Dictionary<string, object>();

            try
            {
                // Lấy data và mac từ callback
                var dataStr = Convert.ToString(cbdata["data"]);
                var reqMac = Convert.ToString(cbdata["mac"]);

                // Tính toán lại MAC bằng HMACSHA256
                var mac = ComputeHMACSHA256(_key2, dataStr);

                Console.WriteLine("mac = {0}", mac);

                // Kiểm tra tính hợp lệ của callback
                if (!reqMac.Equals(mac))
                {
                    // Callback không hợp lệ
                    result["returncode"] = -1;
                    result["returnmessage"] = "mac not equal";
                }
                else
                {
                    // Thanh toán thành công - Cập nhật trạng thái đơn hàng
                    var dataJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(dataStr);
                    Console.WriteLine("update order's status = success where apptransid = {0}", dataJson["apptransid"]);

                    result["returncode"] = 1;
                    result["returnmessage"] = "success";
                }
            }
            catch (Exception ex)
            {
                // Gửi mã lỗi, ZaloPay sẽ thử callback lại tối đa 3 lần
                result["returncode"] = 0;
                result["returnmessage"] = ex.Message;
            }

            // Trả về kết quả cho ZaloPay server
            return Ok(result);
        }

        // Hàm tính toán HMAC SHA256
        private string ComputeHMACSHA256(string key, string data)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}
