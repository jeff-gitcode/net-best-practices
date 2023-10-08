namespace clean_minimal_api_demo.Application.Movies.Queries.GetMovies;

using Entities;
using MediatR;

public class GetMoviesQuery : IRequest<List<Movie>>
{
}