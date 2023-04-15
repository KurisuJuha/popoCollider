using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public readonly ReadOnlyCollection<(long, long)> gridPositions;

        public BoxCollider(FixVector2 position, FixVector2 size, bool check = false)
        {
            this.position = position;
            this.size = size;
            this.halfSize = size / new Fix64(2);
            this.check = check;
            leftDownPosition = position - size / new Fix64(2);
            gridPositions = new(GetGridPositions());
        }

        private (long, long)[] GetGridPositions()
        {
            List<(long, long)> rets = new();

            for (int y = 0; y < (long)size.y; y++)
            {
                for (int x = 0; x < (long)size.x; x++)
                {
                    rets.Add((x + ((long)leftDownPosition.x), y + (long)leftDownPosition.y));
                }
            }

            return rets.ToArray();
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