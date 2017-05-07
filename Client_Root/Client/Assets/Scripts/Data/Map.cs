using UnityEngine;
using System.Collections.Generic;

public class Map
{
	public int 					m_nID = -1;
	public string 				m_strSceneName = "";
	public float 				m_fWidth = 0f;
	public float 				m_fHeight = 0f;
    public Rect3D               m_rectStartArea = default(Rect3D);
    public List<Vector3>        m_listCheckPoint = new List<Vector3>();
    public List<Vector3>        m_listSpawnPoint = new List<Vector3>();
	public List<TerrainObject> 	m_listTerrainObject = new List<TerrainObject>();
}