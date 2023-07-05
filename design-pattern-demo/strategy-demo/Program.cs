// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
// Strategy pattern test
var service = new RegistrationService();
service.SubmitRegistration(new RegistrationRequest
{
    FullName = "John Doe",
    Title = "Mr",
    Type = TicketType.Premium
});