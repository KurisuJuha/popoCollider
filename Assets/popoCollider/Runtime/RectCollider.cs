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
        public ColliderWorld<T> world { get; internal set; }

        private static Fix64 two = new Fix64(2);

        public RectCollider(FixVector2 position, FixVector2 size, T obj, bool check = false)
        {
            ChangeData(position, size, obj, check);
        }

        public bool Detect(RectCollider<T> otherCollider)
        {
            if (Fix64.Abs(position.x - otherCollider.position.x) < halfSize.x + otherCollider.halfSize.x &&
                Fix64.Abs(position.y - otherCollider.position.y) < halfSize.y + otherCollider.halfSize.y)
                return true;

            return false;
        }

        public void ChangeData(FixVector2 position, FixVector2 size, T obj, bool check = false)
        {
            bool b = isRegistered;
            if (b) world.RemoveCollider(this);

            this.position = position;
            this.size = size;
            this.obj = obj;
            this.check = check;
            this.halfSize = size / two;
            this.leftDownPosition = position - halfSize;
            this.gridPositions = new ReadOnlyCollection<(long, long)>(GetGridPositions());

            if (b) world.AddCollider(this);
        }

        private (long, long)[] GetGridPositions()
        {
            long height = (long)Fix64.Ceiling(size.y) + 1;
            long width = (long)Fix64.Ceiling(size.x) + 1;
            (long, long)[] ret = new (long, long)[height * width];

            for (long y = 0; y < height; y++)
            {
                long _y = y * width;
                for (long x = 0; x < width; x++)
                {
                    ret[_y + x] = (x + Fix64.FloorToLong(leftDownPosition.x), y + Fix64.FloorToLong(leftDownPosition.y));
                }
            }

            return ret;
        }
    }
}