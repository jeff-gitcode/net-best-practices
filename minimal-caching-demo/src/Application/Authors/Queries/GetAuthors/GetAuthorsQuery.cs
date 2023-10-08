namespace minimal_caching_demo.Application.Authors.Queries.GetAuthors;

using Entities;
using MediatR;

public class GetAuthorsQuery : IRequest<List<Author>>
{
}
