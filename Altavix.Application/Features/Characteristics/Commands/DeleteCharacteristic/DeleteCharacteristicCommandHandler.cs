using Altavix.Domain.Repositories;
using MediatR;

namespace Altavix.Application.Features.Characteristics.Commands.DeleteCharacteristic;

public class DeleteCharacteristicCommandHandler : IRequestHandler<DeleteCharacteristicCommand, bool>
{
    private readonly ICharacteristicRepository _characteristicRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCharacteristicCommandHandler(ICharacteristicRepository characteristicRepository, IUnitOfWork unitOfWork)
    {
        _characteristicRepository = characteristicRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteCharacteristicCommand request, CancellationToken cancellationToken)
    {
        var characteristic = await _characteristicRepository.GetByIdAsync(request.Id, cancellationToken);
        if (characteristic == null) return false;

        _characteristicRepository.Remove(characteristic);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
