using JuhaKurisu.PopoTools.ColliderSystem;
using JuhaKurisu.PopoTools.Deterministics;

var colliderWorld = new ColliderWorld<int>(new WorldTransform(new FixVector2(0, 0), new FixVector2(10000, 10000)));

const int count = 10000000;
var colliders = new RectCollider<int>[count];
for (var i = 0; i < colliders.Length; i++)
    colliders[i] = new RectCollider<int>(10, new RectColliderTransform(FixVector2.zero, FixVector2.zero, Fix64.zero),
        colliderWorld);

var startTime = DateTime.Now;
for (var i = 0; i < colliders.Length; i++)
{
    colliders[i].Register();
    colliders[i].Remove();
}

var endTime = DateTime.Now;

var time = endTime - startTime;
Console.WriteLine($"count: {count}");
Console.WriteLine($"time: {time.TotalMilliseconds}ms");
Console.WriteLine($"time/count: {(time / count).TotalMilliseconds}ms");

uint BitSeparate32(uint n)
{
    n = (n | (n << 4)) & 0x0f0f0f0f;
    n = (n | (n << 2)) & 0x33333333;
    return (n | (n << 1)) & 0x55555555;
}

uint GetMortonNumber(ushort x, ushort y)
{
    return BitSeparate32(x) | (BitSeparate32(y) << 1);
}

(uint, uint) GetRoot(uint a, uint b)
{
    var shift = 0;

    // 16
    for (var i = 1; i < 8; i++)
        if ((a ^ b) >> (i * 2) != 0)
            shift = i;

    return (a >> ((shift + 1) * 2), (uint)(7 - shift));
}

int GetStartIndex(uint n)
{
    return ((int)Math.Pow(4, n) - 1) / 3;
}