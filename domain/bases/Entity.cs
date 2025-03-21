public abstract class Entity : IEntity<long>, ILookupEntity<long>, IEquatable<Entity>
{
    public virtual long Id { get; set; }

    public virtual long Key => Id;

    public virtual long ParentKey => 0;

    public virtual string Value => ToString();

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Entity) obj);
    }

    public static bool operator ==(Entity a, Entity b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            return true;

        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity a, Entity b)
    {
        return !(a == b);
    }

    public bool Equals(Entity other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id == other.Id;
    }


    public override string ToString()
    {
        return GetType().Name + " [Id=" + Id + "]";
    }

    public override int GetHashCode()
    {
        unchecked // Overflow is fine, just wrap
        {
            int hash = 17;
            // Suitable nullity checks etc, of course :)
            hash = hash * 23 + Id.GetHashCode();
            hash = hash * 23 + Value.GetHashCode();
            hash = hash * 23 + Key.GetHashCode();
            return hash;
        }
    }
}