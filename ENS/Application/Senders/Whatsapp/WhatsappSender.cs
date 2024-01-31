using ENS.Application.Senders.Common;
using ENS.Domain.Entities;
using RestSharp;

namespace ENS.Application.Senders.Whatsapp;

public class WhatsappSender : IEventSender
{
    public const string OneCaseTemplate = "Добрый день, мы оповещаем что на указанный {0}" +
        " было запланировано событие на {1}";
    public async void Send(Event @event)
    {
        var options = new RestClientOptions("https://gate.whapi.cloud/messages/text?token=wyHkg1XCIK4G8Q45gLz8WAXEKjNbCuyE");

        var client = new RestClient(options);
        foreach (var contact in @event.Contacts)
        {
            var request = new RestRequest("", Method.Post);
            var body = new
            {
                typing_time = 0,
                To = contact,
                Body = string.Format(OneCaseTemplate, contact, @event.PostedDate)
            };

            request.AddHeader("accept", "application/json");

            request.AddJsonBody(body);

            await client.PostAsync(request);
        }
    }
}
