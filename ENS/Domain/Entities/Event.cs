using ENS.Domain.Entities.Common;

namespace ENS.Domain.Entities;
public class Event : EntityBase
{
    public DateTime CreatedDate { get; set; }
    public Guid UserId { get; set; }
    public DateTime PostedDate { get; set; }
    public List<string> Contacts { get; set; }
    public string Text { get; set; }
    public bool IsPosted { get; set; }
    public string Subject { get; set; }
    public EventType Type { get; set; }
}
public enum EventType
{
    Email,
    Phone
}