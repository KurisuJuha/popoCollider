namespace JuhaKurisu.PopoTools.ColliderSystem;

public sealed class ColliderWorld<T>
{
    private readonly ColliderCell<T>[] _colliderCells;
    private readonly List<RectCollider<T>> _collisionDetectionTargetColliders;
    public readonly List<(T, T)> ContactingColliders;
    public readonly WorldTransform WorldTransform;

    public ColliderWorld(WorldTransform transform)
    {
        var length = ((int)Math.Pow(4, transform.Level + 1) - 1) / 3;
        ContactingColliders = new List<(T, T)>();
        _collisionDetectionTargetColliders = new List<RectCollider<T>>();
        _colliderCells = new ColliderCell<T>[length];
        WorldTransform = transform;
    }

    public void Remove(RectCollider<T> collider)
    {
        if (!collider.IsRegistered) throw new Exception("It is not possible to delete unregistered items.");

        var colliders = _colliderCells[collider.CellIndex].Colliders;

        if (colliders is null || colliders.Count == 0 || !_colliderCells[collider.CellIndex].HasChild) return;
        collider.IsRegistered = false;

        var movingCollider = colliders[^1];
        // コライダーに新しいインデックスを記録しておく
        movingCollider.Index = collider.Index;

        colliders[movingCollider.Index] = movingCollider;
        colliders.RemoveAt(colliders.Count - 1);
    }

    public void Register(RectCollider<T> collider)
    {
        if (collider.IsRegistered)
            throw new Exception("It is not possible to register an item that has already been registered.");

        if (WorldTransform.LeftTopPosition.x > collider.InternalTransform.LeftTopPosition.x)
            throw new Exception("The position or size of the collider crosses a world boundary.");
        if (WorldTransform.LeftTopPosition.y < collider.InternalTransform.LeftTopPosition.y)
            throw new Exception("The position or size of the collider crosses a world boundary.");
        if (WorldTransform.RightBottomPosition.x < collider.InternalTransform.RightBottomPosition.x)
            throw new Exception("The position or size of the collider crosses a world boundary.");
        if (WorldTransform.RightBottomPosition.y > collider.InternalTransform.RightBottomPosition.y)
            throw new Exception("The position or size of the collider crosses a world boundary.");

        var cellIndex = collider.CellIndex;

        // 初期化されていない、または、子を持っていないとされているとき
        if (_colliderCells[cellIndex].Colliders is null || !_colliderCells[cellIndex].HasChild)
        {
            var currentCellIndex = cellIndex;
            while (currentCellIndex >= 0)
            {
                // すでに子がある判定ならばその親たちは全てhasChildがtrueなためこれ以上繰り返さない
                if (_colliderCells[currentCellIndex].HasChild) break;

                if (_colliderCells[currentCellIndex].Colliders is null) _colliderCells[currentCellIndex].Init();

                // 親は全て子を持つのでhasChildをtrueに変更
                _colliderCells[currentCellIndex].HasChild = true;

                // 一つ上に
                currentCellIndex = (currentCellIndex - 1) / 4;
            }
        }

        var colliders = _colliderCells[cellIndex].Colliders;

        collider.Index = colliders.Count;
        colliders.Add(collider);
        collider.IsRegistered = true;
    }

    public List<(T, T)> Check()
    {
        ContactingColliders.Clear();
        _collisionDetectionTargetColliders.Clear();

        Check(0, 0);

        return ContactingColliders;
    }

    private void Check(uint level, int index)
    {
        var cell = _colliderCells[MortonOrder.GetStartIndex(level) + index];

        // 当たり判定を処理する
        if (!cell.HasChild) return;

        // 同じ空間内の全ての組み合わせを処理する
        for (var i = 0; i < cell.Colliders.Count; i++)
        for (var j = i + 1; j < cell.Colliders.Count - i; j++)
            if (cell.Colliders[i].Detect(cell.Colliders[j]))
                ContactingColliders.Add((cell.Colliders[i].Entity, cell.Colliders[j].Entity));
        // 全ての親空間のコライダーとの組み合わせを処理する
        for (var i = 0; i < cell.Colliders.Count; i++)
        for (var j = 0; j < _collisionDetectionTargetColliders.Count; j++)
            if (cell.Colliders[i].Detect(_collisionDetectionTargetColliders[j]))
                ContactingColliders.Add((cell.Colliders[i].Entity, _collisionDetectionTargetColliders[j].Entity));

        // 最大レベルなら返す
        if (WorldTransform.Level == level) return;

        // collisionDetectionTargetCollidersに現在のセルのコライダーたちを追加
        if (cell.Colliders is not null) _collisionDetectionTargetColliders.AddRange(cell.Colliders);

        // 全ての子要素も同じように処理
        for (var i = 0; i < 4; i++) Check(level + 1, index * 4 + i);

        // 全ての巡回が終わったら現在のセルのコライダーたちをPop
        for (var i = 0; i < cell.Colliders?.Count; i++)
            _collisionDetectionTargetColliders.RemoveAt(_collisionDetectionTargetColliders.Count - 1);
    }
}