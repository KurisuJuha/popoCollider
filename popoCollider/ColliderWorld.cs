namespace JuhaKurisu.PopoTools.ColliderSystem;

public class ColliderWorld<T>
{
    private readonly ColliderCell<T>[] _colliderCells;

    public ColliderWorld(ushort level)
    {
        var length = ((int)Math.Pow(4, level + 1) - 1) / 3;
        _colliderCells = new ColliderCell<T>[length];
    }

    public void Register(RectCollider<T> collider)
    {
    }
}