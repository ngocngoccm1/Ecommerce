using App.DTO.Payment;
using App.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("process")]
        public IActionResult ProcessPayment([FromBody] PaymentDTO payment)
        {
            var result = _paymentService.ProcessPayment(payment);
            if (!result.IsSuccess) return BadRequest();
            return Ok(result);
        }
    }

}