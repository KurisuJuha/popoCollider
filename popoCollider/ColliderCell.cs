namespace JuhaKurisu.PopoTools.ColliderSystem;

public struct ColliderCell<T>
{
    public List<RectCollider<T>> Colliders;

    public ColliderCell()
    {
        Colliders = new List<RectCollider<T>>();
    }
}