using System.Collections.Generic;
using JuhaKurisu.PopoTools.Deterministics;

namespace JuhaKurisu.PopoTools.ColliderSystem
{
    public class BoxCollider
    {
        public readonly bool check;
        public readonly FixVector2 position;
        public readonly FixVector2 size;
        public readonly FixVector2 halfSize;
        public readonly FixVector2 leftDownPosition;
        public readonly List<int> quadTreePosition;

        public BoxCollider(FixVector2 position, FixVector2 size, bool check = false)
        {
            this.position = position;
            this.size = size;
            this.halfSize = size / new Fix64(2);
            this.check = check;
            leftDownPosition = position - size / new Fix64(2);
            quadTreePosition = new();
        }

        private void CalculateQuadTreePosition()
        {
            quadTreePosition.Clear();
            while (true)
            {

            }
        }

        public void AddToNode(ColliderNode root)
        {

        }

        public void RemoveFromNode(ColliderNode root)
        {

        }

        public bool Detect(BoxCollider otherCollider)
        {
            if (position.x - otherCollider.position.x < halfSize.x + otherCollider.halfSize.x &&
                position.y - otherCollider.position.y < halfSize.y + otherCollider.halfSize.y)
                return true;

            return false;
        }
    }
}