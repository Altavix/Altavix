using Altavix.Application.Interfaces;
using Altavix.Domain.Enums;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Altavix.Application.Features.Orders.Commands.UpdateOrderStatus;

public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, bool>
{
    private readonly IAltavixDbContext _context;

    public UpdateOrderStatusCommandHandler(IAltavixDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders.FindAsync(new object[] { request.OrderId }, cancellationToken);

        if (order == null)
        {
            return false; // Order not found
        }

        order.ChangeStatus(request.NewStatus);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
