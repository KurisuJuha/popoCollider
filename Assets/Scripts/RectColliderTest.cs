using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JuhaKurisu.PopoTools.Deterministics;
using JuhaKurisu.PopoTools.ColliderSystem;

public class RectColliderTest : MonoBehaviour
{
    [SerializeField] Transform square1;
    [SerializeField] Transform square2;

    RectCollider<Transform> square1Collider;
    RectCollider<Transform> square2Collider;
    ColliderWorld<Transform> world = new ColliderWorld<Transform>();


    // Start is called before the first frame update
    void Start()
    {
        square1Collider = new RectCollider<Transform>(new(), FixVector2.one, square1, true);
        square2Collider = new RectCollider<Transform>(new(), FixVector2.one, square2, true);
        Debug.Log($"{square1Collider.leftDownPosition.x}, {square1Collider.leftDownPosition.y}");
        Debug.Log($"{square1Collider.size.x}, {square1Collider.size.y}");
        Debug.Log($"{square1Collider.position.x}, {square1Collider.position.y}");
        foreach (var pos in square1Collider.gridPositions)
        {
            Debug.Log($"{pos.Item1}, {pos.Item2}");
        }
        world.AddCollider(square1Collider);
        world.AddCollider(square2Collider);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        square1Collider.ChangeData(
            new FixVector2(
                new Fix64((int)(square1.position.x * 1000)) / new Fix64(1000),
                new Fix64((int)(square1.position.y * 1000)) / new Fix64(1000)
            ),
            FixVector2.one,
            square1,
            true
        );
        square2Collider.ChangeData(
            new FixVector2(
                new Fix64((int)(square2.position.x * 1000)) / new Fix64(1000),
                new Fix64((int)(square2.position.y * 1000)) / new Fix64(1000)
            ),
            FixVector2.one / new FixVector2(2, 2),
            square2,
            true
        );
        // Debug.Log(square1Collider.Detect(square2Collider));

        // foreach (var collider in world.CheckAll())
        // {
        //     Debug.Log($"{collider.collider.obj.transform.name} : {string.Join(" , ", collider.otherCollider.Select(o => o.obj.transform.name))}");
        // }
        foreach (var collider in world.Check(square1Collider))
        {
            Debug.Log($"{collider.obj.transform.name}");
        }
    }
}
