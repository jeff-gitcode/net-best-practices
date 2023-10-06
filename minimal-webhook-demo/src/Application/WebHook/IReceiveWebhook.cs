public interface IReceiveWebhook
{
    Task<string> ProcessRequest(string requestBody);
}
