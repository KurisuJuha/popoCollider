using JuhaKurisu.PopoTools.Deterministics;

namespace JuhaKurisu.PopoTools.ColliderSystem;

public readonly struct WorldTransform
{
    public readonly FixVector2 Position;
    public readonly FixVector2 Size;

    public WorldTransform(FixVector2 position, FixVector2 size)
    {
        Position = position;
        Size = size;
    }
}