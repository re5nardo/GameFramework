using UnityEngine;
using System.Collections.Generic;

public abstract class TerrainObject
{
	public enum ShapeType
	{
		Box2d,
		Sphere2d,
		ConvexPolygon2d,
	}

	protected Vector3 m_vec3Position;

	public abstract ShapeType GetShapeType();
	public abstract void SetData(string strText);

    public Vector3 GetPosition()
    {
        return m_vec3Position;
    }

    public void SetPosition(Vector3 vec3Position)
    {
        m_vec3Position = vec3Position;
    }
}

public class Box2dShapeTerrainObject : TerrainObject
{
    private Vector3 m_vec3Rotation;
	private Vector3 m_vec3HalfExtents;

	public override ShapeType GetShapeType()
	{
		return ShapeType.Box2d;
	}

	public override string ToString()
	{
        return string.Format("{0}/{1}/{2}/{3}", ShapeType.Box2d, m_vec3Position, m_vec3Rotation, m_vec3HalfExtents);
	}

	public override void SetData(string strText)
	{
        string[] arrText = strText.Split('/');
            
        m_vec3Position = arrText[1].ToVector3();
        m_vec3Rotation = arrText[2].ToVector3();
        m_vec3HalfExtents = arrText[3].ToVector3();
	}

    public Vector3 GetRotation()
    {
        return m_vec3Rotation;
    }

    public void SetRotation(Vector3 vec3Rotation)
    {
        m_vec3Rotation = vec3Rotation;
    }

    public Vector3 GetHalfExtents()
    {
        return m_vec3HalfExtents;
    }

    public void SetHalfExtents(Vector3 vec3HalfExtents)
    {
        m_vec3HalfExtents = vec3HalfExtents;
    }
}

public class Sphere2dShapeTerrainObject : TerrainObject
{
	private float m_fRadius;

	public override ShapeType GetShapeType()
	{
		return ShapeType.Sphere2d;
	}

	public override string ToString()
	{
        return string.Format("{0}/{1}/{2}", ShapeType.Sphere2d, m_vec3Position, m_fRadius);
	}

	public override void SetData(string strText)
	{
        string[] arrText = strText.Split('/');

        m_vec3Position = arrText[1].ToVector3();
        m_fRadius = float.Parse(arrText[2]);
	}

    public float GetRadius()
    {
        return m_fRadius;
    }

    public void SetRadius(float fRadius)
    {
        m_fRadius = fRadius;
    }
}

public class ConvexPolygon2dShapeTerrainObject : TerrainObject
{
    private List<Vector3> m_listVertex = new List<Vector3>();

	public override ShapeType GetShapeType()
	{
		return ShapeType.ConvexPolygon2d;
	}

	public override string ToString()
	{
        string strText = string.Format("{0}/{1}", ShapeType.ConvexPolygon2d, m_vec3Position);

		foreach(Vector3 vec3Vertex in m_listVertex)
		{
            strText += string.Format("/{0}", vec3Vertex);
		}

		return strText;
	}

	public void AddVertex(Vector3 vec3Vertex)
	{
        m_listVertex.Add(vec3Vertex);
	}

	public override void SetData(string strText)
	{
        string[] arrText = strText.Split('/');

        m_vec3Position = arrText[1].ToVector3();

        for(int i = 2; i < arrText.Length; ++i)
        {
            m_listVertex.Add(arrText[i].ToVector3());
        }
	}

    public List<Vector3> GetVertices()
    {
        return m_listVertex;
    }

    public void SetVertex(int nIndex, Vector3 vec3Value)
    {
        if(nIndex >= m_listVertex.Count)
        {
            Debug.LogWarning("nIndex is invalid!, nIndex : " + nIndex);
            return;
        }

        m_listVertex[nIndex] = vec3Value;
    }

    public Vector3 GetVertex(int nIndex)
    {
        if(nIndex >= m_listVertex.Count)
        {
            Debug.LogWarning("nIndex is invalid!, nIndex : " + nIndex);
            return Vector3.zero;
        }

        return m_listVertex[nIndex];
    }

    public void RemoveVertex(int nIndex)
    {
        if(nIndex >= m_listVertex.Count)
        {
            Debug.LogWarning("nIndex is invalid!, nIndex : " + nIndex);
            return;
        }

        m_listVertex.RemoveAt(nIndex);
    }
}