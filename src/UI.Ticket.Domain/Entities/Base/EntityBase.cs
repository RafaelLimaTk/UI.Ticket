
namespace UI.Ticket.Domain.Entities.Base;
public abstract class EntityBase : IEntity
{
    protected EntityBase()
    {
        this.Id = this.Id == Guid.Empty ? Guid.NewGuid() : this.Id;
    }

    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public override bool Equals(object? obj)
    {
        var compareTo = obj as EntityBase;
        if (ReferenceEquals(this, compareTo)) return true;
        if (ReferenceEquals(null, compareTo)) return false;
        return Id.Equals(compareTo.Id);
    }

    public static bool operator ==(EntityBase a, EntityBase b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) return true;
        if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
        return a.Equals(b);
    }

    public static bool operator !=(EntityBase a, EntityBase b) => !(a == b);

    public override int GetHashCode() => (GetType().GetHashCode() * 13) + Id.GetHashCode();

    public override string ToString() => GetType().Name + " [Id=" + Id + "]";
}
