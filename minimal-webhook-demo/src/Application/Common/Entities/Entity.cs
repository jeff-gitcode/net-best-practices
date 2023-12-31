namespace minimal_webhook_demo.Application.Common.Entities;

public abstract record Entity
{
    public Guid Id { get; init; }
    public DateTime DateCreated { get; init; }
    public DateTime DateModified { get; init; }
}
