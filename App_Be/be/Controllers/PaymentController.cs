using App.DTO.Payment;
using App.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ZaloPayController : ControllerBase
{
    private readonly ZaloPayService _zaloPayService;
    private readonly AppDbContext _context;

    public ZaloPayController()
    {
        _zaloPayService = new ZaloPayService();
    }

    [HttpPost("create-order")]
    [Authorize]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {

        var response = await _zaloPayService.CreateOrderAsync(request.OrderId, request.Description, request.Amount);
        return Ok(response); // Trả về JSON từ ZaloPay
    }


    [HttpPost]
    public async Task<IActionResult> Status(string apptransid, string OrderId)
    {
        var content = await _zaloPayService.StatusPayment(apptransid);
        ApiResponse response = JsonConvert.DeserializeObject<ApiResponse>(content);
        if (response.ReturnCode == 1)
        {
            Payment p = new Payment()
            {
                OrderId = int.Parse(OrderId),
                Status = "Đã thanh toán",
                Amount = response.Amount,
                PaymentMethod = "Chuyển khoản",
                PaymentDate = DateTime.Now
            };
            // await addPayment(p);

            Ok("Giao dịch thành công");
        }
        return Ok(content);
    }
    // public async Task addPayment(Payment p)
    // {
    //     await _context.Payments.AddAsync(p);
    //     await _context.SaveChangesAsync();
    // }

}

public class CreateOrderRequest
{
    public int OrderId { get; set; }
    public string Description { get; set; }
    public int Amount { get; set; }


}
public class ApiResponse
{
    public int Amount { get; set; }
    public int Pmcid { get; set; }
    public int DiscountAmount { get; set; }
    public long Apptime { get; set; }
    public string CcbankCode { get; set; }
    public string BankCode { get; set; }
    public int Bccode { get; set; }
    public int BctransStatus { get; set; }
    public int UserFeeAmount { get; set; }
    public bool IsFullFlow { get; set; }
    public long Zptransid { get; set; }
    public bool IsProcessing { get; set; }
    public string SuggestMessage { get; set; }
    public List<string> SuggestAction { get; set; }
    public string SubErrorCode { get; set; }
    public bool RequiredOtp { get; set; }
    public int ReturnCode { get; set; }
    public string ReturnMessage { get; set; }
}

