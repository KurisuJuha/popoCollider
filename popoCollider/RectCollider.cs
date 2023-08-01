// ReSharper disable InconsistentNaming

namespace JuhaKurisu.PopoTools.ColliderSystem;

public readonly struct RectCollider<T>
{
    private readonly AABB AABB;
    public readonly T Entity;
    public readonly RectColliderTransform Transform;
    private readonly ColliderWorld<T> World;
    public readonly long index;

    public RectCollider(T entity, RectColliderTransform transform, ColliderWorld<T> world)
    {
        Entity = entity;
        Transform = transform;
        AABB = new AABB(transform);
        World = world;
        index = GetIndex();
    }
    }
}