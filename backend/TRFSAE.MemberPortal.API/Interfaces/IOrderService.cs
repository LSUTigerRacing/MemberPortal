using TRFSAE.MemberPortal.API.DTOs;

namespace TRFSAE.MemberPortal.API.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<OrderSummaryDto>> GetAllOrdersAsync();
    Task<OrderDetailDto> GetOrderAsync(Guid id);
    Task<bool> CreateOrderAsync(OrderCreateDto dto);
    Task<bool> UpdateOrderAsync(Guid id, OrderUpdateDto dto);
    Task<bool> CreateOrderReviewAsync(OrderCreateDto dto);
}
