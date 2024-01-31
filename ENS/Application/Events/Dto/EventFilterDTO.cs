using ENS.Domain.Entities;

namespace ENS.Application.Events.Dto;

public class EventFilterDTO
{
    public DateTime? PostedDate { get; set; }
    public bool? IsPosted{ get; set; }
    public Guid? UserId{ get; set; }
    public EventType? EventType { get; set; }
}
