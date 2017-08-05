using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BaeGameRoom : IGameRoom
{
    [SerializeField] private InputManager           m_InputManager = null;
    [SerializeField] private Camera                 m_CameraMain = null;
    [SerializeField] private CameraController       m_CameraController = null;
    [SerializeField] private SkillController        m_SkillController = null;

    public static float deltaTime = 0;

    //  Temp
    private string m_strIP = "172.30.1.18";
    private int m_nPort = 9111;

    private Dictionary<int, Entity>            m_dicEntity = new Dictionary<int, Entity>();

    private int m_nOldFrameRate = 0;
    private int m_nPlayerIndex = -1;

    //  Snapshot logic is legacy..
    private float m_fLastSnapshotTime = 0f;
    private Dictionary<int, Dictionary<int, List<WorldSnapShotToC>>> m_dicWorldSnapShot = new Dictionary<int, Dictionary<int, List<WorldSnapShotToC>>>();

    private float m_fLastUpdateTime = -0.001f;  //  include (The reason why default value is -0.001 is for processing events that happen at 0 sec)
    private float m_fElapsedTime = 0;           //  include
    private float m_fLastWorldInfoTime = -1;
    private Dictionary<int, List<IGameEvent>> m_dicGameEvent = new Dictionary<int, List<IGameEvent>>();
    private HashSet<IGameEvent> m_ProcessedGameEvent = new HashSet<IGameEvent>();

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
            if (m_fLastWorldInfoTime == -1)
                yield return null;

            UpdateWorld();

            yield return null;

            UpdateTime();
        }
    }

    private void UpdateWorld()
    {
        if (m_fLastUpdateTime >= m_fElapsedTime)
            return;

        ProcessWorldInfo();

        foreach (KeyValuePair<int, Entity> kv in m_dicEntity)
        {
            kv.Value.Sample();
        }

        m_fLastUpdateTime = m_fElapsedTime;
    }

    private void UpdateTime()
    {
        if (m_fLastWorldInfoTime > m_fElapsedTime)
        {
            if (m_fLastWorldInfoTime > m_fElapsedTime + Time.deltaTime)
            {
                float fPlaySpeed = 1;
                if (m_fLastWorldInfoTime - m_fElapsedTime > 0.5f)
                {
                    fPlaySpeed = 1.5f;
                }
                else if (m_fLastWorldInfoTime - m_fElapsedTime > 0.1f)
                {
                    fPlaySpeed = 1.2f;
                }

                deltaTime = Time.deltaTime * fPlaySpeed;
                if (m_fLastWorldInfoTime < m_fElapsedTime + deltaTime)
                {
                    deltaTime = m_fLastWorldInfoTime - m_fElapsedTime;
                }
            }
            else
            {
                deltaTime = m_fLastWorldInfoTime - m_fElapsedTime;
            }

            m_fElapsedTime += deltaTime;
        }
    }

    private void ProcessWorldInfo()
    {
        for (int sec = (int)m_fLastUpdateTime; sec <= (int)m_fElapsedTime; ++sec)
        {
            if (m_dicGameEvent.ContainsKey(sec))
            {
                foreach(IGameEvent iGameEvent in m_dicGameEvent[sec])
                {
                    if (iGameEvent.m_fEventTime < m_fLastUpdateTime)
                    {
                        continue;
                    }
                    else if (m_fElapsedTime < iGameEvent.m_fEventTime)
                    {
                        return;
                    }
                    else if (m_ProcessedGameEvent.Contains(iGameEvent))
                    {
                        continue;
                    }
                    else
                    {
                        ProcessGameEvent(iGameEvent);
                    }
                }
            }
        }
    }

    private void ProcessGameEvent(IGameEvent iGameEvent)
    {
        if (iGameEvent.GetEventType() == FBS.GameEventType.BehaviorStart)
        {
            GameEvent.BehaviorStart gameEvent = (GameEvent.BehaviorStart)iGameEvent;

            m_dicEntity[gameEvent.m_nEntityID].ProcessGameEvent(gameEvent);
        }
        else if (iGameEvent.GetEventType() == FBS.GameEventType.BehaviorEnd)
        {
            GameEvent.BehaviorEnd gameEvent = (GameEvent.BehaviorEnd)iGameEvent;

            m_dicEntity[gameEvent.m_nEntityID].ProcessGameEvent(gameEvent);
        }
        else if (iGameEvent.GetEventType() == FBS.GameEventType.Position)
        {
            GameEvent.Position gameEvent = (GameEvent.Position)iGameEvent;

            m_dicEntity[gameEvent.m_nEntityID].ProcessGameEvent(gameEvent);
        }
        else if (iGameEvent.GetEventType() == FBS.GameEventType.Rotation)
        {
            GameEvent.Rotation gameEvent = (GameEvent.Rotation)iGameEvent;

            m_dicEntity[gameEvent.m_nEntityID].ProcessGameEvent(gameEvent);
        }
        else if (iGameEvent.GetEventType() == FBS.GameEventType.EntityCreate)
        {
            GameEvent.EntityCreate gameEvent = (GameEvent.EntityCreate)iGameEvent;

            Entity entity = Factory.Instance.CreateEntity(gameEvent.m_EntityType, gameEvent.m_nEntityID, gameEvent.m_nMasterDataID);

            m_dicEntity[gameEvent.m_nEntityID] = entity;
        }

        m_ProcessedGameEvent.Add(iGameEvent);
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
        m_fElapsedTime = 0f;
        m_fLastSnapshotTime = 0f;
        deltaTime = 0;
        m_fLastUpdateTime = -0.001f;
        m_fLastWorldInfoTime = -1;
        m_dicGameEvent.Clear();
        m_ProcessedGameEvent.Clear();

        m_InputManager.Work(100, 500/*temp.. always 200, 200*/, m_CameraMain, OnClicked);

        StartCoroutine(Loop());
    }

    private IEnumerator PrepareGame()
    {
        //  prefare for game
        yield return SceneManager.LoadSceneAsync("BasicStraightWay"/*temp.. always TestMap*/, LoadSceneMode.Additive);

        PreparationStateToR preparationStateToR = new PreparationStateToR();
        preparationStateToR.m_fState = 1.0f;

        RoomNetwork.Instance.Send(preparationStateToR);
    }

#region Network Message Handler
    private void OnRecvMessage(IMessage iMsg)
    {
        if (iMsg.GetID() == EnterRoomToC.MESSAGE_ID)
        {
            OnEnterRoomToC((EnterRoomToC)iMsg);
        }
        else if (iMsg.GetID() == PlayerEnterRoomToC.MESSAGE_ID)
        {
            
        }
        else if (iMsg.GetID() == GameStartToC.MESSAGE_ID)
        {
            OnGameStartToC((GameStartToC)iMsg);
        }
        else if (iMsg.GetID() == WorldSnapShotToC.MESSAGE_ID)
        {
            OnWorldSnapShotToC((WorldSnapShotToC)iMsg);
        }
        else if (iMsg.GetID() == WorldInfoToC.MESSAGE_ID)
        {
            OnWorldInfoToC((WorldInfoToC)iMsg);
        }
    }

    private void OnEnterRoomToC(EnterRoomToC msg)
    {
        if (msg.m_nResult == 0)
        {
            m_nPlayerIndex = msg.m_nPlayerIndex;

            foreach(KeyValuePair<int, string> kv in msg.m_dicPlayers)
            {
                //  temp.. MisterBae
                Entity entity = Factory.Instance.CreateEntity(FBS.Data.EntityType.Character, 0, 0);
                m_dicEntity[kv.Key] = entity;
                //entity.Initialize();

                if (kv.Key == m_nPlayerIndex)
                {
                    m_CameraController.SetTarget(entity.GetUITransform());
                    m_CameraController.StartFollowTarget();

                    m_SkillController.SetSkills(new List<int>(){0, 1, 2});
                }
            }

            StartCoroutine(PrepareGame());
        }
        else
        {
            Debug.LogError("Enter Room Fail! m_nResult : " + msg.m_nResult);
            SceneManager.LoadScene("Lobby");
        }
    }

    private void OnGameStartToC(GameStartToC msg)
    {
        StartGame();
    }

    private void OnWorldSnapShotToC(WorldSnapShotToC msg)
    {
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

    private void OnWorldInfoToC(WorldInfoToC msg)
    {
        m_fLastWorldInfoTime = msg.m_fEndTime;

        foreach(IGameEvent iGameEvent in msg.m_listGameEvent)
        {
            int nSec = (int)iGameEvent.m_fEventTime;

            if (!m_dicGameEvent.ContainsKey(nSec))
                m_dicGameEvent.Add(nSec, new List<IGameEvent>());
            
            m_dicGameEvent[nSec].Add(iGameEvent);
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

    private void OnDestroy()
    {
        if (RoomNetwork.GetInstance() != null)
        {
            RoomNetwork.Instance.RemoveConnectHandler(OnConnected);
            RoomNetwork.Instance.RemoveRecvMessageHandler(OnRecvMessage);
        }

        Application.targetFrameRate = m_nOldFrameRate;
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
        foreach (FBS.EntityState entityState in start.m_listEntityState)
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

        foreach (FBS.EntityState entityState in end.m_listEntityState)
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
        foreach (FBS.EntityState entityState in start.m_listEntityState)
        {
            if(!dicPlayerPosition.ContainsKey(entityState.PlayerIndex))
                dicPlayerPosition.Add(entityState.PlayerIndex, new KeyValuePair<Vector3, Vector3>());

            dicPlayerPosition[entityState.PlayerIndex] = new KeyValuePair<Vector3, Vector3>(new Vector3(entityState.Position.X, entityState.Position.Y, entityState.Position.Z), Vector3.zero);
        }

        foreach (FBS.EntityState entityState in end.m_listEntityState)
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

        //  Rotation
        Dictionary<int, KeyValuePair<Vector3, Vector3>> dicPlayerRotation = new Dictionary<int, KeyValuePair<Vector3, Vector3>>();
        foreach (FBS.EntityState entityState in start.m_listEntityState)
        {
            if(!dicPlayerRotation.ContainsKey(entityState.PlayerIndex))
                dicPlayerRotation.Add(entityState.PlayerIndex, new KeyValuePair<Vector3, Vector3>());

            dicPlayerRotation[entityState.PlayerIndex] = new KeyValuePair<Vector3, Vector3>(new Vector3(entityState.Rotation.X, entityState.Rotation.Y, entityState.Rotation.Z), Vector3.zero);
        }

        foreach (FBS.EntityState entityState in end.m_listEntityState)
        {
            if(!dicPlayerRotation.ContainsKey(entityState.PlayerIndex))
                dicPlayerRotation.Add(entityState.PlayerIndex, new KeyValuePair<Vector3, Vector3>());

            dicPlayerRotation[entityState.PlayerIndex] = new KeyValuePair<Vector3, Vector3>(dicPlayerRotation[entityState.PlayerIndex].Key, new Vector3(entityState.Rotation.X, entityState.Rotation.Y, entityState.Rotation.Z));
        }

        foreach(KeyValuePair<int, KeyValuePair<Vector3, Vector3>> playerRotation in dicPlayerRotation)
        {
            Vector3 vec3PrevRotation = dicPlayerRotation[playerRotation.Key].Key;
            Vector3 vec3NextRotation = dicPlayerRotation[playerRotation.Key].Value;

            if (vec3NextRotation.y < vec3PrevRotation.y)
            {
                vec3NextRotation.y += 360;
            }

            //  For lerp calculation
            //  Clockwise rotation
            if (vec3NextRotation.y - vec3PrevRotation.y <= 180)
            {
                if (vec3PrevRotation.y > vec3NextRotation.y)
                    vec3NextRotation.y += 360;
            }
            //  CounterClockwise rotation
            else
            {
                if (vec3PrevRotation.y < vec3NextRotation.y)
                    vec3PrevRotation.y += 360;
            }

            Vector3 vecRotation;
            vecRotation.x = Mathf.Lerp(dicPlayerRotation[playerRotation.Key].Key.x, dicPlayerRotation[playerRotation.Key].Value.x, fInterpolationValue);
            vecRotation.y = Mathf.Lerp(vec3PrevRotation.y, vec3NextRotation.y, fInterpolationValue);
            vecRotation.z = Mathf.Lerp(dicPlayerRotation[playerRotation.Key].Key.z, dicPlayerRotation[playerRotation.Key].Value.z, fInterpolationValue);

            m_dicEntity[playerRotation.Key].SetRotation(vecRotation);
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
