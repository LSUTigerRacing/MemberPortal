using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TRFSAE.MemberPortal.API.DTOs;
using TRFSAE.MemberPortal.API.Interfaces;
using Supabase;

namespace TRFSAE.MemberPortal.API.Controllers;

[ApiController]
[Route("api/orders")]
[Authorize]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IPermissionService _permissionService;

    public OrderController(IOrderService orderService, IPermissionService permissionService)
    {
        _orderService = orderService;
        _permissionService = permissionService;
    }

    [HttpGet("list")]
    [Authorize(Policy = "MemberAA")] // track shipping request
    public async Task<IActionResult> GetAllOrdersAsync()
    {
        var result = await _orderService.GetAllOrdersAsync();
        return Ok(result);
    }

    [HttpGet("fetch")]
    [Authorize(Policy = "MemberAA")] // track shipping request
    public async Task<IActionResult> GetOrderAsync([FromQuery] Guid id)
    {
        var item = await _orderService.GetOrderAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost("create")]
    [Authorize(Policy = "SubsystemLeadAA")] // create shipping request
    public async Task<IActionResult> CreateOrderAsync(OrderCreateDto dto)
    {
        var requesterId = _permissionService.CurrentUserId ?? Guid.Empty;
        var created = await _orderService.CreateOrderAsync(dto, requesterId);
        return Ok(created);
    }

    [HttpPatch("update")]
    [Authorize(Policy = "SubsystemLeadAA")] // delete shipping request (status change)
    public async Task<IActionResult> UpdateOrderAsync([FromQuery] Guid id, OrderUpdateDto dto)
    {
        var updated = await _orderService.UpdateOrderAsync(id, dto);
        return Ok(updated);
    }

    [HttpPost("review")]
    // Approve/reject: Finance permission (SuperAdmin bypasses everything),
    // not a ladder rung, so it's checked inline rather than via policy.
    public async Task<IActionResult> CreateOrderReviewAsync([FromQuery] Guid id, OrderCreateDto dto)
    {
        if (!_permissionService.CanApproveOrder())
        {
            return Forbid();
        }

        var created = await _orderService.CreateOrderReviewAsync(dto);
        return Ok(created);
    }
}
