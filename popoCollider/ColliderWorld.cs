namespace JuhaKurisu.PopoTools.ColliderSystem;

public class ColliderWorld<T> : IEquatable<ColliderWorld<T>>
{
    private readonly ColliderCell<T>[] _colliderCells;
    public readonly WorldTransform WorldTransform;

    public ColliderWorld(WorldTransform transform)
    {
        var length = ((int)Math.Pow(4, transform.Level + 1) - 1) / 3;
        _colliderCells = new ColliderCell<T>[length];
        WorldTransform = transform;
    }

    public bool Equals(ColliderWorld<T>? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _colliderCells.Equals(other._colliderCells) && WorldTransform.Equals(other.WorldTransform);
    }

    public void Remove(RectCollider<T> collider)
    {
        _colliderCells[collider.index].Colliders?.Remove(collider);
    }

    public long Register(RectCollider<T> collider)
    {
        var index = collider.index;
        if (_colliderCells[index].Colliders is null) _colliderCells[index] = new ColliderCell<T>();

        _colliderCells[index].Colliders?.Add(collider);

        return index;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((ColliderWorld<T>)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_colliderCells, WorldTransform);
    }
}