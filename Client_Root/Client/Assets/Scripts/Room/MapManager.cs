﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public const float                  INTERVAL_OF_NODE = 1f;

    private Map 						m_Map = null;
	private GameObject 					m_goLoadedScene = null;
    private Dictionary<Vector3, Node>   m_dicNode = new Dictionary<Vector3, Node>();
    private LinkedList<Node>            m_listOpenNode = new LinkedList<Node>();
    private QuadTree                    m_qtNode = null;
	
    public void InsertNode(Node node)
    {
        AddNode(node.m_vec3Pos, node);

        List<Node> listNode = m_qtNode.QueryRange(new Rect2D(node.m_vec3Pos.x, node.m_vec3Pos.z, INTERVAL_OF_NODE * 2, INTERVAL_OF_NODE * 2));
        foreach(Node oldNode in listNode)
        {
            node.m_listNeighborNode.Add(oldNode);
            oldNode.m_listNeighborNode.Add(node);
        }
    }
    public void RemoveNode(Node node)
    {
        if (!m_dicNode.ContainsKey(node.m_vec3Pos))
        {
            Debug.LogWarning("node is not contained!");
            return;
        }

        if (node.m_bFixed)
        {
            return;
        }

        m_qtNode.Remove(node);
        m_dicNode.Remove(node.m_vec3Pos);
    }

    public IEnumerator LoadMap(int nID)
	{
        m_Map = new Map();

        //  Set Map Data
        XmlDocument xmlDoc = XmlEditor.Instance.LoadXmlFromResources("Map" + Path.DirectorySeparatorChar + "map_" + nID.ToString());
        if (xmlDoc == null)
        {
            Debug.LogError("xmlDoc is null!");
            yield break;
        }

        SetMapFromXML(m_Map, xmlDoc);

        //  Load map scene
        yield return SceneManager.LoadSceneAsync(m_Map.m_strSceneName, LoadSceneMode.Additive);

        //  Set Map Nodes
        yield return StartCoroutine(SeedingNode(Vector3.zero));
	}

    private void SetMapFromXML(Map map, XmlDocument xmlDoc)
    {
        XmlNode Map = xmlDoc.SelectSingleNode("Map");

        //  General info
        XmlNode ID = Map.SelectSingleNode("ID");
        XmlNode SceneName = Map.SelectSingleNode("SceneName");
        XmlNode Width = Map.SelectSingleNode("Width");
        XmlNode Height = Map.SelectSingleNode("Height");

        map.m_nID = int.Parse(ID.InnerText);
        map.m_strSceneName = SceneName.InnerText;
        map.m_fWidth = float.Parse(Width.InnerText);
        map.m_fHeight = float.Parse(Height.InnerText);

        //  Start area
        XmlNode StartArea = Map.SelectSingleNode("StartArea");
        string[] arrRect = StartArea.InnerText.Split(',');
        if (arrRect.Length != 5)
        {
            Debug.LogWarning("arrRect.Length is not 5!");
            return;
        }

        map.m_rectStartArea = new Rect3D(new Vector3(float.Parse(arrRect[0]), float.Parse(arrRect[1]), float.Parse(arrRect[2])), float.Parse(arrRect[3]), float.Parse(arrRect[4]));

        //  Check points
        XmlNode CheckPoints = Map.SelectSingleNode("CheckPoints");
        XmlNodeList listCheckPoint = CheckPoints.SelectNodes("CheckPoint");
        foreach (XmlNode CheckPoint in listCheckPoint)
        {
            string[] arrPos = CheckPoint.InnerText.Split(',');
            if (arrPos.Length != 3)
            {
                Debug.LogWarning("arrPos.Length is not 3!");
                continue;
            }

            Vector3 vec3CheckPoint = Vector3.zero;
            vec3CheckPoint.x = float.Parse(arrPos[0]);
            vec3CheckPoint.y = float.Parse(arrPos[1]);
            vec3CheckPoint.z = float.Parse(arrPos[2]);

            m_Map.m_listCheckPoint.Add(vec3CheckPoint);
        }

        //  Spawn points
        XmlNode SpawnPoints = Map.SelectSingleNode("SpawnPoints");
        XmlNodeList listSpawnPoint = SpawnPoints.SelectNodes("SpawnPoint");
        foreach (XmlNode SpawnPoint in listSpawnPoint)
        {
            string[] arrPos = SpawnPoint.InnerText.Split(',');
            if (arrPos.Length != 3)
            {
                Debug.LogWarning("arrPos.Length is not 3!");
                continue;
            }

            Vector3 vec3SpawnPoint = Vector3.zero;
            vec3SpawnPoint.x = float.Parse(arrPos[0]);
            vec3SpawnPoint.y = float.Parse(arrPos[1]);
            vec3SpawnPoint.z = float.Parse(arrPos[2]);

            m_Map.m_listSpawnPoint.Add(vec3SpawnPoint);
        }

		//	TerrainObjects
		XmlNode TerrainObjects = Map.SelectSingleNode("TerrainObjects");
		XmlNodeList listTerrainObject = TerrainObjects.SelectNodes("TerrainObject");
		foreach (XmlNode TerrainObject in listTerrainObject)
        {
			string strShape = TerrainObject.InnerText.Split (',')[0];

			TerrainObject terrainObject = CreateTerrainObject (strShape);
			terrainObject.SetData (TerrainObject.InnerText);

			m_Map.m_listTerrainObject.Add(terrainObject);
        }
    }
		
	private TerrainObject CreateTerrainObject(string strShape)
	{
		if (strShape == TerrainObject.ShapeType.Box2d.ToString ())
		{
			return new Box2dShapeTerrainObject();
		}
		else if (strShape == TerrainObject.ShapeType.Sphere2d.ToString ())
		{
            return new Sphere2dShapeTerrainObject();
		}
		else if (strShape == TerrainObject.ShapeType.ConvexPolygon2d.ToString ())
		{
            return new ConvexPolygon2dShapeTerrainObject();
		}

		Debug.LogWarning ("strShape is invalid!, strShape : " + strShape);
		return null;
	}

    private IEnumerator SeedingNode(Vector3 vec3Pos)
    {
        Node nodeSeed = new Node(vec3Pos);
        AddNode(vec3Pos, nodeSeed);

        m_listOpenNode.AddFirst(nodeSeed);

        while (m_listOpenNode.Count > 0)
        {
            Node node = m_listOpenNode.First.Value;
            m_listOpenNode.RemoveFirst();

            ExtendingNode(node);
        }

        yield break;
    }

    private void ExtendingNode(Node node)
    {
        for(int nHorizon = -1; nHorizon <= 1; ++nHorizon)
        {
            for(int nVertical = -1; nVertical <= 1; ++nVertical)
            {
                if (nHorizon == 0 && nVertical == 0)
                {
                    continue;
                }

                Vector3 vec3Dest = new Vector3(node.m_vec3Pos.x + nHorizon * INTERVAL_OF_NODE, node.m_vec3Pos.y, node.m_vec3Pos.z + nVertical * INTERVAL_OF_NODE);

                if(CanMoveStraightly(node.m_vec3Pos, vec3Dest))
                {
                    if (m_dicNode.ContainsKey(vec3Dest))
                    {
                        node.m_listNeighborNode.Add(m_dicNode[vec3Dest]);
                    }
                    else
                    {
                        Node nodeNew = new Node(vec3Dest);
                        AddNode(vec3Dest, nodeNew);

                        node.m_listNeighborNode.Add(nodeNew);

                        m_listOpenNode.AddLast(nodeNew);
                    }
                }
            }
        }
    }

    private bool AddNode(Vector3 vec3Pos, Node node)
    {
        if (m_dicNode.ContainsKey(vec3Pos))
        {
            Debug.LogWarning("m_dicNode already contains!, vec3Pos : " + vec3Pos);
            return false;
        }

        if (m_dicNode.Count == 0)
        {
            m_qtNode = new QuadTree(new Rect2D(0, 0, m_Map.m_fWidth, m_Map.m_fHeight), 0);
        }
        else
        {
            m_qtNode.Insert(node);
        }

        m_dicNode.Add(vec3Pos, node);

        return true;
    }

    public bool CanMoveStraightly(Vector3 start, Vector3 dest)
    {
        if (!IsPositionValidToMove(dest))
        {
            return false;
        }

        Line2D line = new Line2D(new Vector2(start.x, start.z), new Vector2(dest.x, dest.z));
        Vector2 intersectionPos = Vector2.zero;

//        foreach (Polygon polygon in m_Map.m_listObstacle)
//        {
//            if (PhysicsHelper.Collision.FindLineAndPolygonIntersection(line, polygon, ref intersectionPos))
//            {
//                return false;
//            }
//        }

        return true;
    }

    public bool IsPositionValidToMove(Vector3 vec3Pos)
    {
        if (!(vec3Pos.x < m_Map.m_fWidth * 0.5f && vec3Pos.x > m_Map.m_fWidth * -0.5f && vec3Pos.z < m_Map.m_fHeight * 0.5f && vec3Pos.z > m_Map.m_fHeight * -0.5f))
        {
            return false;
        }

//        foreach (Polygon polygon in m_Map.m_listObstacle)
//        {
//            if (PhysicsHelper.Collision.PointInConvexPolygon(vec3Pos, polygon))
//            {
//                return false;
//            }
//        }

        return true;
    }

    public float GetWidth()
    {
        return m_Map.m_fWidth;
    }

    public float GetHeight()
    {
        return m_Map.m_fHeight;
    }

    private Vector3 GetRandomSpawnPoint()
	{
        return m_Map.m_listSpawnPoint[Random.Range(0, m_Map.m_listSpawnPoint.Count)];
	}
}
