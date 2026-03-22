using System;
using UnityEngine;

[Serializable]
public struct GridCoordinate
{
    public int x;
    public int y;

    public GridCoordinate(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public Vector2 ToVector2()
    {
        return new Vector2(x, y);
    }

    public static float Distance(GridCoordinate a, GridCoordinate b)
    {
        return Vector2.Distance(a.ToVector2(), b.ToVector2());
    }

    public override string ToString()
    {
        return $"({x},{y})";
    }
}
