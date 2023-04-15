using System.Collections.Generic;
using UnityEngine;
using AnnulusGames.LucidTools.Inspector;
using JuhaKurisu.PopoTools.Deterministics;
using JuhaKurisu.PopoTools.ColliderSystem;
using PopoCollider = JuhaKurisu.PopoTools.ColliderSystem.BoxCollider;

public class CollidersTest : MonoBehaviour
{
    public List<PopoCollider> colliders = new();
    public List<(PopoCollider collider, List<PopoCollider> otherColliders)> checkedColliders;
    private ColliderWorld world = new();
    [SerializeField] private int count;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            PopoCollider collider = new PopoCollider(
                new FixVector2(
                    new(Random.Range(-10, 10)),
                    new(Random.Range(-10, 10))
                ),
                new FixVector2(
                    new(1),
                    new(1)
                ),
                false
            );
            colliders.Add(collider);
            world.AddCollider(collider);
        }
        if (Input.GetKey(KeyCode.Z))
        {
            PopoCollider collider = new PopoCollider(
                new FixVector2(
                    new(Random.Range(-10, 10)),
                    new(Random.Range(-10, 10))
                ),
                new FixVector2(
                    new(1),
                    new(1)
                ),
                true
            );
            colliders.Add(collider);
            world.AddCollider(collider);
        }

        checkedColliders = world.CheckAll();
        count = world?.colliders.Count ?? 0;
    }
}
