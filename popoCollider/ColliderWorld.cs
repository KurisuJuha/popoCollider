using JuhaKurisu.PopoTools.Deterministics;

namespace PopoTools.ColliderSystem;

public class ColliderWorld<T>
{
    private readonly List<(RectCollider<T> collider, IReadOnlyList<RectCollider<T>>)> _checkAllRets = new();
    private readonly HashSet<RectCollider<T>> _checkedColliders = new();

    private readonly List<RectCollider<T>> _checkRets = new();
    public readonly HashSet<RectCollider<T>> CheckColliders = new();
    public readonly HashSet<RectCollider<T>> Colliders = new();
    public readonly Dictionary<(long, long), HashSet<RectCollider<T>>> CollidersMap = new();

    public void AddCollider(RectCollider<T> boxCollider)
    {
        // 失敗したら既に存在するオブジェクトのためreturn
        if (!Colliders.Add(boxCollider)) return;
        if (boxCollider.Check) CheckColliders.Add(boxCollider);
        foreach (var position in boxCollider.GridPositions)
        {
            if (!CollidersMap.ContainsKey(position)) CollidersMap[position] = new HashSet<RectCollider<T>>();

            CollidersMap[position].Add(boxCollider);
        }

        boxCollider.IsRegistered = true;
        boxCollider.World = this;
    }

    public void RemoveCollider(RectCollider<T> boxCollider)
    {
        // 失敗したらそもそも存在しないオブジェクトのためreturn
        if (!Colliders.Remove(boxCollider)) return;
        if (boxCollider.Check) CheckColliders.Remove(boxCollider);
        foreach (var position in boxCollider.GridPositions)
        {
            if (!CollidersMap.ContainsKey(position)) CollidersMap[position] = new HashSet<RectCollider<T>>();

            CollidersMap[position].Remove(boxCollider);
        }

        boxCollider.IsRegistered = false;
    }

    public IReadOnlyList<(RectCollider<T> collider, IReadOnlyList<RectCollider<T>> otherCollider)> CheckAll()
    {
        _checkAllRets.Clear();

        foreach (var collider in CheckColliders)
        {
            var check = Check(collider);
            if (check.Count == 0) continue;
            _checkAllRets.Add((collider, check));
        }

        return new List<(RectCollider<T> collider, IReadOnlyList<RectCollider<T>> otherCollider)>(_checkAllRets);
    }

    public IReadOnlyList<RectCollider<T>> Check(RectCollider<T> boxCollider)
    {
        _checkRets.Clear();
        _checkedColliders.Clear();

        foreach (var position in boxCollider.GridPositions)
        {
            // ないなら返す
            if (!CollidersMap.ContainsKey(position)) continue;

            // あるなら全て判定してcheckedCollidersに登録
            foreach (var otherCollider in CollidersMap[position]
                         .Where(otherCollider => !_checkedColliders.Contains(otherCollider))
                         .Where(otherCollider => otherCollider != boxCollider))
            {
                //当たってるなら返り値に登録
                if (boxCollider.Detect(otherCollider))
                    _checkRets.Add(otherCollider);

                _checkedColliders.Add(otherCollider);
            }
        }

        return new List<RectCollider<T>>(_checkRets);
    }

    public List<RectCollider<T>> PointCast(FixVector2 position)
    {
        _checkRets.Clear();

        var pointGridPosition = (Fix64.FloorToLong(position.x), Fix64.FloorToLong(position.y));
        if (!CollidersMap.ContainsKey(pointGridPosition)) return _checkRets;

        foreach (var otherCollider in CollidersMap[pointGridPosition]
                     .Where(otherCollider => otherCollider.Detect(position)))
            _checkRets.Add(otherCollider);

        return new List<RectCollider<T>>(_checkRets);
    }

    public bool TryPointCast(FixVector2 position)
    {
        return PointCast(position).Count != 0;
    }

    public bool TryPointCast(FixVector2 position, out List<RectCollider<T>> value)
    {
        value = PointCast(position);
        return value.Count != 0;
    }

    public IReadOnlyList<RectCollider<T>> RectCast(FixVector2 position, FixVector2 size)
    {
        return Check(new RectCollider<T>(position, size, Fix64.zero));
    }

    public bool TryRectCast(FixVector2 position, FixVector2 size)
    {
        return RectCast(position, size).Count != 0;
    }

    public bool TryRectCast(FixVector2 position, FixVector2 size, out IReadOnlyList<RectCollider<T>> value)
    {
        value = RectCast(position, size);
        return value.Count != 0;
    }
}