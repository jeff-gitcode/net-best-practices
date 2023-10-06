namespace minimal_webhook_demo.Application.WebHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record WebhookDto
{
    public string Body { get; init; }
}
