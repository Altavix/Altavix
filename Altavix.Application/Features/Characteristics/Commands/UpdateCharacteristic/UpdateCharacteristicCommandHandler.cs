using Altavix.Domain.Repositories;
using MediatR;

namespace Altavix.Application.Features.Characteristics.Commands.UpdateCharacteristic;

public class UpdateCharacteristicCommandHandler : IRequestHandler<UpdateCharacteristicCommand, bool>
{
    private readonly ICharacteristicRepository _characteristicRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCharacteristicCommandHandler(ICharacteristicRepository characteristicRepository, IUnitOfWork unitOfWork)
    {
        _characteristicRepository = characteristicRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateCharacteristicCommand request, CancellationToken cancellationToken)
    {
        var characteristic = await _characteristicRepository.GetByIdAsync(request.Id, cancellationToken);
        if (characteristic == null) return false;

        characteristic.Name = request.Name;
        characteristic.Enabled = request.Enabled;

        _characteristicRepository.Update(characteristic);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
