using ENS.Domain.Entities;

namespace ENS.Application.Senders.Common;

public interface IEventSender
{
    public void Send(Event @event);
}
