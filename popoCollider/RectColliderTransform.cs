using JuhaKurisu.PopoTools.Deterministics;

namespace JuhaKurisu.PopoTools.ColliderSystem;

public struct RectColliderTransform
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
}