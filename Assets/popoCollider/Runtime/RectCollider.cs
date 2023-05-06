using System.Collections.Generic;
using System.Collections.ObjectModel;
using JuhaKurisu.PopoTools.Deterministics;

namespace JuhaKurisu.PopoTools.ColliderSystem
{
    public class RectCollider<T>
    {
        public bool check { get; private set; }
        public FixVector2 position { get; private set; }
        public FixVector2 size { get; private set; }
        public FixVector2 halfSize { get; private set; }
        public FixVector2 leftDownPosition { get; private set; }
        public ReadOnlyCollection<(long, long)> gridPositions { get; private set; }
        public T obj { get; private set; }
        public bool isRegistered { get; internal set; }

        private static Fix64 two = new Fix64(2);

        public RectCollider(FixVector2 position, FixVector2 size, T obj, bool check = false)
        {
            ChangeData(position, size, obj, check);
        }

        public bool Detect(RectCollider<T> otherCollider)
        {
            if (position.x - otherCollider.position.x < halfSize.x + otherCollider.halfSize.x &&
                position.y - otherCollider.position.y < halfSize.y + otherCollider.halfSize.y)
                return true;

            return false;
        }

        public void ChangeData(FixVector2 position, FixVector2 size, T obj, bool check = false)
        {
            this.position = position;
            this.size = size;
            this.obj = obj;
            this.halfSize = size / two;
            this.check = check;
            leftDownPosition = position - size / two;
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
    }
}