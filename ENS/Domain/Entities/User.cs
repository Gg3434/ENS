namespace ENS.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public List<Event> Events { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Role {  get; set; }
}
