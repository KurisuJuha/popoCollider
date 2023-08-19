using JuhaKurisu.PopoTools.Deterministics;

namespace PopoTools.ColliderSystem;

public class RectCollider<T>
{
    private readonly ColliderWorld<T> _world;
    private AABB _aabb;
    internal int CellIndex;
    public T Entity;
    internal int Index;
    internal RectColliderTransform InternalTransform;
    internal bool IsRegistered;

    public RectCollider(T entity, RectColliderTransform transform, ColliderWorld<T> world)
    {
        Entity = entity;
        _world = world;
        ChangeTransform(transform);
    }

    public RectColliderTransform Transform
    {
        get => InternalTransform;
        set => ChangeTransform(value);
    }

    public void ChangeTransform(RectColliderTransform transform)
    {
        var isRegistered = IsRegistered;

        // 登録されているなら一度削除
        if (isRegistered) Remove();

        InternalTransform = transform;
        _aabb = new AABB(InternalTransform);
        CellIndex = (int)MortonOrder.GetIndex(_aabb, _world.WorldTransform);

        // もともと登録されていたならもう一度登録しておく
        if (isRegistered) Register();
    }

    public void Remove()
    {
        _world.Remove(this);
    }

    public void Register()
    {
        _world.Register(this);
    }

    public bool Detect(RectCollider<T> otherCollider)
    {
        if (Detect(otherCollider.InternalTransform.LeftBottomPosition)) return true;
        if (Detect(otherCollider.InternalTransform.LeftTopPosition)) return true;
        if (Detect(otherCollider.InternalTransform.RightTopPosition)) return true;
        if (Detect(otherCollider.InternalTransform.RightBottomPosition)) return true;

        if (otherCollider.Detect(InternalTransform.LeftBottomPosition)) return true;
        if (otherCollider.Detect(InternalTransform.LeftTopPosition)) return true;
        if (otherCollider.Detect(InternalTransform.RightTopPosition)) return true;
        if (otherCollider.Detect(InternalTransform.RightBottomPosition)) return true;

        return false;
    }

    private bool Detect(FixVector2 point)
    {
        return IsRight(InternalTransform.LeftBottomPosition, InternalTransform.LeftTopPosition, point) &&
               IsRight(InternalTransform.LeftTopPosition, InternalTransform.RightTopPosition, point) &&
               IsRight(InternalTransform.RightTopPosition, InternalTransform.RightBottomPosition, point) &&
               IsRight(InternalTransform.RightBottomPosition, InternalTransform.LeftBottomPosition, point);
    }

    private static bool IsRight(FixVector2 a, FixVector2 b, FixVector2 point)
    {
        var f = (b.x - a.x) * (point.y - a.y) - (point.x - a.x) * (b.y - a.y);
        return f <= Fix64.zero;
    }

    public override string ToString()
    {
        return $"({_aabb} {InternalTransform})";
    }
}