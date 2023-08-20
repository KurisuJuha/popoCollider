using JuhaKurisu.PopoTools.Deterministics;

namespace PopoTools.ColliderSystem;

public struct AABB : IEquatable<AABB>
{
    public readonly FixVector2 LeftTopPosition;
    public readonly FixVector2 RightBottomPosition;

    public AABB(RectColliderTransform transform)
    {
        var p = transform.Size / Fix64.two;
        var m = -p;

        var pos1 = RotatePoint(new FixVector2(m.x, m.y), transform.Angle) + transform.Position;
        var pos2 = RotatePoint(new FixVector2(m.x, p.y), transform.Angle) + transform.Position;
        var pos3 = RotatePoint(new FixVector2(p.x, p.y), transform.Angle) + transform.Position;
        var pos4 = RotatePoint(new FixVector2(p.x, m.y), transform.Angle) + transform.Position;

        var leftUpPositionX = transform.Position.x;
        var leftUpPositionY = transform.Position.y;
        var rightDownPositionX = transform.Position.x;
        var rightDownPositionY = transform.Position.y;

        // Left Up Position
        if (leftUpPositionX > pos1.x) leftUpPositionX = pos1.x;
        if (leftUpPositionX > pos2.x) leftUpPositionX = pos2.x;
        if (leftUpPositionX > pos3.x) leftUpPositionX = pos3.x;
        if (leftUpPositionX > pos4.x) leftUpPositionX = pos4.x;

        if (leftUpPositionY < pos1.y) leftUpPositionY = pos1.y;
        if (leftUpPositionY < pos2.y) leftUpPositionY = pos2.y;
        if (leftUpPositionY < pos3.y) leftUpPositionY = pos3.y;
        if (leftUpPositionY < pos4.y) leftUpPositionY = pos4.y;

        // Right Down Position
        if (rightDownPositionX < pos1.x) rightDownPositionX = pos1.x;
        if (rightDownPositionX < pos2.x) rightDownPositionX = pos2.x;
        if (rightDownPositionX < pos3.x) rightDownPositionX = pos3.x;
        if (rightDownPositionX < pos4.x) rightDownPositionX = pos4.x;

        if (rightDownPositionY > pos1.y) rightDownPositionY = pos1.y;
        if (rightDownPositionY > pos2.y) rightDownPositionY = pos2.y;
        if (rightDownPositionY > pos3.y) rightDownPositionY = pos3.y;
        if (rightDownPositionY > pos4.y) rightDownPositionY = pos4.y;

        LeftTopPosition = new FixVector2(leftUpPositionX, leftUpPositionY);
        RightBottomPosition = new FixVector2(rightDownPositionX, rightDownPositionY);
    }

    private AABB(FixVector2 leftTopPosition, FixVector2 rightBottomPosition)
    {
        LeftTopPosition = leftTopPosition;
        RightBottomPosition = rightBottomPosition;
    }

    public static FixVector2 RotatePoint(FixVector2 vec, Fix64 angle)
    {
        var radAngle = Fix64.deg2Rad * angle;
        var cos = Fix64.Cos(radAngle);
        var sin = Fix64.Sin(radAngle);

        return new FixVector2(
            vec.x * cos - vec.y * sin,
            vec.x * sin + vec.y * cos
        );
    }

    public readonly AABB Rescale(WorldTransform worldTransform)
    {
        var defaultSize = new Fix64(1 << (int)worldTransform.Level);
        var scale = new FixVector2(worldTransform.Size.x / defaultSize, worldTransform.Size.y / defaultSize);
        return new AABB((LeftTopPosition - worldTransform.LeftBottomPosition) / scale,
            (RightBottomPosition - worldTransform.LeftBottomPosition) / scale);
    }

    public bool Equals(AABB other)
    {
        return LeftTopPosition.Equals(other.LeftTopPosition) &&
               RightBottomPosition.Equals(other.RightBottomPosition);
    }

    public override bool Equals(object obj)
    {
        return obj is AABB other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(LeftTopPosition, RightBottomPosition);
    }

    public override string ToString()
    {
        return $"LT: {LeftTopPosition} RT: {RightBottomPosition}";
    }
}