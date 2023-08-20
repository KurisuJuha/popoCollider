namespace PopoTools.ColliderSystem;

public struct ColliderCell<T> : IEquatable<ColliderCell<T>>
{
    public List<RectCollider<T>> Colliders { get; private set; }
    public bool HasChild;

    public void Init()
    {
        Colliders = new List<RectCollider<T>>();
    }

    public bool Equals(ColliderCell<T> other)
    {
        return Equals(Colliders, other.Colliders);
    }

    public override bool Equals(object obj)
    {
        return obj is ColliderCell<T> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Colliders != null ? Colliders.GetHashCode() : 0;
    }
}