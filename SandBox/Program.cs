using JuhaKurisu.PopoTools.ColliderSystem;
using JuhaKurisu.PopoTools.Deterministics;

var aabb0 = new AABB(new RectColliderTransform(FixVector2.one, FixVector2.one, Fix64.zero));
Console.WriteLine($"{aabb0.LeftTopPosition}, {aabb0.RightBottomPosition}");
var aabb1 = aabb0.Rescale(new WorldTransform(16, FixVector2.zero, new FixVector2(10, 10)));
Console.WriteLine($"{aabb1.LeftTopPosition}, {aabb1.RightBottomPosition}");

var colliderWorld = new ColliderWorld<int>(new WorldTransform(8, new FixVector2(0, 0), new FixVector2(10000, 10000)));

const int count = 10000000;
var colliders = new RectCollider<int>[count];
for (var i = 0; i < colliders.Length; i++)
    colliders[i] = new RectCollider<int>(10,
        new RectColliderTransform(new FixVector2(10000, 10000), FixVector2.zero, Fix64.zero),
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