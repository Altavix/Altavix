using Altavix.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Altavix.Domain;

namespace Altavix.Application.Features.Orders.Commands.CheckoutOrder;

public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<UserEntity> _userManager;

    public CheckoutOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, UserManager<UserEntity> userManager)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<bool> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetOrderWithDetailsAsync(request.OrderId, cancellationToken);
        
        if (order == null)
            throw new KeyNotFoundException($"Cart/Order with ID {request.OrderId} was not found.");

        if (order.Ordered.HasValue)
            throw new InvalidOperationException("This cart has already been checked out as an order.");

        if (order.Items.Count == 0)
            throw new InvalidOperationException("Cannot checkout an empty cart.");

        // Update User Profile if authenticated
        if (order.ClientId.HasValue)
        {
            var user = await _userManager.FindByIdAsync(order.ClientId.Value.ToString());
            if (user != null)
            {
                // Always update Full Name
                var names = request.ClientName?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
                user.FirstName = names.ElementAtOrDefault(0) ?? "";
                user.LastName = names.ElementAtOrDefault(1) ?? "";
                user.MiddleName = names.ElementAtOrDefault(2) ?? "";
                
                // Update Phone only if empty
                if (string.IsNullOrEmpty(user.PhoneNumber) && !string.IsNullOrEmpty(request.ClientMobilePhone))
                {
                    user.PhoneNumber = request.ClientMobilePhone;
                }
                
                await _userManager.UpdateAsync(user);
            }
        }

        // Apply checkout info
        order.ClientName = request.ClientName;
        order.ClientMobilePhone = request.ClientMobilePhone;
        order.ClientEmail = request.ClientEmail;
        order.City = request.City;
        order.CityRef = request.CityRef;
        order.Address = request.Address;
        order.Comment = request.Comment;
        order.DeliveryMethodId = request.DeliveryMethodId;
        order.PaymentMethodId = request.PaymentMethodId;
        
        // Mark as Ordered
        order.Ordered = DateTime.UtcNow;

        _orderRepository.Update(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
