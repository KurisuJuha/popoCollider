using System.Collections.ObjectModel;
using JuhaKurisu.PopoTools.Deterministics;

namespace JuhaKurisu.PopoTools.ColliderSystem;

public class RectCollider<T>
{
    public RectCollider(FixVector2 position, FixVector2 size, T obj = default, bool check = false)
    {
        ChangeData(position, size, Fix64.zero, obj, check);
    }

    public RectCollider(FixVector2 position, FixVector2 size, Fix64 angle, T obj = default, bool check = false)
    {
        ChangeData(position, size, angle, obj, check);
    }

    public bool Check { get; private set; }
    public FixVector2 Position { get; private set; }
    public FixVector2 Size { get; private set; }
    public Fix64 Angle { get; private set; }
    public FixVector2 Pos1 { get; private set; }
    public FixVector2 Pos2 { get; private set; }
    public FixVector2 Pos3 { get; private set; }
    public FixVector2 Pos4 { get; private set; }
    public ReadOnlyCollection<(long, long)> GridPositions { get; private set; }
    public T Obj { get; private set; }
    public bool IsRegistered { get; internal set; }
    public ColliderWorld<T> World { get; internal set; }

    public void ChangeData(FixVector2 position, FixVector2 size, T obj = default, bool check = false)
    {
        ChangeData(position, size, Fix64.zero, obj, check);
    }

    public void ChangeData(FixVector2 position, FixVector2 size, Fix64 angle, T obj = default, bool check = false)
    {
        var b = IsRegistered;
        if (b) World.RemoveCollider(this);

        Position = position;
        Size = size;
        Angle = angle;
        Obj = obj;
        Check = check;
        SetCorners();
        GridPositions = new ReadOnlyCollection<(long, long)>(GetGridPositions());

        if (b) World.AddCollider(this);
    }

    public bool Detect(RectCollider<T> otherCollider)
    {
        if (Detect(otherCollider.Pos1)) return true;
        if (Detect(otherCollider.Pos2)) return true;
        if (Detect(otherCollider.Pos3)) return true;
        if (Detect(otherCollider.Pos4)) return true;

        if (otherCollider.Detect(Pos1)) return true;
        if (otherCollider.Detect(Pos2)) return true;
        if (otherCollider.Detect(Pos3)) return true;
        if (otherCollider.Detect(Pos4)) return true;

        return false;
    }

    public bool Detect(FixVector2 point)
    {
        return IsRight(Pos1, Pos2, point) &&
               IsRight(Pos2, Pos3, point) &&
               IsRight(Pos3, Pos4, point) &&
               IsRight(Pos4, Pos1, point);
    }

    public FixVector2 RotatePoint(FixVector2 vec, FixVector2 origin, Fix64 angle)
    {
        return RotatePoint(vec - origin, angle) + origin;
    }

    public FixVector2 RotatePoint(FixVector2 vec, Fix64 angle)
    {
        var radAngle = Fix64.deg2Rad * angle;
        var cos = Fix64.Cos(radAngle);
        var sin = Fix64.Sin(radAngle);

        return new FixVector2(
            vec.x * cos - vec.y * sin,
            vec.x * sin + vec.y * cos
        );
    }

    public bool IsRight(FixVector2 a, FixVector2 b, FixVector2 point)
    {
        var f = (b.x - a.x) * (point.y - a.y) - (point.x - a.x) * (b.y - a.y);
        return f > Fix64.zero ? false : true;
    }

    private void SetCorners()
    {
        var p = Size.x / Fix64.two;
        var m = -p;

        Pos1 = RotatePoint(new FixVector2(m, m) + Position, Position, Angle);
        Pos2 = RotatePoint(new FixVector2(m, p) + Position, Position, Angle);
        Pos3 = RotatePoint(new FixVector2(p, p) + Position, Position, Angle);
        Pos4 = RotatePoint(new FixVector2(p, m) + Position, Position, Angle);
    }

    private (long, long)[] GetGridPositions()
    {
        var right = Pos1.x;
        var left = Pos1.x;
        var up = Pos1.y;
        var down = Pos1.y;

        right = Fix64.Max(Pos2.x, right);
        left = Fix64.Min(Pos2.x, left);
        up = Fix64.Max(Pos2.y, up);
        down = Fix64.Min(Pos2.y, down);

        right = Fix64.Max(Pos3.x, right);
        left = Fix64.Min(Pos3.x, left);
        up = Fix64.Max(Pos3.y, up);
        down = Fix64.Min(Pos3.y, down);

        right = Fix64.Max(Pos4.x, right);
        left = Fix64.Min(Pos4.x, left);
        up = Fix64.Max(Pos4.y, up);
        down = Fix64.Min(Pos4.y, down);

        var height = (long)Fix64.Ceiling(up - down) + 1;
        var width = (long)Fix64.Ceiling(right - left) + 1;
        var leftDownPosition = new FixVector2(left, down);
        var ret = new (long, long)[height * width];

        for (long y = 0; y < height; y++)
        {
            var _y = y * width;
            for (long x = 0; x < width; x++)
                ret[_y + x] = (x + Fix64.FloorToLong(leftDownPosition.x), y + Fix64.FloorToLong(leftDownPosition.y));
        }

        return ret;
    }
}