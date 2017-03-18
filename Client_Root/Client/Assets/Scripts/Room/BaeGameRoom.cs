using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BaeGameRoom : IGameRoom
{
    [SerializeField] private InputManager         m_InputManager = null;
    [SerializeField] private Camera               m_CameraMain = null;
    [SerializeField] private CameraController     m_CameraController;


    //  Temp
    private string m_strIP = "175.197.228.224";
    private int m_nPort = 9111;

    private Dictionary<int, ICharacter>         m_dicCharacter = new Dictionary<int, ICharacter>();
    private Dictionary<int, IEntity>            m_dicEntity = new Dictionary<int, IEntity>();

    private int m_nOldFrameRate = 0;

    private int m_nPlayerIndex = -1;

    private bool m_bPlaying = false;
    private float m_fElapsedTime = 0f;
    private float m_fLastSnapshotTime = 0f;

    private List<KeyValuePair<float, IMessage>> m_listGameEventRecord = new List<KeyValuePair<float, IMessage>>();

    private Dictionary<int, Dictionary<int, List<WorldSnapShotToC>>> m_dicWorldSnapShot = new Dictionary<int, Dictionary<int, List<WorldSnapShotToC>>>();


    private void Start()
    {
        m_nOldFrameRate = Application.targetFrameRate;
        Application.targetFrameRate = 30;

        RoomNetwork.Instance.ConnectToServer(m_strIP, m_nPort, OnConnected, OnRecvMessage);
    }
        
    private IEnumerator Loop()
    {
        while (true)
        {
            UpdateWorld();

            yield return null;

            UpdateElapsedTime();
        }
    }

    private void UpdateElapsedTime()
    {
        if (FindEqualOrFirstSmallerSnapshot(m_fElapsedTime) != null && FindEqualOrFirstBiggerSnapshot(m_fElapsedTime) != null)
        {
            if (m_fLastSnapshotTime - m_fElapsedTime > 0.015f)
            {
                m_fElapsedTime = m_fLastSnapshotTime - 0.015f;
            }
            else
            {
                m_fElapsedTime += Time.deltaTime;
            }
        }
    }

    private void UpdateWorld()
    {
        WorldSnapshotInterpolation();
    }

    private void OnConnected(bool bResult)
    {
        Debug.Log("OnConnected! Result : " + bResult);

        if (!bResult)
        {
            Debug.LogError("OnConnected Fail!");
            SceneManager.LoadScene("Lobby");

            return;
        }

        EnterRoomToR msgToR = new EnterRoomToR ();
        msgToR.m_strPlayerKey = SystemInfo.deviceUniqueIdentifier;
        msgToR.m_nAuthKey = 0;
        msgToR.m_nMatchID = 0;

        RoomNetwork.Instance.Send(msgToR);
    }

    private void StartGame()
    {
        m_bPlaying = true;
        m_fElapsedTime = 0f;
        m_fLastSnapshotTime = 0f;

        m_InputManager.Work(m_MapManager.GetWidth(), m_MapManager.GetHeight(), m_CameraMain, OnClicked);

        foreach (IBehaviorBasedObjectAI obstacle in m_listObstacle)
        {
            obstacle.StartAI();
        }

        StartCoroutine(Loop());
    }

    private IEnumerator PrepareGame()
    {
        //  prefare for game
        yield return StartCoroutine(m_MapManager.LoadMap(1));

        foreach (IBehaviorBasedObjectAI obstacle in m_listObstacle)
        {
            obstacle.Init();
        }

        PreparationStateToR preparationStateToR = new PreparationStateToR();
        preparationStateToR.m_fState = 1.0f;

        RoomNetwork.Instance.Send(preparationStateToR);
    }

#region Network Message Handler
    private void OnRecvMessage(IMessage iMsg)
    {
        if (iMsg.GetID() == EnterRoomToC.MESSAGE_ID)
        {
            EnterRoomToC msg = (EnterRoomToC)iMsg;

            if (msg.m_nResult == 0)
            {
                m_nPlayerIndex = msg.m_nPlayerIndex;

                foreach(KeyValuePair<int, string> kv in msg.m_dicPlayers)
                {
                    GameObject goCharacter = new GameObject("Player_" + kv.Key.ToString());
                    MisterBae misterBae = goCharacter.AddComponent<MisterBae>();

                    Stat stat = new Stat();
                    stat.fSpeed = 4f;

                    m_dicEntity[kv.Key] = misterBae;

                    misterBae.Initialize(stat);

                    if(kv.Key == m_nPlayerIndex)
                        m_CameraController.FollowTarget(misterBae.GetUITransform());
                }

                StartCoroutine(PrepareGame());
            }
            else
            {
                Debug.LogError("Enter Room Fail! m_nResult : " + msg.m_nResult);
                SceneManager.LoadScene("Lobby");
            }
        }
        else if (iMsg.GetID() == GameEventTeleportToC.MESSAGE_ID)
        {
            GameEventTeleportToC msg = (GameEventTeleportToC)iMsg;

            m_dicCharacter[msg.m_nPlayerIndex].GameEvent(iMsg);
        }
        else if (iMsg.GetID() == PlayerEnterRoomToC.MESSAGE_ID)
        {
            
        }
        else if (iMsg.GetID() == GameStartToC.MESSAGE_ID)
        {
            StartGame();
        }
        else if (iMsg.GetID() == GameEventMoveToC.MESSAGE_ID)
        {
            GameEventMoveToC msg = (GameEventMoveToC)iMsg;

            Move(msg.m_nPlayerIndex, msg.m_vec3Dest, msg.m_lEventTime / 1000.0f);
        }
        else if (iMsg.GetID() == GameEventIdleToC.MESSAGE_ID)
        {
            GameEventIdleToC msg = (GameEventIdleToC)iMsg;

            if (!m_dicCharacter.ContainsKey(msg.m_nPlayerIndex))
            {
                Debug.LogWarning("PlayerIndex is invalid!, PlayerIndex : " + msg.m_nPlayerIndex);
                return;
            }

            m_dicCharacter[msg.m_nPlayerIndex].Idle();
        }
        else if (iMsg.GetID() == GameEventStopToC.MESSAGE_ID)
        {
            GameEventStopToC msg = (GameEventStopToC)iMsg;

            if (!m_dicCharacter.ContainsKey(msg.m_nPlayerIndex))
            {
                Debug.LogWarning("PlayerIndex is invalid!, PlayerIndex : " + msg.m_nPlayerIndex);
                return;
            }

            m_dicCharacter[msg.m_nPlayerIndex].Stop();
        }
        else if (iMsg.GetID() == WorldSnapShotToC.MESSAGE_ID)
        {
            WorldSnapShotToC msg = (WorldSnapShotToC)iMsg;

            if(msg.m_fTime > m_fLastSnapshotTime)
                m_fLastSnapshotTime = msg.m_fTime;

            int nSec = (int)msg.m_fTime;
            int n100MilliSec = (int)((msg.m_fTime - nSec) * 10f);

            if (!m_dicWorldSnapShot.ContainsKey(nSec))
                m_dicWorldSnapShot.Add(nSec, new Dictionary<int, List<WorldSnapShotToC>>());

            if(!m_dicWorldSnapShot[nSec].ContainsKey(n100MilliSec))
                m_dicWorldSnapShot[nSec].Add(n100MilliSec, new List<WorldSnapShotToC>());

            m_dicWorldSnapShot[nSec][n100MilliSec].Add(msg);
        }
    }
#endregion

#region Event Handler
    private void OnClicked(Vector3 vec3Pos)
    {
        GameEventMoveToR moveToR = new GameEventMoveToR();
        moveToR.m_nPlayerIndex = m_nPlayerIndex;
        moveToR.m_vec3Dest = vec3Pos;

        RoomNetwork.Instance.Send(moveToR);
    }
#endregion

    private void Move(int nPlayerIndex, Vector3 vec3Pos, float fEventTime)
    {
        if (!m_dicCharacter.ContainsKey(nPlayerIndex))
        {
            Debug.LogError("nPlayerIndex is invalid!, nPlayerIndex : " + nPlayerIndex);
            return;
        }

        LinkedList<Node> listPath = GetMovePath(m_dicCharacter[nPlayerIndex].GetPosition(), vec3Pos);

        if (listPath == null)
        {
            Debug.LogError("Position is invalid!, vec3Pos : " + vec3Pos);
            return;
        }

        m_dicCharacter[nPlayerIndex].Move(listPath, fEventTime);
    }

    private void OnDestroy()
    {
        if (RoomNetwork.GetInstance() != null)
        {
            RoomNetwork.Instance.RemoveConnectHandler(OnConnected);
            RoomNetwork.Instance.RemoveRecvMessageHandler(OnRecvMessage);
        }

        Application.targetFrameRate = m_nOldFrameRate;

        m_InputManager.Stop();
        m_CameraController.StopFollow();
    }

    public override float GetElapsedTime()
    {
        return m_fElapsedTime;
    }

    public override int GetPlayerIndex()
    {
        return m_nPlayerIndex;
    }

    private void WorldSnapshotInterpolation()
    {
        //  find snapshots to interpolate
        WorldSnapShotToC start = FindEqualOrFirstSmallerSnapshot(m_fElapsedTime);
        WorldSnapShotToC end = FindEqualOrFirstBiggerSnapshot(m_fElapsedTime);
        if (start == null || end == null)
        {
            //  Show network unstable ui

            return;
        }

        //  Calc interpolation value
        float fInterpolationValue = 0f;
        if (end.m_fTime - start.m_fTime == 0f)
        {
            fInterpolationValue = 0f;
        }
        else
        {
            fInterpolationValue = (m_fElapsedTime - start.m_fTime) / (end.m_fTime - start.m_fTime);
        }

        //  Behavior
        float fEmptyValue = -1f;
        Dictionary<int, Dictionary<string, KeyValuePair<float, float>>> dicPlayerBehaviors = new Dictionary<int, Dictionary<string, KeyValuePair<float, float>>>();
        foreach (EntityState entityState in start.m_listEntityState)
        {
            if(!dicPlayerBehaviors.ContainsKey(entityState.PlayerIndex))
                dicPlayerBehaviors.Add(entityState.PlayerIndex, new Dictionary<string, KeyValuePair<float, float>>());

            for (int i = 0; i < entityState.BehaviorsMapKeyLength; ++i)
            {
                if (!dicPlayerBehaviors[entityState.PlayerIndex].ContainsKey(entityState.GetBehaviorsMapKey(i).ToString()))
                    dicPlayerBehaviors[entityState.PlayerIndex].Add(entityState.GetBehaviorsMapKey(i).ToString(), new KeyValuePair<float, float>(entityState.GetBehaviorsMapValue(i), fEmptyValue));

                dicPlayerBehaviors[entityState.PlayerIndex][entityState.GetBehaviorsMapKey(i).ToString()] = 
                    new KeyValuePair<float, float>(entityState.GetBehaviorsMapValue(i), dicPlayerBehaviors[entityState.PlayerIndex][entityState.GetBehaviorsMapKey(i).ToString()].Value);
            }
        }

        foreach (EntityState entityState in end.m_listEntityState)
        {
            if(!dicPlayerBehaviors.ContainsKey(entityState.PlayerIndex))
                dicPlayerBehaviors.Add(entityState.PlayerIndex, new Dictionary<string, KeyValuePair<float, float>>());

            for (int i = 0; i < entityState.BehaviorsMapKeyLength; ++i)
            {
                if (!dicPlayerBehaviors[entityState.PlayerIndex].ContainsKey(entityState.GetBehaviorsMapKey(i).ToString()))
                    dicPlayerBehaviors[entityState.PlayerIndex].Add(entityState.GetBehaviorsMapKey(i).ToString(), new KeyValuePair<float, float>(fEmptyValue, entityState.GetBehaviorsMapValue(i)));

                dicPlayerBehaviors[entityState.PlayerIndex][entityState.GetBehaviorsMapKey(i).ToString()] =
                    new KeyValuePair<float, float>(dicPlayerBehaviors[entityState.PlayerIndex][entityState.GetBehaviorsMapKey(i).ToString()].Key, entityState.GetBehaviorsMapValue(i));
            }
        }

        foreach(KeyValuePair<int, Dictionary<string, KeyValuePair<float, float>>> playerBehaviors in dicPlayerBehaviors)
        {
            m_dicEntity[playerBehaviors.Key].SampleBehaviors(playerBehaviors.Value, fInterpolationValue, end.m_fTime - start.m_fTime, fEmptyValue); 
        }

        //  Position
        Dictionary<int, KeyValuePair<Vector3, Vector3>> dicPlayerPosition = new Dictionary<int, KeyValuePair<Vector3, Vector3>>();
        foreach (EntityState entityState in start.m_listEntityState)
        {
            if(!dicPlayerPosition.ContainsKey(entityState.PlayerIndex))
                dicPlayerPosition.Add(entityState.PlayerIndex, new KeyValuePair<Vector3, Vector3>());

            dicPlayerPosition[entityState.PlayerIndex] = new KeyValuePair<Vector3, Vector3>(new Vector3(entityState.Position.X, entityState.Position.Y, entityState.Position.Z), Vector3.zero);
        }

        foreach (EntityState entityState in end.m_listEntityState)
        {
            if(!dicPlayerPosition.ContainsKey(entityState.PlayerIndex))
                dicPlayerPosition.Add(entityState.PlayerIndex, new KeyValuePair<Vector3, Vector3>());

            dicPlayerPosition[entityState.PlayerIndex] = new KeyValuePair<Vector3, Vector3>(dicPlayerPosition[entityState.PlayerIndex].Key, new Vector3(entityState.Position.X, entityState.Position.Y, entityState.Position.Z));
        }
            
        foreach(KeyValuePair<int, KeyValuePair<Vector3, Vector3>> playerPosition in dicPlayerPosition)
        {
            Vector3 vecPosition;

            vecPosition.x = Mathf.Lerp(dicPlayerPosition[playerPosition.Key].Key.x, dicPlayerPosition[playerPosition.Key].Value.x, fInterpolationValue);
            vecPosition.y = Mathf.Lerp(dicPlayerPosition[playerPosition.Key].Key.y, dicPlayerPosition[playerPosition.Key].Value.y, fInterpolationValue);
            vecPosition.z = Mathf.Lerp(dicPlayerPosition[playerPosition.Key].Key.z, dicPlayerPosition[playerPosition.Key].Value.z, fInterpolationValue);

            m_dicEntity[playerPosition.Key].SetPosition(vecPosition);
        }
    }

    private WorldSnapShotToC FindEqualOrFirstBiggerSnapshot(float fTime)
    {
        int nSec = (int)fTime;
        int n100MilliSec = (int)((fTime - nSec) * 10f);
        int nMaxSec = 0;
        foreach(int sec in m_dicWorldSnapShot.Keys)
        {
            if (sec > nMaxSec)
                nMaxSec = sec;
        }
            
        for (int i = nSec; i <= nMaxSec; ++i)
        {
            if (!m_dicWorldSnapShot.ContainsKey(i))
            {
                continue;
            }

            for (int j = (i == nSec ? n100MilliSec : 0); j <= 9; ++j)
            {
                if (!m_dicWorldSnapShot[i].ContainsKey(j))
                    continue;

                List<WorldSnapShotToC> listFound = m_dicWorldSnapShot[i][j].FindAll(a => a.m_fTime >= fTime);
                if (listFound.Count == 0)
                    continue;

                listFound.Sort(delegate(WorldSnapShotToC x, WorldSnapShotToC y)
                {
                    return x.m_fTime.CompareTo(y.m_fTime);
                });

                return listFound[0];
            }
        }

        return null;
    }

    private WorldSnapShotToC FindEqualOrFirstSmallerSnapshot(float fTime)
    {
        int nSec = (int)fTime;
        int n100MilliSec = (int)((fTime - nSec) * 10f);

        for (int i = nSec; i >= 0; --i)
        {
            if (!m_dicWorldSnapShot.ContainsKey(i))
            {
                continue;
            }

            for (int j = (i == nSec ? n100MilliSec : 9); j >= 0; --j)
            {
                if (!m_dicWorldSnapShot[i].ContainsKey(j))
                    continue;

                List<WorldSnapShotToC> listFound = m_dicWorldSnapShot[i][j].FindAll(a => a.m_fTime <= fTime);
                if (listFound.Count == 0)
                    continue;

                listFound.Sort(delegate(WorldSnapShotToC x, WorldSnapShotToC y)
                {
                        return x.m_fTime.CompareTo(y.m_fTime);
                });
                
                return listFound[listFound.Count - 1];
            }
        }

        return null;
    }
}
