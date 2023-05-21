using UnityEngine;

public class RectPointTest : MonoBehaviour
{
    [SerializeField] private Transform rectTransform;
    [SerializeField] private SpriteRenderer sr;

    void Update()
    {
        sr.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 point = sr.transform.position;
        Rect rect1 = new Rect(rectTransform.position, rectTransform.localScale, rectTransform.eulerAngles.z);
        Rect rect2 = new Rect(sr.transform.position, sr.transform.localScale, sr.transform.rotation.eulerAngles.z);

        sr.color = rect1.Detect(rect2) ? Color.red : Color.blue;
    }
}

public class Rect
{
    public readonly Vector2 position;
    public readonly Vector2 size;
    public readonly float angle;
    public readonly Vector2 pos1;
    public readonly Vector2 pos2;
    public readonly Vector2 pos3;
    public readonly Vector2 pos4;

    public Rect(Vector2 position, Vector2 size, float angle)
    {
        this.position = position;
        this.size = size;
        this.angle = angle;

        pos1 = position + new Vector2(-size.x / 2, -size.y / 2);
        pos1 = RotatePoint(pos1, position, angle);
        pos2 = position + new Vector2(-size.x / 2, +size.y / 2);
        pos2 = RotatePoint(pos2, position, angle);
        pos3 = position + new Vector2(+size.x / 2, +size.y / 2);
        pos3 = RotatePoint(pos3, position, angle);
        pos4 = position + new Vector2(+size.x / 2, -size.y / 2);
        pos4 = RotatePoint(pos4, position, angle);
    }

    public bool Detect(Vector2 point)
        => IsRightPoint(pos1, pos2, point) &&
            IsRightPoint(pos2, pos3, point) &&
            IsRightPoint(pos3, pos4, point) &&
            IsRightPoint(pos4, pos1, point);

    public bool Detect(Rect rect)
    {
        if (Detect(rect.pos1)) return true;
        if (Detect(rect.pos2)) return true;
        if (Detect(rect.pos3)) return true;
        if (Detect(rect.pos4)) return true;

        if (rect.Detect(pos1)) return true;
        if (rect.Detect(pos2)) return true;
        if (rect.Detect(pos3)) return true;
        if (rect.Detect(pos4)) return true;

        return false;
    }

    Vector2 RotatePoint(Vector2 vec, Vector2 origin, float angle)
    => RotatePoint(vec - origin, angle) + origin;

    Vector2 RotatePoint(Vector2 vec, float angle)
    {
        float radAngle = Mathf.Deg2Rad * angle;
        float cos = Mathf.Cos(radAngle);
        float sin = Mathf.Sin(radAngle);

        return new Vector2(
            vec.x * cos - vec.y * sin,
            vec.x * sin + vec.y * cos
        );
    }

    bool IsRightPoint(Vector2 a, Vector2 b, Vector2 point)
    {
        float f = (b.x - a.x) * (point.y - a.y) - (point.x - a.x) * (b.y - a.y);
        return f > 0 ? false : true;
    }
}