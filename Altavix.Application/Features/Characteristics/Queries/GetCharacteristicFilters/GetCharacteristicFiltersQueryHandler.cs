using Altavix.Application.Interfaces;
using Dapper;
using MediatR;

namespace Altavix.Application.Features.Characteristics.Queries.GetCharacteristicFilters;

public class GetCharacteristicFiltersQueryHandler : BaseQueryHandler, IRequestHandler<GetCharacteristicFiltersQuery, List<CharacteristicFilterDto>>
{
    public GetCharacteristicFiltersQueryHandler(IDbConnectionFactory connectionProvider) : base(connectionProvider) { }

    public async Task<List<CharacteristicFilterDto>> Handle(GetCharacteristicFiltersQuery request, CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT 
                c.Id, 
                c.Name, 
                pc.Value
            FROM tbCharacteristics c
            INNER JOIN tbProductCharacteristics pc ON c.Id = pc.CharacteristicId
            WHERE c.Enabled = 1
            GROUP BY c.Id, c.Name, pc.Value
            ORDER BY c.Name ASC, pc.Value ASC;
        ";

        var filtersDict = new Dictionary<Guid, CharacteristicFilterDto>();

        var rows = await QueryAsync<dynamic>(sql);
        foreach (var row in rows)
        {
            var id = (Guid)row.Id;
            var name = (string)row.Name;
            var value = (string)row.Value;

            if (!filtersDict.TryGetValue(id, out var filterDto))
            {
                filterDto = new CharacteristicFilterDto { Id = id, Name = name };
                filtersDict.Add(id, filterDto);
            }
            
            if (!string.IsNullOrWhiteSpace(value) && !filterDto.Values.Contains(value))
            {
                filterDto.Values.Add(value);
            }
        }

        return filtersDict.Values.ToList();
    }
}
