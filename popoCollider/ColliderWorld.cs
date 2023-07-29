using JuhaKurisu.PopoTools.Deterministics;

namespace JuhaKurisu.PopoTools.ColliderSystem;

public class ColliderWorld<T>
{
    private readonly ColliderCell<T>[] _colliderCells;
    private readonly WorldTransform _worldTransform;

    public ColliderWorld(ushort level, Fix64 left, Fix64 top, Fix64 right, Fix64 bottom)
    {
        var length = ((int)Math.Pow(4, level + 1) - 1) / 3;
        _colliderCells = new ColliderCell<T>[length];
    }

    public void Register(RectCollider<T> collider)
    {
    }

    private void CreateNewCell()
    {
    }
}