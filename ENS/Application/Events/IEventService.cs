using ENS.Application.Events.Dto;

namespace ENS.Application.Events;
public interface IEventService
{
    void Create(CreateEventRequestDTO eventRequest);
    List<GetAllEventDTO> GetAll(EventFilterDTO filter);
    void Send(Guid id);
}
