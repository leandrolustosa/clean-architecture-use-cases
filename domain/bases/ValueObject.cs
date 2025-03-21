public abstract class ValueObject<T> where T : ValueObject<T>
{
    public override bool Equals(object obj)
    {
        return obj is T valueObject && EqualsCore(valueObject);
    }

    protected abstract bool EqualsCore(T other);

    public override int GetHashCode()
    {
        return GetHashCodeCore();
    }

    protected abstract int GetHashCodeCore();

    public static bool operator ==(ValueObject<T> a, ValueObject<T> b)
    {
        if (a == null)
        {
            return b == null;
        }

        return a.Equals(b);
    }

    public static bool operator !=(ValueObject<T> a, ValueObject<T> b)
    {
        return !(a == b);
    }
}