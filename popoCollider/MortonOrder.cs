namespace PopoTools.ColliderSystem;

public static class MortonOrder
{
    public static uint BitSeparate(uint n)
    {
        n = (n | (n << 8)) & 0x00ff00ff;
        n = (n | (n << 4)) & 0x0f0f0f0f;
        n = (n | (n << 2)) & 0x33333333;
        return (n | (n << 1)) & 0x55555555;
    }

    public static uint GetMortonNumber(ushort x, ushort y)
    {
        return BitSeparate(x) | (BitSeparate(y) << 1);
    }

    public static uint GetIndex(uint a, uint b, uint level)
    {
        var highLevel = 0;

        var def = a ^ b;

        for (var i = 0; i < level; i++)
        {
            var check = (def >> (i * 2)) & 0x3;
            if (check != 0) highLevel = i + 1;
        }

        var space = a >> (highLevel * 2);
        var startIndex = (uint)GetStartIndex(level - highLevel);

        return space + startIndex;
    }

    public static int GetStartIndex(long n)
    {
        return ((int)Math.Pow(4, n) - 1) / 3;
    }

    public static long GetIndex(AABB aabb, WorldTransform worldTransform)
    {
        var rescaledAABB = aabb.Rescale(worldTransform);
        var leftTopMortonNumber = GetMortonNumber((ushort)rescaledAABB.LeftTopPosition.x,
            (ushort)rescaledAABB.LeftTopPosition.y);
        var rightBottomMortonNumber = GetMortonNumber((ushort)rescaledAABB.RightBottomPosition.x,
            (ushort)rescaledAABB.RightBottomPosition.y);

        return GetIndex(leftTopMortonNumber, rightBottomMortonNumber,
            worldTransform.Level);
    }
}