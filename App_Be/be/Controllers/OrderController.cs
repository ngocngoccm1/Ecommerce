using App.DTO.Order;
using App.Interface;
using App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace App.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> Order(List<int> ids)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString();
            var order = await _orderService.Order(userId, ids);
            return Ok(order);
        }
        [HttpGet("ALL")]
        public async Task<IActionResult> GetALl()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString();
            var orders = await _orderService.GetAll(userId);
            if (orders == null) return NotFound();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderById(id);
            if (order == null) return NotFound();
            return Ok(order);
        }
    }

}
