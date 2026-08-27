using Altavix.Domain;
using Altavix.Domain.Repositories;
using MediatR;

namespace Altavix.Application.Features.Characteristics.Commands.CreateCharacteristic;

public class CreateCharacteristicCommandHandler : IRequestHandler<CreateCharacteristicCommand, Guid>
{
    private readonly ICharacteristicRepository _characteristicRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCharacteristicCommandHandler(ICharacteristicRepository characteristicRepository, IUnitOfWork unitOfWork)
    {
        _characteristicRepository = characteristicRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateCharacteristicCommand request, CancellationToken cancellationToken)
    {
        var characteristic = new CharacteristicEntity
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Enabled = request.Enabled
        };

        await _characteristicRepository.AddAsync(characteristic, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return characteristic.Id;
    }
}
