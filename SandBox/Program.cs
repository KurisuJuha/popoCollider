using JuhaKurisu.PopoTools.ColliderSystem;
using JuhaKurisu.PopoTools.Deterministics;

var colliderWorld = new ColliderWorld<int>(new WorldTransform(8, new FixVector2(0, 0), new FixVector2(10000, 10000)));

const int count = 10000000;
var collider = new RectCollider<int>(10,
    new RectColliderTransform(new FixVector2(5000, 5000), FixVector2.zero, Fix64.zero),
    colliderWorld);

Console.ReadKey(true);

var startTime = DateTime.Now;
for (var i = 0; i < count; i++)
{
    collider.Register();
    collider.Remove();
}

var endTime = DateTime.Now;

var time = endTime - startTime;
Console.WriteLine($"count: {count}");
Console.WriteLine($"time: {time.TotalMilliseconds}ms");
Console.WriteLine($"time/count: {time.TotalNanoseconds / count}ns");