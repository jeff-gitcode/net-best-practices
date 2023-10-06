namespace minimal_webhook_demo.Infrastructure.Webhook;

public class ConsoleWebhookReceiver : IReceiveWebhook
{
    /// <summary>
    /// Writes the POST request body to the console and returns JSON
    /// </summary>
    public async Task<string> ProcessRequest(string requestBody)
    {
        //This is where you would put your actual business logic for receiving webhooks
        Console.WriteLine($"Request Body: {requestBody}");
        return /*lang=json,strict*/ "{\"message\" : \"Thanks Jeff! I got your webhook\"}";
    }
}
