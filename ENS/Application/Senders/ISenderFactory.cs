using ENS.Application.Senders.Common;
using ENS.Domain.Entities;

namespace ENS.Application.Senders;

/// <summary>
/// Сервис для создания обьекта типа отправляемого ивента
/// </summary>
public interface ISenderFactory
{
    public IEventSender Create(EventType eventType);
}
