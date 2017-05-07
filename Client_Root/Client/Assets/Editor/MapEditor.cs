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
        AddCheckPoint,
        AddSpawnPoint,
        AddConvexPolygon2dVertex,
    }

    private const float MOUSE_CLICK_THRESHOLD = 1;


    private Vector3 m_vec3MouseDownPos = Vector3.zero;

    private Vector2 m_vec2MainScrollPos = Vector2.zero;
    private Vector2 m_vec2CheckPointsScrollPos = Vector2.zero;
    private Vector2 m_vec2SpawnPointsScrollPos = Vector2.zero;
    private Vector2 m_vec2TerrainObjectsScrollPos = Vector2.zero;

    private List<bool> m_listSpawnPointsHighlight = new List<bool>();
    private List<AnimBool> m_listAnimBool_TerrainObject = new List<AnimBool>();

    private int m_ConvexPolygon2dIndex = -1;

    private TerrainObject.ShapeType m_ShapeTypeSelected = TerrainObject.ShapeType.Box2d;

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
        string[] arrRect = StartArea.InnerText.Split('/');
        if (arrRect.Length != 3)
        {
            Debug.LogWarning("arrRect.Length is not 3!");
            return;
        }

        map.m_rectStartArea = new Rect3D(arrRect[0].ToVector3(), float.Parse(arrRect[1]), float.Parse(arrRect[2]));

        //  Check points
        XmlNode CheckPoints = Map.SelectSingleNode("CheckPoints");
        XmlNodeList listCheckPoint = CheckPoints.SelectNodes("CheckPoint");
        foreach (XmlNode CheckPoint in listCheckPoint)
        {
            AddCheckPoint(CheckPoint.InnerText.ToVector3());
        }

        //  Spawn points
        XmlNode SpawnPoints = Map.SelectSingleNode("SpawnPoints");
        XmlNodeList listSpawnPoint = SpawnPoints.SelectNodes("SpawnPoint");
        foreach (XmlNode SpawnPoint in listSpawnPoint)
        {
            AddSpawnPoint(SpawnPoint.InnerText.ToVector3());
        }

        //  TerrainObjects
        XmlNode TerrainObjects = Map.SelectSingleNode("TerrainObjects");
        XmlNodeList listTerrainObject = TerrainObjects.SelectNodes("TerrainObject");
        foreach (XmlNode TerrainObject in listTerrainObject)
        {
            string strShape = TerrainObject.InnerText.Split('/')[0];

            TerrainObject terrainObject = CreateTerrainObject(strShape);
            terrainObject.SetData(TerrainObject.InnerText);

            AddTerrainObject(terrainObject);
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

        //  start area
        XmlNode StartArea = xmlDoc.CreateElement("StartArea");
        StartArea.InnerText = m_Map.m_rectStartArea.center.ToString() + '/' + m_Map.m_rectStartArea.width.ToString() + '/' + m_Map.m_rectStartArea.height.ToString();
        Map.AppendChild(StartArea);

        //  check points
        XmlNode CheckPoints = xmlDoc.CreateElement("CheckPoints");
        Map.AppendChild(CheckPoints);

        foreach (Vector3 vec3CheckPoint in m_Map.m_listCheckPoint)
        {
            XmlNode CheckPoint = xmlDoc.CreateElement("CheckPoint");
            CheckPoint.InnerText = vec3CheckPoint.ToString();
            CheckPoints.AppendChild(CheckPoint);
        }

        //  spawn points
        XmlNode SpawnPoints = xmlDoc.CreateElement("SpawnPoints");
        Map.AppendChild(SpawnPoints);

        foreach (Vector3 vec3SpawnPoint in m_Map.m_listSpawnPoint)
        {
            XmlNode SpawnPoint = xmlDoc.CreateElement("SpawnPoint");
            SpawnPoint.InnerText = vec3SpawnPoint.ToString();
            SpawnPoints.AppendChild(SpawnPoint);
        }

		//	terrain objects
		XmlNode TerrainObjectsNode = xmlDoc.CreateElement("TerrainObjects");
		Map.AppendChild(TerrainObjectsNode);

		foreach (TerrainObject terrainObject in m_Map.m_listTerrainObject)
        {
			XmlNode terrainObjectNode = xmlDoc.CreateElement("TerrainObject");
			terrainObjectNode.InnerText = terrainObject.ToString();

			TerrainObjectsNode.AppendChild(terrainObjectNode);
        }

        string strFilePath = EditorUtility.SaveFilePanel("Save Map", "", "map_" + m_Map.m_nID, "xml");
        if (strFilePath.Length == 0)
        {
            return;
        }

        XmlEditor.Instance.SaveXml(strFilePath, xmlDoc);
    }

    private TerrainObject CreateTerrainObject(string strShape)
    {
        if (strShape == TerrainObject.ShapeType.Box2d.ToString())
        {
            return new Box2dShapeTerrainObject();
        }
        else if (strShape == TerrainObject.ShapeType.Sphere2d.ToString())
        {
            return new Sphere2dShapeTerrainObject();
        }
        else if (strShape == TerrainObject.ShapeType.ConvexPolygon2d.ToString())
        {
            return new ConvexPolygon2dShapeTerrainObject();
        }

        Debug.LogWarning("strShape is invalid!, strShape : " + strShape);
        return null;
    }

    private TerrainObject CreateTerrainObject(TerrainObject.ShapeType shapeType)
    {
        if (shapeType == TerrainObject.ShapeType.Box2d)
        {
            return new Box2dShapeTerrainObject();
        }
        else if (shapeType == TerrainObject.ShapeType.Sphere2d)
        {
            return new Sphere2dShapeTerrainObject();
        }
        else if (shapeType == TerrainObject.ShapeType.ConvexPolygon2d)
        {
            return new ConvexPolygon2dShapeTerrainObject();
        }

        Debug.LogWarning("strShape is invalid!, strShape : " + shapeType.ToString());
        return null;
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

    private void AddCheckPoint(Vector3 vec3Pos = default(Vector3))
    {
        m_Map.m_listCheckPoint.Add(vec3Pos);
    }

    private void RemoveCheckPoint(int nIndex)
    {
        m_Map.m_listCheckPoint.RemoveAt(nIndex);
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

    private TerrainObject AddTerrainObject(TerrainObject terrainObject)
	{
        m_Map.m_listTerrainObject.Add(terrainObject);

        AnimBool animBool = new AnimBool();
        animBool.value = true;
        animBool.valueChanged.AddListener(Repaint);
        animBool.target = true;

        m_listAnimBool_TerrainObject.Add(animBool);

        return terrainObject;
	}

	private void RemoveTerrainObject(int nIndex)
	{
        m_Map.m_listTerrainObject.RemoveAt(nIndex);

        m_listAnimBool_TerrainObject.RemoveAt(nIndex);

        if (nIndex == m_ConvexPolygon2dIndex)
        {
            m_State = State.Normal;

            m_ConvexPolygon2dIndex = -1;
        }
	}

    private void AddConvexPolygon2dVertex(int nTerrainObjectIndex, Vector3 vec3Pos = default(Vector3))
    {
        ConvexPolygon2dShapeTerrainObject terrainObject = (ConvexPolygon2dShapeTerrainObject)m_Map.m_listTerrainObject[nTerrainObjectIndex];

        terrainObject.AddVertex(vec3Pos);
    }

    private void RemoveConvexPolygon2dVertex(int nTerrainObjectIndex, int nVertexIndex)
    {
        ConvexPolygon2dShapeTerrainObject terrainObject = (ConvexPolygon2dShapeTerrainObject)m_Map.m_listTerrainObject[nTerrainObjectIndex];

        terrainObject.RemoveVertex(nVertexIndex);
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

        DrawStartArea();

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        m_vec2MainScrollPos = EditorGUILayout.BeginScrollView(m_vec2MainScrollPos, GUILayout.Height(600f));

        DrawCheckPointsUI();

        EditorGUILayout.Space();
        EditorGUILayout.Space();
       
        DrawSpawnPointsUI();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

		DrawTerrainObjectsUI();

        EditorGUILayout.EndScrollView();

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

    private void DrawStartArea()
    {
        EditorGUILayout.LabelField("[Start Area]", EditorStyles.boldLabel);
        m_Map.m_rectStartArea.center = EditorGUILayout.Vector3Field("Start Area Center", m_Map.m_rectStartArea.center);
        m_Map.m_rectStartArea.width = EditorGUILayout.FloatField("Start Area Width", m_Map.m_rectStartArea.width);
        m_Map.m_rectStartArea.height = EditorGUILayout.FloatField("Start Area Height", m_Map.m_rectStartArea.height);
    }

    private void DrawCheckPointsUI()
    {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("[Check Points]", EditorStyles.boldLabel, GUILayout.Width(150f));

            if (GUILayout.Button("Add", GUILayout.Width(150f)))
            {
                OnCheckPointAddClicked();
            }

            if (GUILayout.Button("Add From Click", GUILayout.Width(150f)))
            {
                OnCheckPointAddFromClickClicked();
            }
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10f);

        m_vec2CheckPointsScrollPos = EditorGUILayout.BeginScrollView(m_vec2CheckPointsScrollPos, GUILayout.Height(300f));
        for (int nIndex = 0; nIndex < m_Map.m_listCheckPoint.Count; ++nIndex)
        {
            DrawCheckPointItem(nIndex);
        }
        EditorGUILayout.EndScrollView();
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

	private void DrawTerrainObjectsUI()
	{
        EditorGUILayout.BeginHorizontal();
        {
			EditorGUILayout.LabelField("[TerrainObjects]", EditorStyles.boldLabel, GUILayout.Width(150f));

            if (GUILayout.Button("Add TerrainObject", GUILayout.Width(150f)))
            {
                OnTerrainObjectAddClicked();
            }

            m_ShapeTypeSelected = (TerrainObject.ShapeType)EditorGUILayout.EnumPopup("ShapeType to add:", m_ShapeTypeSelected, GUILayout.Width(300f));
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10f);

        m_vec2TerrainObjectsScrollPos = EditorGUILayout.BeginScrollView(m_vec2TerrainObjectsScrollPos, GUILayout.Height(300f));
		for (int nIndex = 0; nIndex < m_Map.m_listTerrainObject.Count; ++nIndex)
        {
            DrawTerrainObjectUI(nIndex);
        }
        EditorGUILayout.EndScrollView();
	}

    private void DrawCheckPointItem(int nIndex)
    {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Position", GUILayout.MaxWidth(50f));

            GUILayout.Space(5f);

            Vector3 vec3OldValue = m_Map.m_listCheckPoint[nIndex];
            m_Map.m_listCheckPoint[nIndex] = EditorGUILayout.Vector3Field("", m_Map.m_listCheckPoint[nIndex]);
            if (vec3OldValue != m_Map.m_listCheckPoint[nIndex])
            {
                SceneView.RepaintAll();
            }

            GUILayout.Space(10f);

            if (GUILayout.Button("Remove", GUILayout.Width(150f)))
            {
                OnCheckPointRemoveClicked(nIndex);
            }

            GUILayout.Space(10f);
        }
        EditorGUILayout.EndHorizontal();
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

	private void DrawTerrainObjectUI(int nIndex)
	{
        EditorGUILayout.BeginHorizontal();
        {
            GUIStyle style = new GUIStyle();

            if (m_State == State.AddConvexPolygon2dVertex && nIndex == m_ConvexPolygon2dIndex)
            {
                style.normal.textColor = Color.yellow;
            }
            else
            {
                style.normal.textColor = Color.black;
            }

            string strTitle = string.Format("TerrainObject_{0} ({1})", nIndex, m_Map.m_listTerrainObject[nIndex].GetShapeType().ToString());
            m_listAnimBool_TerrainObject[nIndex].target = EditorGUILayout.Foldout(m_listAnimBool_TerrainObject[nIndex].target, strTitle, style);

            if (GUILayout.Button("Remove TerrainObject", GUILayout.Width(150f)))
            {
                OnTerrainObjectRemoveClicked(nIndex);
            }

            if (nIndex < m_Map.m_listTerrainObject.Count && m_Map.m_listTerrainObject[nIndex].GetShapeType() == TerrainObject.ShapeType.ConvexPolygon2d)
            {
                if (GUILayout.Button("Add", GUILayout.Width(150f)))
                {
                    OnConvexPolygon2dVertexAddClicked(nIndex);
                }

                if (GUILayout.Button("Add From Click", GUILayout.Width(150f)))
                {
                    OnConvexPolygon2dVertexAddFromClickClicked(nIndex);
                }
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        //  TerrainObject may be removed at OnTerrainObjectRemoveClicked above
        if (nIndex >= m_Map.m_listTerrainObject.Count)   
        {
            return;
        }

        if (EditorGUILayout.BeginFadeGroup(m_listAnimBool_TerrainObject[nIndex].faded))
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.BeginVertical();
            {
                TerrainObject.ShapeType shapeType = m_Map.m_listTerrainObject[nIndex].GetShapeType();
                if (shapeType == TerrainObject.ShapeType.Box2d) DrawBox2dShapeTerrainObjectUI(nIndex);
                else if (shapeType == TerrainObject.ShapeType.Sphere2d) DrawSphere2dShapeTerrainObjectUI(nIndex);
                else if (shapeType == TerrainObject.ShapeType.ConvexPolygon2d) DrawConvexPolygon2dShapeTerrainObjectUI(nIndex);
            }
            EditorGUILayout.EndVertical();

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFadeGroup();
	}

    private void DrawBox2dShapeTerrainObjectUI(int nIndex)
    {
        Box2dShapeTerrainObject terrainObject = (Box2dShapeTerrainObject)m_Map.m_listTerrainObject[nIndex];

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Position", GUILayout.MaxWidth(65f));

            GUILayout.Space(5f);

            Vector3 vec3OldValue = terrainObject.GetPosition();
            terrainObject.SetPosition(EditorGUILayout.Vector3Field("", vec3OldValue));
            if (vec3OldValue != terrainObject.GetPosition())
            {
                SceneView.RepaintAll();
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Rotation", GUILayout.MaxWidth(65f));

            GUILayout.Space(5f);

            Vector3 vec3OldValue = terrainObject.GetRotation();
            terrainObject.SetRotation(EditorGUILayout.Vector3Field("", vec3OldValue));
            if (vec3OldValue != terrainObject.GetRotation())
            {
                SceneView.RepaintAll();
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("HalfExtents", GUILayout.MaxWidth(65f));

            GUILayout.Space(5f);

            Vector3 vec3OldValue = terrainObject.GetHalfExtents();
            terrainObject.SetHalfExtents(EditorGUILayout.Vector3Field("", vec3OldValue));
            if (vec3OldValue != terrainObject.GetHalfExtents())
            {
                SceneView.RepaintAll();
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawSphere2dShapeTerrainObjectUI(int nIndex)
    {
        Sphere2dShapeTerrainObject terrainObject = (Sphere2dShapeTerrainObject)m_Map.m_listTerrainObject[nIndex];

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Position", GUILayout.MaxWidth(65f));

            GUILayout.Space(5f);

            Vector3 vec3OldValue = terrainObject.GetPosition();
            terrainObject.SetPosition(EditorGUILayout.Vector3Field("", vec3OldValue));
            if (vec3OldValue != terrainObject.GetPosition())
            {
                SceneView.RepaintAll();
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Radius", GUILayout.MaxWidth(65f));

            GUILayout.Space(5f);

            float fOldValue = terrainObject.GetRadius();
            terrainObject.SetRadius(EditorGUILayout.FloatField("", fOldValue));
            if (fOldValue != terrainObject.GetRadius())
            {
                SceneView.RepaintAll();
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawConvexPolygon2dShapeTerrainObjectUI(int nIndex)
    {
        ConvexPolygon2dShapeTerrainObject terrainObject = (ConvexPolygon2dShapeTerrainObject)m_Map.m_listTerrainObject[nIndex];
        List<Vector3> listVertex = terrainObject.GetVertices();

        for (int nVertexIndex = 0; nVertexIndex < listVertex.Count; ++nVertexIndex)
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Position", GUILayout.MaxWidth(65f));

                GUILayout.Space(5f);

                Vector3 vec3OldValue = listVertex[nVertexIndex];
                terrainObject.SetVertex(nVertexIndex, EditorGUILayout.Vector3Field("", vec3OldValue));
                if (vec3OldValue != terrainObject.GetVertex(nVertexIndex))
                {
                    SceneView.RepaintAll();
                }

                GUILayout.Space(10f);

                if (GUILayout.Button("Remove Vertex", GUILayout.Width(150f)))
                {
                    OnConvexPolygon2dVertexRemoveClicked(nIndex, nVertexIndex);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    private void DrawAdditionalInfo()
    {
        EditorGUILayout.LabelField("[Additional Info]", EditorStyles.boldLabel, GUILayout.Width(150f));

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("State : " + m_State.ToString(), GUILayout.Width(250f));

            if (m_State == State.AddConvexPolygon2dVertex)
            {
                EditorGUILayout.LabelField("Selected ConvexPolygon2d Index : " + m_ConvexPolygon2dIndex.ToString(), GUILayout.Width(300f));
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
        DrawStartAreaBorder();
        DrawCheckPoints();
        DrawSpawnPoints();
        DrawTerrainObjects();
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

    private void DrawStartAreaBorder()
    {
        if (m_Map.m_rectStartArea.width == 0 && m_Map.m_rectStartArea.height == 0)
        {
            return;
        }

        Vector3 vec3TopLeft = new Vector3(m_Map.m_rectStartArea.width * -0.5f + m_Map.m_rectStartArea.center.x, 0, m_Map.m_rectStartArea.height * 0.5f + m_Map.m_rectStartArea.center.z);
        Vector3 vec3TopRight = new Vector3(m_Map.m_rectStartArea.width * 0.5f + m_Map.m_rectStartArea.center.x, 0, m_Map.m_rectStartArea.height * 0.5f + m_Map.m_rectStartArea.center.z);
        Vector3 vec3BottomRight = new Vector3(m_Map.m_rectStartArea.width * 0.5f + m_Map.m_rectStartArea.center.x, 0, m_Map.m_rectStartArea.height * -0.5f + m_Map.m_rectStartArea.center.z);
        Vector3 vec3BottomLeft = new Vector3(m_Map.m_rectStartArea.width * -0.5f + m_Map.m_rectStartArea.center.x, 0, m_Map.m_rectStartArea.height * -0.5f + m_Map.m_rectStartArea.center.z);
       
        Color colorOld = Handles.color;

        Handles.color = Color.red;

        Handles.DrawPolyLine(new Vector3[]{vec3TopLeft, vec3TopRight, vec3BottomRight, vec3BottomLeft, vec3TopLeft});

        Handles.color = colorOld;
    }

    private void DrawCheckPoints()
    {
        for (int nIndex = 0; nIndex < m_Map.m_listCheckPoint.Count; ++nIndex)
        {
            Color colorOld = Handles.color;

            Handles.color = Color.green;

            Handles.SphereCap(nIndex, m_Map.m_listCheckPoint[nIndex], Quaternion.identity, 1);

            Handles.color = colorOld;
        }
    }

    private void DrawSpawnPoints()
    {
        for (int nIndex = 0; nIndex < m_Map.m_listSpawnPoint.Count; ++nIndex)
        {
            Color colorOld = Handles.color;

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

    private void DrawTerrainObjects()
    {
        for (int nIndex = 0; nIndex < m_Map.m_listTerrainObject.Count; ++nIndex)
        {
            TerrainObject.ShapeType shapeType = m_Map.m_listTerrainObject[nIndex].GetShapeType();
            if (shapeType == TerrainObject.ShapeType.Box2d) DrawBox2dShapeTerrainObject(nIndex);
            else if (shapeType == TerrainObject.ShapeType.Sphere2d) DrawSphere2dShapeTerrainObject(nIndex);
            else if (shapeType == TerrainObject.ShapeType.ConvexPolygon2d) DrawConvexPolygon2dShapeTerrainObject(nIndex);
        }
    }

    private void DrawBox2dShapeTerrainObject(int nIndex)
    {
        Box2dShapeTerrainObject terrainObject = (Box2dShapeTerrainObject)m_Map.m_listTerrainObject[nIndex];

        Vector3 pos = terrainObject.GetPosition();
        Vector3 rot = terrainObject.GetRotation();
        Vector3 half = terrainObject.GetHalfExtents();

        Vector3 p1 = new Vector3(-half.x, 0, -half.z);
        Vector3 p2 = new Vector3(half.x, 0, -half.z);
        Vector3 p3 = new Vector3(half.x, 0, half.z);
        Vector3 p4 = new Vector3(-half.x, 0, half.z);

        p1 = Quaternion.Euler(0, rot.y, 0) * p1 + pos;
        p2 = Quaternion.Euler(0, rot.y, 0) * p2 + pos;
        p3 = Quaternion.Euler(0, rot.y, 0) * p3 + pos;
        p4 = Quaternion.Euler(0, rot.y, 0) * p4 + pos;

        Handles.SphereCap(0, p1, Quaternion.identity, 1);
        Handles.SphereCap(0, p2, Quaternion.identity, 1);
        Handles.SphereCap(0, p3, Quaternion.identity, 1);
        Handles.SphereCap(0, p4, Quaternion.identity, 1);

        Handles.DrawLine(p1, p2);
        Handles.DrawLine(p2, p3);
        Handles.DrawLine(p3, p4);
        Handles.DrawLine(p4, p1);
    }

    private void DrawSphere2dShapeTerrainObject(int nIndex)
    {
        Sphere2dShapeTerrainObject terrainObject = (Sphere2dShapeTerrainObject)m_Map.m_listTerrainObject[nIndex];

        Vector3 pos = terrainObject.GetPosition();
        float fRadius = terrainObject.GetRadius();

        Vector3 p1 = new Vector3(pos.x - fRadius, 0, pos.z);
        Vector3 p2 = new Vector3(pos.x, 0, pos.z - fRadius);
        Vector3 p3 = new Vector3(pos.x + fRadius, 0, pos.z);
        Vector3 p4 = new Vector3(pos.x, 0, pos.z + fRadius);

        Handles.SphereCap(0, p1, Quaternion.identity, 1);
        Handles.SphereCap(0, p2, Quaternion.identity, 1);
        Handles.SphereCap(0, p3, Quaternion.identity, 1);
        Handles.SphereCap(0, p4, Quaternion.identity, 1);

        Handles.DrawWireDisc(terrainObject.GetPosition(), Vector3.up, fRadius);
    }

    private void DrawConvexPolygon2dShapeTerrainObject(int nIndex)
    {
        ConvexPolygon2dShapeTerrainObject terrainObject = (ConvexPolygon2dShapeTerrainObject)m_Map.m_listTerrainObject[nIndex];

        List<Vector3> listVertex = new List<Vector3>(terrainObject.GetVertices());
        if (listVertex.Count == 0)
        {
            return;
        }

        foreach(Vector3 vertex in listVertex)
        {
            Handles.SphereCap(0, vertex, Quaternion.identity, 1);
        }

        listVertex.Add(listVertex[0]);
            
        Handles.DrawPolyLine(listVertex.ToArray());
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
        else if (m_State == State.AddCheckPoint)
        {
            AddCheckPoint(vec3Pos);
        }
        else if (m_State == State.AddSpawnPoint)
        {
            AddSpawnPoint(vec3Pos);
        }
        else if (m_State == State.AddConvexPolygon2dVertex)
        {
            AddConvexPolygon2dVertex(m_ConvexPolygon2dIndex, vec3Pos);
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

    private void OnTerrainObjectAddClicked()
    {
        AddTerrainObject(CreateTerrainObject(m_ShapeTypeSelected));

        SceneView.RepaintAll();
    }

    private void OnTerrainObjectRemoveClicked(int nIndex)
    {
        RemoveTerrainObject(nIndex);

        SceneView.RepaintAll();
    }

    private void OnConvexPolygon2dVertexAddClicked(int nTerrainObjectIndex)
    {
        AddConvexPolygon2dVertex(nTerrainObjectIndex);

        SceneView.RepaintAll();
    }

    private void OnConvexPolygon2dVertexAddFromClickClicked(int nIndex)
    {
        m_State = State.AddConvexPolygon2dVertex;

        m_ConvexPolygon2dIndex = nIndex;
    }

    private void OnConvexPolygon2dVertexRemoveClicked(int nTerrainObjectIndex, int nVertexIndex)
    {
        RemoveConvexPolygon2dVertex(nTerrainObjectIndex, nVertexIndex);

        SceneView.RepaintAll();
    }

    private void OnCheckPointAddClicked()
    {
        AddCheckPoint();

        SceneView.RepaintAll();
    }

    private void OnCheckPointAddFromClickClicked()
    {
        m_State = State.AddCheckPoint;
    }

    private void OnCheckPointRemoveClicked(int nIndex)
    {
        RemoveCheckPoint(nIndex);

        SceneView.RepaintAll();
    }
	#endregion
} 