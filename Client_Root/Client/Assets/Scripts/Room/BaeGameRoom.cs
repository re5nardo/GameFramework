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
    private string m_strIP = "175.197.227.73";
    private int m_nPort = 9111;

    private Dictionary<int, ICharacter>         m_dicCharacter = new Dictionary<int, ICharacter>();

    private int m_nOldFrameRate = 0;

    private int m_nPlayerIndex = -1;

    private bool m_bPlaying = false;
    private float m_fElapsedTime = 0f;

    private List<KeyValuePair<float, IMessage>> m_listGameEventRecord = new List<KeyValuePair<float, IMessage>>();

    private void Start()
    {
        m_nOldFrameRate = Application.targetFrameRate;
        Application.targetFrameRate = 30;

        RoomNetwork.Instance.ConnectToServer(m_strIP, m_nPort, OnConnected, OnRecvMessage);
    }

    private void Update()
    {
        if (m_bPlaying)
            m_fElapsedTime += Time.deltaTime;
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

        m_InputManager.Work(m_MapManager.GetWidth(), m_MapManager.GetHeight(), m_CameraMain, OnClicked);

        foreach (IBehaviorBasedObjectAI obstacle in m_listObstacle)
        {
            obstacle.StartAI();
        }
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

                    m_dicCharacter[kv.Key] = misterBae;

                    misterBae.Initialize(stat);
                    misterBae.m_onBehavioringFinish = delegate()
                    {
                        misterBae.Idle();

                        GameEventIdleToR idleToR = new GameEventIdleToR();
                        idleToR.m_nPlayerIndex = kv.Key;
                        idleToR.m_vec3Pos = misterBae.GetPosition();

                        RoomNetwork.Instance.Send(idleToR);
                    };

                    misterBae.Idle();

                    GameEventIdleToR idleToR2 = new GameEventIdleToR();
                    idleToR2.m_nPlayerIndex = kv.Key;
                    idleToR2.m_vec3Pos = misterBae.GetPosition();

                    RoomNetwork.Instance.Send(idleToR2);

                    if(kv.Key == m_nPlayerIndex)
                        m_CameraController.FollowTarget(misterBae.m_CharacterUI.transform);
                }

                StartCoroutine(PrepareGame());
            }
            else
            {
                Debug.LogError("Enter Room Fail! m_nResult : " + msg.m_nResult);
                SceneManager.LoadScene("Lobby");
            }
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
}
