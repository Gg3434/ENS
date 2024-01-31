using ENS.Application.Senders.Common;
using ENS.Application.Senders.Email;
using ENS.Application.Senders.Whatsapp;
using ENS.Domain.Entities;

namespace ENS.Application.Senders;

/// <summary>
/// Класс отвечающий за создание обьекта типа передаваемого ивента
/// </summary>
/// <param name="eventSenders">Список типов отправляемых ивентов</param>
public class SenderFactory(IEnumerable<IEventSender> eventSenders) : ISenderFactory
{
    public IEventSender Create(EventType eventType)
    {
        return eventType switch
        {
            EventType.Email => eventSenders.First(x => x is EmailSender),
            EventType.Phone => eventSenders.First(x => x is WhatsappSender),
            _ => throw new Exception()
        };
    }
}
