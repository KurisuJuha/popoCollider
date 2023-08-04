namespace JuhaKurisu.PopoTools.ColliderSystem;

public struct ColliderCell<T> : IEquatable<ColliderCell<T>>
{
    public readonly HashSet<RectCollider<T>>? Colliders;

    public ColliderCell()
    {
        Colliders = new HashSet<RectCollider<T>>();
    }

    public bool Equals(ColliderCell<T> other)
    {
        return Equals(Colliders, other.Colliders);
    }

    public override bool Equals(object? obj)
    {
        return obj is ColliderCell<T> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Colliders != null ? Colliders.GetHashCode() : 0;
    }
}