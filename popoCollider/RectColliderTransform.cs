using JuhaKurisu.PopoTools.Deterministics;

namespace JuhaKurisu.PopoTools.ColliderSystem;

public struct RectColliderTransform : IEquatable<RectColliderTransform>
{
    public readonly FixVector2 Position;
    public readonly FixVector2 Size;
    public readonly Fix64 Angle;

    public RectColliderTransform(FixVector2 position, FixVector2 size, Fix64 angle)
    {
        Position = position;
        Size = size;
        Angle = angle;
    }

    public bool Equals(RectColliderTransform other)
    {
        return Position.Equals(other.Position) && Size.Equals(other.Size) && Angle.Equals(other.Angle);
    }

    public override bool Equals(object? obj)
    {
        return obj is RectColliderTransform other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Position, Size, Angle);
    }
}