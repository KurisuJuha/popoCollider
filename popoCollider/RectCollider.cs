// ReSharper disable InconsistentNaming

namespace JuhaKurisu.PopoTools.ColliderSystem;

public readonly struct RectCollider<T> : IEquatable<RectCollider<T>>
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

    public void Remove()
    {
        World.Remove(this);
    }

    public void Register()
    {
        World.Register(this);
    }

    private long GetIndex()
    {
        var aabb = AABB.Rescale(World.WorldTransform);
        var leftTopMortonNumber = GetMortonNumber((ushort)aabb.LeftTopPosition.x,
            (ushort)aabb.LeftTopPosition.y);
        var rightBottomMortonNumber = GetMortonNumber((ushort)aabb.RightBottomPosition.x,
            (ushort)aabb.RightBottomPosition.y);
        var (relatedIndex, areaIndex) = GetRoot(leftTopMortonNumber, rightBottomMortonNumber);

        return GetStartIndex(areaIndex) + relatedIndex;
    }

    private uint BitSeparate32(uint n)
    {
        n = (n | (n << 8)) & 0x00ff00ff;
        n = (n | (n << 4)) & 0x0f0f0f0f;
        n = (n | (n << 2)) & 0x33333333;
        return (n | (n << 1)) & 0x55555555;
    }

    private uint GetMortonNumber(ushort x, ushort y)
    {
        return BitSeparate32(x) | (BitSeparate32(y) << 1);
    }

    private (uint, uint) GetRoot(uint a, uint b)
    {
        var shift = 0;

        // 8
        for (var i = 1; i < World.WorldTransform.Level; i++)
            if ((a ^ b) >> (i * 2) != 0)
                shift = i;

        return (a >> ((shift + 1) * 2), (uint)(7 - shift));
    }

    private int GetStartIndex(uint n)
    {
        return ((int)Math.Pow(4, n) - 1) / 3;
    }

    public bool Equals(RectCollider<T> other)
    {
        return AABB.Equals(other.AABB) && EqualityComparer<T>.Default.Equals(Entity, other.Entity) &&
               Transform.Equals(other.Transform) && World.Equals(other.World) && index == other.index;
    }

    public override bool Equals(object? obj)
    {
        return obj is RectCollider<T> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(AABB, Entity, Transform, World, index);
    }
}