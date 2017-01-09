﻿using UnityEngine;
using System.Collections.Generic;

public struct Point
{
    public int x;
    public int y;

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

public struct Line2D
{
    public Vector2 start;
    public Vector2 end;

    public Line2D(Vector2 start, Vector2 end)
    {
        this.start = start;
        this.end = end;
    }
}

public struct Rect2D
{
    public Vector2 center;
    public float width;
    public float height;
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;

    public Rect2D(float x, float y, float width, float height)
    {
        center = new Vector2(x, y);
        this.width = width;
        this.height = height;

        xMin = center.x - width * 0.5f;
        xMax = center.x + width * 0.5f;
        yMin = center.y - height * 0.5f;
        yMax = center.y + height * 0.5f;
    }

    public bool Contains(Vector2 pos)
    {
        if (xMin > pos.x || xMax < pos.x || yMin > pos.y || yMax < pos.y)
        {
            return false;
        }

        return true;
    }

    public bool Overlaps(Rect2D rect)
    {
        if (xMin > rect.xMax || xMax < rect.xMin || yMin > rect.yMax || yMax < rect.yMin)
        {
            return false;
        }

        return true;
    }
}

public struct Rect3D
{
    public Vector3 center;
    public float width;     //  x - axis
    public float height;    //  z = axis
    public float xMin;
    public float xMax;
    public float zMin;
    public float zMax;

    public Rect3D(Vector3 center, float width, float height)
    {
        this.center = center;
        this.width = width;
        this.height = height;

        xMin = center.x - width * 0.5f;
        xMax = center.x + width * 0.5f;
        zMin = center.z - height * 0.5f;
        zMax = center.z + height * 0.5f;
    }
}

public class Polygon
{
    public List<Vector3> m_listVertex = new List<Vector3>();
}

public struct Sphere
{
    public Vector3 center;
    public float radius;

    public Sphere(Vector3 center, float radius)
    {
        this.center = center;
        this.radius = radius;
    }
}