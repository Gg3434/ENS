using ENS.Domain.Entities;

namespace ENS.Application.Events.Dto;
public class CreateEventRequestDTO
{
    public DateTime PostedDate { get; set; }
    public List<string> Contacts { get; set; }
    public string Text { get; set; }
    public string Subject { get; set; }
    public EventType Type { get; set; }
}
