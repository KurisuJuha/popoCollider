namespace JuhaKurisu.PopoTools.ColliderSystem;

public class ColliderWorld<T>
{
    private const ushort Level = 8;
    private readonly ColliderCell<T>[] _colliderCells;
    public readonly WorldTransform WorldTransform;

    public ColliderWorld(WorldTransform transform)
    {
        var length = ((int)Math.Pow(4, Level + 1) - 1) / 3;
        _colliderCells = new ColliderCell<T>[length];
        WorldTransform = transform;
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
}