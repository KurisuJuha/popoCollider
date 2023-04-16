using System.Collections.Generic;

namespace JuhaKurisu.PopoTools.ColliderSystem
{
    public class ColliderNode
    {
        public HashSet<BoxCollider> colliders { get; private set; }
        public ColliderNode[] childNodes { get; private set; }

        public ColliderNode(HashSet<BoxCollider> colliders)
        {
            this.colliders = colliders;
            childNodes = new ColliderNode[4];
        }

        public void SetChildNode(ColliderNode node, int index)
        {
            childNodes[index] = node;
        }

        public void AddCollider(BoxCollider boxCollider)
        {
            colliders.Add(boxCollider);
        }

        public void RemoveCollider(BoxCollider boxCollider)
        {
            colliders.Remove(boxCollider);
        }
    }
}