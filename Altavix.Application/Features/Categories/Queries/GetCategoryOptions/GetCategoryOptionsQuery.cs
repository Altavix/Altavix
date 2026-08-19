using Altavix.Application.Models;
using MediatR;

namespace Altavix.Application.Features.Categories.Queries.GetCategoryOptions;

public class GetCategoryOptionsQuery : IRequest<List<KeyValue>>
{
}
