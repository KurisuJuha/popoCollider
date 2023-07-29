using JuhaKurisu.PopoTools.ColliderSystem;

Console.WriteLine(GetMortonNumber(3, 6));
Console.WriteLine(GetRoot(31, 19));
Console.WriteLine(GetStartIndex(2));

var colliderWorld = new ColliderWorld<int>(2);

uint BitSeparate32(uint n)
{
    n = (n | (n << 8)) & 0x00ff00ff;
    n = (n | (n << 4)) & 0x0f0f0f0f;
    n = (n | (n << 2)) & 0x33333333;
    return (n | (n << 1)) & 0x55555555;
}

uint GetMortonNumber(ushort x, ushort y)
{
    return BitSeparate32(x) | (BitSeparate32(y) << 1);
}

uint GetRoot(uint a, uint b)
{
    var shift = 0;

    // 16
    for (var i = 1; i < 16; i++)
        if ((a ^ b) >> (i * 2) != 0)
            shift = (i + 1) * 2;

    return a >> shift;
}

int GetStartIndex(uint n)
{
    return ((int)Math.Pow(4, n) - 1) / 3;
}