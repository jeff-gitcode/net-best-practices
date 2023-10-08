namespace minimal_caching_demo.Application.Reviews.Commands.DeleteReview;

using MediatR;

public class DeleteReviewCommand : IRequest<bool>
{
    public Guid Id { get; init; }
}
