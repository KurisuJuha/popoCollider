namespace JuhaKurisu.PopoTools.ColliderSystem;

public struct ColliderCell<T>
{
    public readonly HashSet<RectCollider<T>>? Colliders;

    public ColliderCell()
    {
        Colliders = new HashSet<RectCollider<T>>();
    }
}