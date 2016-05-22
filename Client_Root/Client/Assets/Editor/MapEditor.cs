using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Xml;
using UnityEditor.AnimatedValues;

public class MapEditor : EditorWindow
{
    #region Static Functions
	[MenuItem("Tools/MapEditor/New")]
	public static void New(MenuCommand cmd)
	{
		MapEditor editor = EditorWindow.GetWindow<MapEditor>(false, "MapEditor", true);

		editor.Reset();
	}

	[MenuItem("Tools/MapEditor/Load")]
	public static void Load(MenuCommand cmd)
	{
        string strFilePath = EditorUtility.OpenFilePanel("Load Map", "", "xml");
        if (strFilePath.Length == 0)
        {
            return;
        }

        MapEditor editor = EditorWindow.GetWindow<MapEditor>(false, "MapEditor", true);

        editor.LoadMap(strFilePath);
	}
    #endregion

    private enum State
    {
        Normal,
        AddSpawnPoint,
        AddObstacleVertex,
    }

    private const float MOUSE_CLICK_THRESHOLD = 1;


    private Vector3 m_vec3MouseDownPos = Vector3.zero;

    private Vector2 m_vec2SpawnPointsScrollPos = Vector2.zero;
    private Vector2 m_vec2ObstaclesScrollPos = Vector2.zero;

    private List<bool> m_listSpawnPointsHighlight = new List<bool>();
    private List<AnimBool> m_listAnimBool = new List<AnimBool>();

    private int m_ObstacleIndex = -1;


    private Map m_Map = new Map();
    private State m_State = State.Normal;

    public MapEditor()
    {
        SceneView.onSceneGUIDelegate += OnSceneGUIHandler;
    }

    private void OnDestroy()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUIHandler;
    }

	public void Reset()
	{
        m_Map = new Map();
	}

    public void LoadMap(string strFilePath)
    {
        XmlDocument xmlDoc = XmlEditor.Instance.LoadXml(strFilePath);

        SetMapFromXML(m_Map, xmlDoc);
    }

    private void SetMapFromXML(Map map, XmlDocument xmlDoc)
    {
        XmlNode Map = xmlDoc.SelectSingleNode("Map");

        //  general info
        XmlNode ID = Map.SelectSingleNode("ID");
        XmlNode SceneName = Map.SelectSingleNode("SceneName");
        XmlNode Width = Map.SelectSingleNode("Width");
        XmlNode Height = Map.SelectSingleNode("Height");

        map.m_nID = int.Parse(ID.InnerText);
        map.m_strSceneName = SceneName.InnerText;
        map.m_fWidth = float.Parse(Width.InnerText);
        map.m_fHeight = float.Parse(Height.InnerText);

        //  spawn points
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

            AddSpawnPoint(vec3SpawnPoint);
        }

        //  obstacles
        XmlNode Obstacles = Map.SelectSingleNode("Obstacles");
        XmlNodeList listObstacle = Obstacles.SelectNodes("Obstacle");
        foreach (XmlNode Obstacle in listObstacle)
        {
            Polygon obstacle =  AddObstacle();

            XmlNodeList listVertex = Obstacle.SelectNodes("Vertex");
            foreach (XmlNode Vertex in listVertex)
            {
                string[] arrPos = Vertex.InnerText.Split(',');
                if (arrPos.Length != 3)
                {
                    Debug.LogWarning("arrPos.Length is not 3!");
                    continue;
                }
                    
                Vector3 vec3Vertex = Vector3.zero;
                vec3Vertex.x = float.Parse(arrPos[0]);
                vec3Vertex.y = float.Parse(arrPos[1]);
                vec3Vertex.z = float.Parse(arrPos[2]);

                obstacle.m_listVertex.Add(vec3Vertex);
            }
        }
    }

    private void SaveMap()
    {
        //  save data
        XmlDocument xmlDoc = new XmlDocument();
        XmlNode docNode = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
        xmlDoc.AppendChild(docNode);

        //  map
        XmlNode Map = xmlDoc.CreateElement("Map");
        xmlDoc.AppendChild(Map);

        //  id
        XmlNode ID = xmlDoc.CreateElement("ID");
        ID.InnerText = m_Map.m_nID.ToString();
        Map.AppendChild(ID);

        //  scene name
        XmlNode SceneName = xmlDoc.CreateElement("SceneName");
        SceneName.InnerText = m_Map.m_strSceneName;
        Map.AppendChild(SceneName);

        //  width
        XmlNode Width = xmlDoc.CreateElement("Width");
        Width.InnerText = m_Map.m_fWidth.ToString();
        Map.AppendChild(Width);

        //  height
        XmlNode Height = xmlDoc.CreateElement("Height");
        Height.InnerText = m_Map.m_fHeight.ToString();
        Map.AppendChild(Height);

        //  spawn points
        XmlNode SpawnPoints = xmlDoc.CreateElement("SpawnPoints");
        Map.AppendChild(SpawnPoints);

        foreach (Vector3 vec3SpawnPoint in m_Map.m_listSpawnPoint)
        {
            XmlNode SpawnPoint = xmlDoc.CreateElement("SpawnPoint");
            SpawnPoint.InnerText = vec3SpawnPoint.x.ToString() + "," + vec3SpawnPoint.y.ToString() + "," + vec3SpawnPoint.z.ToString();
            SpawnPoints.AppendChild(SpawnPoint);
        }

        //  obstacles
        XmlNode Obstacles = xmlDoc.CreateElement("Obstacles");
        Map.AppendChild(Obstacles);

        foreach (Polygon polyObstacle in m_Map.m_listObstacle)
        {
            XmlNode Obstacle = xmlDoc.CreateElement("Obstacle");

            foreach (Vector3 vec3Vertex in polyObstacle.m_listVertex)
            {
                XmlNode Vertex = xmlDoc.CreateElement("Vertex");
                Vertex.InnerText = vec3Vertex.x.ToString() + "," + vec3Vertex.y.ToString() + "," + vec3Vertex.z.ToString();
                Obstacle.AppendChild(Vertex);
            }

            Obstacles.AppendChild(Obstacle);
        }

//        //  nodes
//        XmlNode Nodes = xmlDoc.CreateElement("Nodes");
//        xmlDoc.AppendChild(Nodes);
//
//        foreach (INode iNode in m_Map.m_listNode)
//        {
//            XmlNode Node = xmlDoc.CreateElement("Node");
//            Node.InnerText = iNode.ToString();
//            Obstacles.AppendChild(Node);
//        }
//
//        //  edges
//        XmlNode Edges = xmlDoc.CreateElement("Edges");
//        xmlDoc.AppendChild(Edges);
//
//        foreach (IEdge iEdge in m_Map.m_listEdge)
//        {
//            XmlNode Edge = xmlDoc.CreateElement("Edge");
//            Edge.InnerText = iEdge.ToString();
//            Obstacles.AppendChild(Edge);
//        }

        string strFilePath = EditorUtility.SaveFilePanel("Save Map", "", "map_" + m_Map.m_nID, "xml");
        if (strFilePath.Length == 0)
        {
            return;
        }

        XmlEditor.Instance.SaveXml(strFilePath, xmlDoc);
    }

    private void SceneGUIEventHandler()
    {
        RaycastHit hitInfo;

        switch(Event.current.type)
        {
            case EventType.MouseDown:
                //  left button
                if (Event.current.button == 0)
                {
                    if (Physics.Raycast(HandleUtility.GUIPointToWorldRay(Event.current.mousePosition), out hitInfo))
                    {
                        m_vec3MouseDownPos = hitInfo.point;
                    }
                }
                //  right button
                else if (Event.current.button == 1)
                {
                    m_State = State.Normal;

                    Repaint();
                }
                break;

            case EventType.MouseUp:
                if (Physics.Raycast(HandleUtility.GUIPointToWorldRay(Event.current.mousePosition), out hitInfo))
                {
                    if (Vector3.Distance(m_vec3MouseDownPos, hitInfo.point) <= MOUSE_CLICK_THRESHOLD)
                    {
                        OnSceneGUIClicked(hitInfo.point);
                    }
                }
                break;
        }
    }

    private void AddSpawnPoint(Vector3 vec3Pos = default(Vector3))
    {
        m_Map.m_listSpawnPoint.Add(vec3Pos);

        m_listSpawnPointsHighlight.Add(false);
    }

    private void RemoveSpawnPoint(int nIndex)
    {
        m_Map.m_listSpawnPoint.RemoveAt(nIndex);

        m_listSpawnPointsHighlight.RemoveAt(nIndex);
    }

    private Polygon AddObstacle()
    {
        Polygon obstacle = new Polygon();

        m_Map.m_listObstacle.Add(obstacle);

        AnimBool animBool = new AnimBool();
        animBool.value = true;
        animBool.valueChanged.AddListener(Repaint);
        animBool.target = true;

        m_listAnimBool.Add(animBool);

        return obstacle;
    }

    private void RemoveObstacle(int nIndex)
    {
        m_Map.m_listObstacle.RemoveAt(nIndex);

        m_listAnimBool.RemoveAt(nIndex);

        if (nIndex == m_ObstacleIndex)
        {
            m_State = State.Normal;

            m_ObstacleIndex = -1;
        }
    }

    private void AddObstacleVertex(int nObstacleIndex, Vector3 vec3Pos = default(Vector3))
    {
        m_Map.m_listObstacle[nObstacleIndex].m_listVertex.Add(vec3Pos);
    }

    private void RemoveObstacleVertex(int nObstacleIndex, int nVertexIndex)
    {
        m_Map.m_listObstacle[nObstacleIndex].m_listVertex.RemoveAt(nVertexIndex);
    }

	#region GUI
    private void OnGUI()
	{
		DrawWindow();
	}

    private void DrawWindow()
	{
        DrawGeneralInfo();

        EditorGUILayout.Space();
        EditorGUILayout.Space();
       
        DrawSpawnPointsUI();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        DrawObstaclesUI();

        EditorGUILayout.Space();

        DrawAdditionalInfo();

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        DrawSaveButton();
	}

    private void DrawGeneralInfo()
    {
        EditorGUILayout.LabelField("[Map General Info]", EditorStyles.boldLabel);
        m_Map.m_nID = EditorGUILayout.IntField("Map ID", m_Map.m_nID);
        m_Map.m_strSceneName = EditorGUILayout.TextField("Map Scene Name", m_Map.m_strSceneName);
        m_Map.m_fWidth = EditorGUILayout.FloatField("Map Width", m_Map.m_fWidth);
        m_Map.m_fHeight = EditorGUILayout.FloatField("Map Height", m_Map.m_fHeight);
    }

    private void DrawSpawnPointsUI()
    {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("[Spawn Points]", EditorStyles.boldLabel, GUILayout.Width(150f));

            if (GUILayout.Button("Add", GUILayout.Width(150f)))
            {
                OnSpawnPointAddClicked();
            }

            if (GUILayout.Button("Add From Click", GUILayout.Width(150f)))
            {
                OnSpawnPointAddFromClickClicked();
            }
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10f);

        m_vec2SpawnPointsScrollPos = EditorGUILayout.BeginScrollView(m_vec2SpawnPointsScrollPos, GUILayout.Height(300f));
        for (int nIndex = 0; nIndex < m_Map.m_listSpawnPoint.Count; ++nIndex)
        {
            DrawSpawnPointItem(nIndex);
        }
        EditorGUILayout.EndScrollView();
    }

    private void DrawObstaclesUI()
    {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("[Obstacles]", EditorStyles.boldLabel, GUILayout.Width(150f));

            if (GUILayout.Button("Add Obstacle", GUILayout.Width(150f)))
            {
                OnObstacleAddClicked();
            }
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10f);

        m_vec2ObstaclesScrollPos = EditorGUILayout.BeginScrollView(m_vec2ObstaclesScrollPos, GUILayout.Height(300f));
        for (int nIndex = 0; nIndex < m_Map.m_listObstacle.Count; ++nIndex)
        {
            DrawObstacleItem(nIndex);
        }
        EditorGUILayout.EndScrollView();
    }

    private void DrawSpawnPointItem(int nIndex)
    {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Position", GUILayout.MaxWidth(50f));

            GUILayout.Space(5f);

            Vector3 vec3OldValue = m_Map.m_listSpawnPoint[nIndex];
            m_Map.m_listSpawnPoint[nIndex] = EditorGUILayout.Vector3Field("", m_Map.m_listSpawnPoint[nIndex]);
            if (vec3OldValue != m_Map.m_listSpawnPoint[nIndex])
            {
                SceneView.RepaintAll();
            }

            GUILayout.Space(10f);

            if (GUILayout.Button("Remove", GUILayout.Width(150f)))
            {
                OnSpawnPointRemoveClicked(nIndex);
            }

            GUILayout.Space(10f);

            if (m_listSpawnPointsHighlight.Count - 1 >= nIndex)
            {
                bool bOldValue = m_listSpawnPointsHighlight[nIndex];
                m_listSpawnPointsHighlight[nIndex] = GUILayout.Toggle(m_listSpawnPointsHighlight[nIndex], "Highlighting", GUILayout.Width(150f));
                if (bOldValue != m_listSpawnPointsHighlight[nIndex])
                {
                    SceneView.RepaintAll();
                }
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawObstacleItem(int nIndex)
    {
        EditorGUILayout.BeginHorizontal();
        {
            GUIStyle style = new GUIStyle();

            if (m_State == State.AddObstacleVertex && nIndex == m_ObstacleIndex)
            {
                style.normal.textColor = Color.yellow;
            }
            else
            {
                style.normal.textColor = Color.black;
            }

            m_listAnimBool[nIndex].target = EditorGUILayout.Foldout(m_listAnimBool[nIndex].target, "Obstacle_" + nIndex.ToString(), style);

            if (GUILayout.Button("Remove Obstacle", GUILayout.Width(150f)))
            {
                OnObstacleRemoveClicked(nIndex);
            }

            if (GUILayout.Button("Add", GUILayout.Width(150f)))
            {
                OnObstacleVertexAddClicked(nIndex);
            }

            if (GUILayout.Button("Add From Click", GUILayout.Width(150f)))
            {
                OnObstacleVertexAddFromClickClicked(nIndex);
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        if (m_listAnimBool.Count - 1 >= nIndex)
        {
            if (EditorGUILayout.BeginFadeGroup(m_listAnimBool[nIndex].faded))
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.BeginVertical(); 
                {
                    for (int nVertexIndex = 0; nVertexIndex < m_Map.m_listObstacle[nIndex].m_listVertex.Count; ++nVertexIndex)
                    {
                        DrawObstacleVertexItem(nIndex, nVertexIndex);
                    }
                }
                EditorGUILayout.EndVertical();

                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFadeGroup();
        }
    }

    private void DrawObstacleVertexItem(int nObstacleIndex, int nVertexIndex)
    {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Position", GUILayout.MaxWidth(65f));

            GUILayout.Space(5f);

            Vector3 vec3OldValue = m_Map.m_listObstacle[nObstacleIndex].m_listVertex[nVertexIndex];
            m_Map.m_listObstacle[nObstacleIndex].m_listVertex[nVertexIndex] = EditorGUILayout.Vector3Field("", m_Map.m_listObstacle[nObstacleIndex].m_listVertex[nVertexIndex]);
            if (vec3OldValue != m_Map.m_listObstacle[nObstacleIndex].m_listVertex[nVertexIndex])
            {
                SceneView.RepaintAll();
            }

            GUILayout.Space(10f);

            if (GUILayout.Button("Remove Vertex", GUILayout.Width(150f)))
            {
                OnObstacleVertexRemoveClicked(nObstacleIndex, nVertexIndex);
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawAdditionalInfo()
    {
        EditorGUILayout.LabelField("[Additional Info]", EditorStyles.boldLabel, GUILayout.Width(150f));

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("State : " + m_State.ToString(), GUILayout.Width(200f));

            if (m_State == State.AddObstacleVertex)
            {
                EditorGUILayout.LabelField("Selected Obstacle Index : " + m_ObstacleIndex.ToString(), GUILayout.Width(200f));
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawSaveButton()
    {
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Space(150f);

            if (GUILayout.Button("Save Map", GUILayout.Width(300f)))
            {
                OnSaveMapButtonClicked();
            }
        }
        EditorGUILayout.EndHorizontal();
    }
	#endregion

    #region SceneGUI
    private void OnSceneGUI()
    {
        DrawGeometry();
    }

    private void DrawGeometry()
    {
        DrawMapBorder();
        DrawObstacleBorders();
        DrawSpawnPoints();
    }

    private void DrawMapBorder()
    {
        if (m_Map.m_fWidth == 0 && m_Map.m_fHeight == 0)
        {
            return;
        }

        Handles.DrawPolyLine(new Vector3[]{new Vector3(m_Map.m_fWidth * -0.5f, 0, m_Map.m_fHeight * 0.5f), new Vector3(m_Map.m_fWidth * 0.5f, 0, m_Map.m_fHeight * 0.5f), 
            new Vector3(m_Map.m_fWidth * 0.5f, 0, m_Map.m_fHeight * -0.5f), new Vector3(m_Map.m_fWidth * -0.5f, 0, m_Map.m_fHeight * -0.5f), new Vector3(m_Map.m_fWidth * -0.5f, 0, m_Map.m_fHeight * 0.5f)});
    }

    private void DrawObstacleBorders()
    {
        foreach (Polygon obstacle in m_Map.m_listObstacle)
        {
            if (obstacle.m_listVertex.Count == 0)
            {
                continue;
            }

            List<Vector3> listVertex = new List<Vector3>(obstacle.m_listVertex);

            //  draw vertex
            for (int nIndex = 0; nIndex < listVertex.Count; ++nIndex)
            {
                Handles.SphereCap(nIndex, listVertex[nIndex], Quaternion.identity, 1);
            }

            //  draw line
            listVertex.Add(obstacle.m_listVertex[0]);

            Handles.DrawPolyLine(listVertex.ToArray());
        }
    }

    private void DrawSpawnPoints()
    {
        for (int nIndex = 0; nIndex < m_Map.m_listSpawnPoint.Count; ++nIndex)
        {
            Color colorOld = Handles.selectedColor;

            if (m_listSpawnPointsHighlight[nIndex])
            {
                Handles.color = Color.yellow;
            }
            else
            {
                Handles.color = Color.white;
            }

            Handles.SphereCap(nIndex, m_Map.m_listSpawnPoint[nIndex], Quaternion.identity, 1);

            Handles.color = colorOld;
        }
    }
    #endregion

	#region Event Handlers
    private void OnSceneGUIHandler(SceneView sceneView)
    {
        //  drawing using handles
        OnSceneGUI();

        //  drawing using gui
        Handles.BeginGUI();
        Handles.EndGUI();

        //  event handling
        SceneGUIEventHandler();
    }

    private void OnSceneGUIClicked(Vector3 vec3Pos)
    {
        if (m_State == State.Normal)
        {
        }
        else if (m_State == State.AddSpawnPoint)
        {
            AddSpawnPoint(vec3Pos);
        }
        else if (m_State == State.AddObstacleVertex)
        {
            AddObstacleVertex(m_ObstacleIndex, vec3Pos);
        }

        Repaint();
    }

    private void OnSaveMapButtonClicked()
    {
        SaveMap();
    }

    private void OnSpawnPointRemoveClicked(int nIndex)
    {
        RemoveSpawnPoint(nIndex);

        SceneView.RepaintAll();
    }

    private void OnSpawnPointAddClicked()
    {
        AddSpawnPoint();

        SceneView.RepaintAll();
    }

    private void OnSpawnPointAddFromClickClicked()
    {
        m_State = State.AddSpawnPoint;
    }

    private void OnObstacleAddClicked()
    {
        AddObstacle();

        SceneView.RepaintAll();
    }

    private void OnObstacleRemoveClicked(int nIndex)
    {
        RemoveObstacle(nIndex);

        SceneView.RepaintAll();
    }

    private void OnObstacleVertexAddClicked(int nObstacleIndex)
    {
        AddObstacleVertex(nObstacleIndex);

        SceneView.RepaintAll();
    }

    private void OnObstacleVertexAddFromClickClicked(int nIndex)
    {
        m_State = State.AddObstacleVertex;

        m_ObstacleIndex = nIndex;
    }

    private void OnObstacleVertexRemoveClicked(int nObstacleIndex, int nVertexIndex)
    {
        RemoveObstacleVertex(nObstacleIndex, nVertexIndex);

        SceneView.RepaintAll();
    }
	#endregion
} 