using System;
using System.Security.AccessControl;

public enum TicketType
{
    Standard,
    Premium,
    Vip
}

public sealed record RegistrationRequest
{
    public required string FullName { get; init; }

    public required string Title { get; init; }

    public required TicketType Type { get; init; }
}

public interface IRegistrationStrategy
{
    public void SubmitRegistration(RegistrationRequest request);
}

public class PremiumRegistrationStrategy : IRegistrationStrategy
{
    public void SubmitRegistration(RegistrationRequest request)
    {
        Console.WriteLine("PremiumRegistrationStrategy.SubmitRegistration");
    }
}

public class StandardRegistrationStrategy : IRegistrationStrategy
{
    public void SubmitRegistration(RegistrationRequest request)
    {
        Console.WriteLine("StandardRegistrationStrategy.SubmitRegistration");
    }
}

public class VipRegistrationStrategy : IRegistrationStrategy
{
    public void SubmitRegistration(RegistrationRequest request)
    {
        Console.WriteLine("VipRegistrationStrategy.SubmitRegistration");
    }
}

public class RegistrationService
{
    public void SubmitRegistration(RegistrationRequest request)
    {
        IRegistrationStrategy strategy = request.Type switch
        {
            TicketType.Premium => new PremiumRegistrationStrategy(),
            TicketType.Standard => new StandardRegistrationStrategy(),
            TicketType.Vip => new VipRegistrationStrategy(),
            _ => throw new ArgumentOutOfRangeException("No strategy found for this type")
        };

        strategy.SubmitRegistration(request);
    }
}