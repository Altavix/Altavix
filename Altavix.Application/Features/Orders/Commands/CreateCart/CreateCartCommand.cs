using MediatR;

namespace Altavix.Application.Features.Orders.Commands.CreateCart;

public class CreateCartCommand : IRequest<Guid>
{
    // ClientId is optional, used if the user is already authenticated
    public Guid? ClientId { get; set; }
}
