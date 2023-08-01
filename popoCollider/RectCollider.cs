// ReSharper disable InconsistentNaming

namespace JuhaKurisu.PopoTools.ColliderSystem;

public readonly struct RectCollider<T>
{
    public readonly AABB AABB;
    public readonly T Entity;
    public readonly RectColliderTransform Transform;

    public RectCollider(T entity, RectColliderTransform transform)
    {
        Entity = entity;
        Transform = transform;
        AABB = new AABB(transform);
    }
}