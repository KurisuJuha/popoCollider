using JuhaKurisu.PopoTools.Deterministics;
using PopoTools.ColliderSystem;

// var colliderWorld = new ColliderWorld<int>(new WorldTransform(8, new FixVector2(0, 0), new FixVector2(10000, 10000)));
//
// const int count = 10000000;
// var collider = new RectCollider<int>(10,
//     new RectColliderTransform(new FixVector2(5000, 5000), FixVector2.zero, Fix64.zero),
//     colliderWorld);
//
// Console.ReadKey(true);
//
// var startTime = DateTime.Now;
// for (var i = 0; i < count; i++)
// {
//     collider.Register();
//     collider.Remove();
// }
//
// var endTime = DateTime.Now;
//
// var time = endTime - startTime;
// Console.WriteLine($"count: {count}");
// Console.WriteLine($"time: {time.TotalMilliseconds}ms");
// Console.WriteLine($"time/count: {time.TotalNanoseconds / count}ns");

var world =
    new ColliderWorld<int>(new WorldTransform(8, new FixVector2(-4096, -4096), new FixVector2(8192, 8192)));
var count = 100000;
var collider = new RectCollider<int>(0, new RectColliderTransform(), world);

var startTime = DateTime.Now;
for (var i = 0; i < count; i++)
    collider.ChangeTransform(new RectColliderTransform(new FixVector2(100, 100), new FixVector2(1, 1), Fix64.zero));

var endTime = DateTime.Now;

var time = endTime - startTime;

Console.WriteLine($"count: {count}");
Console.WriteLine($"time: {time.TotalMilliseconds}ms");
Console.WriteLine($"time/count: {time.TotalNanoseconds / count}ns");


//