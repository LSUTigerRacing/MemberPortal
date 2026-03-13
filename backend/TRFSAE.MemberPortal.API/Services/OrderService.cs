using TRFSAE.MemberPortal.API.DTOs;
using TRFSAE.MemberPortal.API.Interfaces;
using TRFSAE.MemberPortal.API.Models;
using System.Text.Json;
using Supabase;

namespace TRFSAE.MemberPortal.API.Services;

public class OrderService : IOrderService
{
    private readonly Client _supabaseClient;

    public OrderService(Client supabaseClient)
    {
        _supabaseClient = supabaseClient;
    }

    public async Task<IEnumerable<OrderSummaryDto>> GetAllOrdersAsync()
    {
        var response = await _supabaseClient
            .From<OrderModel>()
            .Order(p => p.CreatedAt, Supabase.Postgrest.Constants.Ordering.Ascending)
            .Select("id,requesterId,name,subsystem,status,deadline")
            .Get();

        var orderSummaries = response.Models.Select(p => new OrderSummaryDto
        {
            Id = p.Id,
            RequesterId = p.RequesterId,
            Name = p.Name,
            Subsystem = p.Subsystem,
            Status = p.Status,
            Deadline = p.Deadline,
        });

        return orderSummaries;
    }

    public async Task<OrderDetailDto> GetOrderAsync(Guid id)
    {
        var response = await _supabaseClient
            .From<OrderModel>()
            .Select("*")
            .Where(p => p.Id == id)
            .Single();

        if (response == null)
        {
            Console.WriteLine($"Order not found.");
            return null!;
        }

        var orderDetail = new OrderDetailDto
        {
            Id = response.Id,
            RequesterId = response.RequesterId,
            Name = response.Name,
            Subsystem = response.Subsystem,
            Status = response.Status,
            Deadline = response.Deadline,
            Notes = response.Notes,
            CreatedAt = response.CreatedAt,
            UpdatedAt = response.UpdatedAt
        };

        return orderDetail;
    }

    public async Task<bool> CreateOrderAsync(OrderCreateDto dto)
    {
        var newOrder = new OrderModel
        {
            Id = Guid.NewGuid(),
            RequesterId = new Guid("8cff6494-d336-4d38-947e-ff299ae3d204"), // temp until JWT is setup
            Name = dto.Name,
            Subsystem = dto.Subsystem,
            Status = dto.Status,
            Deadline = dto.Deadline,
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        try
        {
            var response = await _supabaseClient
                .From<OrderModel>()
                .Insert(newOrder);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating Order: {ex.Message}");
            return false;
        }

        return true;
    }

    public async Task<bool> UpdateOrderAsync(Guid id, OrderUpdateDto updateDto)
    {
        try
        {
            var model = await _supabaseClient.From<OrderModel>()
                .Where(p => p.Id == id)
                .Single();

            if (model == null)
            {
                Console.WriteLine("Project not found");
                return false;
            }

            if (!string.IsNullOrEmpty(updateDto.Name))
            {
                model.Name = updateDto.Name;
            }
            if (!string.IsNullOrEmpty(updateDto.Notes))
            {
                model.Notes = updateDto.Notes;
            }
            if (updateDto.Subsystem != null)
            {
                model.Subsystem = updateDto.Subsystem.Value;
            }
            if (updateDto.Status != null)
            {
                model.Status = updateDto.Status.Value;
            }
            model.UpdatedAt = DateTime.UtcNow;

            var response = await _supabaseClient
                .From<OrderModel>()
                .Update(model);

            return response.Models.Count > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to update order: {ex.Message}");
            return false;
        }
    }   

    public Task<bool> CreateOrderReviewAsync(OrderCreateDto dto)
    {
        throw new NotImplementedException();
    }


}
