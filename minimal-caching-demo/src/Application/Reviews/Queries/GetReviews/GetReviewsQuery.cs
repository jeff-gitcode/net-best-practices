namespace minimal_caching_demo.Application.Reviews.Queries.GetReviews;

using Entities;
using MediatR;

public class GetReviewsQuery : IRequest<List<Review>>
{
}
