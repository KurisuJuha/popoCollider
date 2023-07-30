namespace JuhaKurisu.PopoTools.ColliderSystem;

public class ColliderWorld<T>
{
    private readonly ColliderCell<T>[] _colliderCells;
    private readonly WorldTransform _worldTransform;

    public ColliderWorld(ushort level, WorldTransform transform)
    {
        var length = ((int)Math.Pow(4, level + 1) - 1) / 3;
        _colliderCells = new ColliderCell<T>[length];
        _worldTransform = transform;
    }

    public void Register(RectCollider<T> collider)
    {
    }

    private void CreateNewCell()
    {
    }
}