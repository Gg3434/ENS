namespace ENS.Domain.Entities.Common;
public class EntityBase : IEntity
{
    public Guid Id { get; private set; }
}