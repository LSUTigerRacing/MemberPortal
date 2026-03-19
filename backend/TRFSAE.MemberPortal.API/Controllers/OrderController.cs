using Microsoft.AspNetCore.Mvc;
using TRFSAE.MemberPortal.API.DTOs;
using TRFSAE.MemberPortal.API.Interfaces;
using Supabase;

namespace TRFSAE.MemberPortal.API.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetAllOrdersAsync()
    {
        var result = await _orderService.GetAllOrdersAsync();
        return Ok(result);
    }

    [HttpGet("fetch")]
    public async Task<IActionResult> GetOrderAsync([FromQuery] Guid id)
    {
        var item = await _orderService.GetOrderAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateOrderAsync(OrderCreateDto dto)
    {
        var created = await _orderService.CreateOrderAsync(dto);
        return Ok(created);
    }

    [HttpPatch("update")]
    public async Task<IActionResult> UpdateOrderAsync([FromQuery] Guid id, OrderUpdateDto dto)
    {
        var updated = await _orderService.UpdateOrderAsync(id, dto);
        return Ok(updated);
    }

    [HttpPost("review")]
    public async Task<IActionResult> CreateOrderReviewAsync([FromQuery] Guid id, OrderCreateDto dto)
    {
        var created = await _orderService.CreateOrderReviewAsync(dto);
        return Ok(created);
    }
}
