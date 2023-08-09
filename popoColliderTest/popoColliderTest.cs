using JuhaKurisu.PopoTools.Deterministics;
using PopoTools.ColliderSystem;

namespace popoColliderTest;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var colliderWorld = new ColliderWorld<int>();

        var collider0 = new RectCollider<int>(FixVector2.zero, FixVector2.one, Fix64.zero);
        colliderWorld.AddCollider(collider0);

        var colliders = colliderWorld.RectCast(FixVector2.zero, new FixVector2(100, 100));

        Console.WriteLine(colliders.Count);

        var collider1 = new RectCollider<int>(FixVector2.zero, new FixVector2(100, 100), Fix64.zero);
        Console.WriteLine(collider1.Detect(collider0));
        Console.WriteLine(colliderWorld.Check(collider1).Count);
    }
}