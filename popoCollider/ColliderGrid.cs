namespace JuhaKurisu.PopoTools.ColliderSystem;

public class ColliderGrid<T>
{
    private readonly int _level;
    private ColliderGrid<T>? _childGrid0;
    private ColliderGrid<T>? _childGrid1;
    private ColliderGrid<T>? _childGrid2;
    private ColliderGrid<T>? _childGrid3;
    public HashSet<RectCollider<T>> Colliders = new();

    public ColliderGrid(int level)
    {
        _level = level;
    }

    public ColliderGrid<T> ChildGrid0 => _childGrid0 ??= new ColliderGrid<T>(_level + 1);
    public ColliderGrid<T> ChildGrid1 => _childGrid1 ??= new ColliderGrid<T>(_level + 1);
    public ColliderGrid<T> ChildGrid2 => _childGrid2 ??= new ColliderGrid<T>(_level + 1);
    public ColliderGrid<T> ChildGrid3 => _childGrid3 ??= new ColliderGrid<T>(_level + 1);
}