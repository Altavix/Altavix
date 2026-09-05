using Altavix.Application.Interfaces;
using Altavix.Domain.Repositories;
using Dapper;
using MediatR;

namespace Altavix.Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbConnectionFactory _connectionFactory;

    public DeleteProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, IDbConnectionFactory connectionFactory)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _connectionFactory = connectionFactory;
    }

    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        // Спочатку перевіряємо через Dapper чи є цей товар в замовленнях
        using (var connection = _connectionFactory.CreateConnection())
        {
            var sql = "SELECT TOP 1 1 FROM tbOrderItems WHERE ProductId = @ProductId";
            var exists = await connection.ExecuteScalarAsync<bool>(sql, new { ProductId = request.Id });
            
            if (exists)
            {
                throw new Exception("Неможливо видалити товар, оскільки він міститься в одному або кількох замовленнях. Рекомендуємо змінити статус товару на неактивний.");
            }
        }

        var entity = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
            throw new Exception($"Product with id {request.Id} not found.");

        _productRepository.Remove(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
