namespace clean_minimal_api_demo.Application.Reviews.Commands.DeleteReview;

using MediatR;

public class DeleteReviewCommand : IRequest<bool>
{
    public Guid Id { get; init; }
}
