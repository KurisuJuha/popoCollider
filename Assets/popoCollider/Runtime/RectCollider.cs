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
        public Fix64 angle { get; private set; }
        public FixVector2 pos1 { get; private set; }
        public FixVector2 pos2 { get; private set; }
        public FixVector2 pos3 { get; private set; }
        public FixVector2 pos4 { get; private set; }
        public ReadOnlyCollection<(long, long)> gridPositions { get; private set; }
        public T obj { get; private set; }
        public bool isRegistered { get; internal set; }
        public ColliderWorld<T> world { get; internal set; }

        private static Fix64 two = new Fix64(2);

        public RectCollider(FixVector2 position, FixVector2 size, Fix64 angle, T obj, bool check = false)
        {
            ChangeData(position, size, angle, obj, check);
        }

        public void ChangeData(FixVector2 position, FixVector2 size, Fix64 angle, T obj, bool check = false)
        {
            bool b = isRegistered;
            if (b) world.RemoveCollider(this);

            this.position = position;
            this.size = size;
            this.angle = angle;
            this.obj = obj;
            this.check = check;
            SetCorners();
            this.gridPositions = new ReadOnlyCollection<(long, long)>(GetGridPositions());

            if (b) world.AddCollider(this);
        }

        public bool Detect(RectCollider<T> otherCollider)
        {
            if (Detect(otherCollider.pos1)) return true;
            if (Detect(otherCollider.pos2)) return true;
            if (Detect(otherCollider.pos3)) return true;
            if (Detect(otherCollider.pos4)) return true;

            if (otherCollider.Detect(pos1)) return true;
            if (otherCollider.Detect(pos2)) return true;
            if (otherCollider.Detect(pos3)) return true;
            if (otherCollider.Detect(pos4)) return true;

            return false;
        }

        public bool Detect(FixVector2 point)
            => IsRight(pos1, pos2, point) &&
                IsRight(pos2, pos3, point) &&
                IsRight(pos3, pos4, point) &&
                IsRight(pos4, pos1, point);

        public FixVector2 RotatePoint(FixVector2 vec, FixVector2 origin, Fix64 angle)
            => RotatePoint(vec - origin, angle) + origin;

        public FixVector2 RotatePoint(FixVector2 vec, Fix64 angle)
        {
            Fix64 radAngle = Fix64.deg2Rad * angle;
            Fix64 cos = Fix64.Cos(radAngle);
            Fix64 sin = Fix64.Sin(radAngle);

            return new FixVector2(
                vec.x * cos - vec.y * sin,
                vec.x * sin + vec.y * cos
            );
        }

        public bool IsRight(FixVector2 a, FixVector2 b, FixVector2 point)
        {
            Fix64 f = (b.x - a.x) * (point.y - a.y) - (point.x - a.x) * (b.y - a.y);
            return f > Fix64.zero ? false : true;
        }

        private void SetCorners()
        {
            Fix64 p = size.x / Fix64.two;
            Fix64 m = -p;

            pos1 = RotatePoint(new FixVector2(m, m) + position, position, angle);
            pos2 = RotatePoint(new FixVector2(m, p) + position, position, angle);
            pos3 = RotatePoint(new FixVector2(p, p) + position, position, angle);
            pos4 = RotatePoint(new FixVector2(p, m) + position, position, angle);
        }

        private (long, long)[] GetGridPositions()
        {
            Fix64 right = pos1.x;
            Fix64 left = pos1.x;
            Fix64 up = pos1.y;
            Fix64 down = pos1.y;

            right = Fix64.Max(pos2.x, right);
            left = Fix64.Min(pos2.x, left);
            up = Fix64.Max(pos2.y, up);
            down = Fix64.Min(pos2.y, down);

            right = Fix64.Max(pos3.x, right);
            left = Fix64.Min(pos3.x, left);
            up = Fix64.Max(pos3.y, up);
            down = Fix64.Min(pos3.y, down);

            right = Fix64.Max(pos4.x, right);
            left = Fix64.Min(pos4.x, left);
            up = Fix64.Max(pos4.y, up);
            down = Fix64.Min(pos4.y, down);

            long height = (long)Fix64.Ceiling(up - down) + 1;
            long width = (long)Fix64.Ceiling(right - left) + 1;
            FixVector2 leftDownPosition = new FixVector2(left, down);
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