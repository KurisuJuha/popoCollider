// ReSharper disable InconsistentNaming

using JuhaKurisu.PopoTools.Deterministics;

namespace JuhaKurisu.PopoTools.ColliderSystem;

public readonly struct AABB
{
    public readonly FixVector2 LeftTopPosition;
    public readonly FixVector2 RightBottomPosition;

    public AABB(RectColliderTransform transform)
    {
        var p = transform.Size.x / Fix64.two;
        var m = -p;

        var pos1 = RotatePoint(new FixVector2(m, m) + transform.Position, transform.Position, transform.Angle);
        var pos2 = RotatePoint(new FixVector2(m, p) + transform.Position, transform.Position, transform.Angle);
        var pos3 = RotatePoint(new FixVector2(p, p) + transform.Position, transform.Position, transform.Angle);
        var pos4 = RotatePoint(new FixVector2(p, m) + transform.Position, transform.Position, transform.Angle);

        var leftUpPositionX = transform.Position.x;
        var leftUpPositionY = transform.Position.y;
        var rightDownPositionX = transform.Position.x;
        var rightDownPositionY = transform.Position.y;

        // Left Up Position
        if (leftUpPositionX > pos1.x) leftUpPositionX = pos1.x;
        if (leftUpPositionY < pos1.y) leftUpPositionY = pos1.y;
        if (leftUpPositionX > pos2.x) leftUpPositionX = pos2.x;
        if (leftUpPositionY < pos2.y) leftUpPositionY = pos2.y;
        if (leftUpPositionX > pos3.x) leftUpPositionX = pos3.x;
        if (leftUpPositionY < pos3.y) leftUpPositionY = pos3.y;
        if (leftUpPositionX > pos4.x) leftUpPositionX = pos4.x;
        if (leftUpPositionY < pos4.y) leftUpPositionY = pos4.y;

        // Right Down Position
        if (rightDownPositionX < pos1.x) rightDownPositionX = pos1.x;
        if (rightDownPositionY > pos1.y) rightDownPositionY = pos1.y;
        if (rightDownPositionX < pos2.x) rightDownPositionX = pos2.x;
        if (rightDownPositionY > pos2.y) rightDownPositionY = pos2.y;
        if (rightDownPositionX < pos3.x) rightDownPositionX = pos3.x;
        if (rightDownPositionY > pos3.y) rightDownPositionY = pos3.y;
        if (rightDownPositionX < pos4.x) rightDownPositionX = pos4.x;
        if (rightDownPositionY > pos4.y) rightDownPositionY = pos4.y;

        LeftTopPosition = new FixVector2(leftUpPositionX, leftUpPositionY);
        RightBottomPosition = new FixVector2(rightDownPositionX, rightDownPositionY);
    }

    private FixVector2 RotatePoint(FixVector2 vec, FixVector2 origin, Fix64 angle)
    {
        return RotatePoint(vec - origin, angle) + origin;
    }

    private FixVector2 RotatePoint(FixVector2 vec, Fix64 angle)
    {
        var radAngle = Fix64.deg2Rad * angle;
        var cos = Fix64.Cos(radAngle);
        var sin = Fix64.Sin(radAngle);

        return new FixVector2(
            vec.x * cos - vec.y * sin,
            vec.x * sin + vec.y * cos
        );
    }
}