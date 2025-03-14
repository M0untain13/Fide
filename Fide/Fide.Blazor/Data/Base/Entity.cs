namespace Fide.Blazor.Data.Base;

public class Entity
{
    public virtual Guid Id { get; set; } = Guid.NewGuid();
}
