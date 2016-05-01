using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Polygon
{
    public List<Vector3> m_listVertex = new List<Vector3>();
}

public class Map
{
	public int 					m_nID = -1;
	public string 				m_strSceneName = "";
	public float 				m_fWidth = 0f;
	public float 				m_fHeight = 0f;
    public List<Vector3>        m_listSpawnPoint = new List<Vector3>();
    public List<Polygon>        m_listObstacle = new List<Polygon>();
    public List<INode>          m_listNode = new List<INode>();
    public List<IEdge>          m_listEdge = new List<IEdge>();
}
