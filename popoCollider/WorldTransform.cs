using JuhaKurisu.PopoTools.Deterministics;

namespace PopoTools.ColliderSystem;

public struct WorldTransform : IEquatable<WorldTransform>
{
    public readonly uint Level;
    public readonly FixVector2 LeftTopPosition;
    public readonly FixVector2 RightBottomPosition;
    public readonly FixVector2 LeftBottomPosition;
    public readonly FixVector2 Size;

    public WorldTransform(uint level, FixVector2 leftBottomPosition, FixVector2 size)
    {
        Level = level;
        LeftBottomPosition = leftBottomPosition;

        LeftTopPosition = leftBottomPosition + new FixVector2(0, size.y);
        RightBottomPosition = leftBottomPosition + new FixVector2(size.x, 0);
        Size = size;
    }

    public bool Equals(WorldTransform other)
    {
        return Level == other.Level && LeftBottomPosition.Equals(other.LeftBottomPosition) &&
               Size.Equals(other.Size);
    }

    public override bool Equals(object obj)
    {
        return obj is WorldTransform other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Level, LeftBottomPosition, Size);
    }
}