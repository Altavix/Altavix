using Altavix.Application.Models;
using MediatR;

namespace Altavix.Application.Features.Orders.Commands.CreateCart;

public class CreateCartCommand : IRequest<ApiResponseDto<Guid>>
{
    // ClientId is optional, used if the user is already authenticated
    public Guid? ClientId { get; set; }
}
