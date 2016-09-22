using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BaeGameRoom : MonoBehaviour
{
    [SerializeField] private MapManager           m_MapManager = null;
    [SerializeField] private InputManager         m_InputManager = null;
    [SerializeField] private Camera               m_CameraMain = null;
    [SerializeField] private CameraController     m_CameraController;

    //  Temp
    private string m_strIP = "175.197.228.153";
    private int m_nPort = 9111;

    private AStarAlgorithm                      m_AStarAlgorithm = new AStarAlgorithm();
    private MisterBae                           m_MisterBae = null;

    private Dictionary<int, ICharacter>         m_dicCharacter = new Dictionary<int, ICharacter>();

    private int m_nOldFrameRate = 0;

    private int m_nPlayerIndex = -1;

    private void Start()
    {
        m_nOldFrameRate = Application.targetFrameRate;
        Application.targetFrameRate = 30;

        RoomNetwork.Instance.ConnectToServer(m_strIP, m_nPort, OnConnected, OnRecvMessage);
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
        StartCoroutine(PrepareGame());
    }

    private IEnumerator PrepareGame()
    {
        //  prefare for game
        yield return StartCoroutine(m_MapManager.LoadMap(1));

        m_InputManager.Work(m_MapManager.GetWidth(), m_MapManager.GetHeight(), m_CameraMain, OnClicked);

        //  temp
        GameObject goCharacter = new GameObject("MisterBae");
        m_MisterBae = goCharacter.AddComponent<MisterBae>();

        Stat stat = new Stat();
        stat.fSpeed = 4f;

        m_MisterBae.Initialize(stat); 

        m_CameraController.FollowTarget(m_MisterBae.m_CharacterUI.transform);
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

                StartGame();
            }
            else
            {
                Debug.LogError("Enter Room Fail! m_nResult : " + msg.m_nResult);
                SceneManager.LoadScene("Lobby");
            }
        }
        else if (iMsg.GetID() == GameEventMoveToC.MESSAGE_ID)
        {
            GameEventMoveToC msg = (GameEventMoveToC)iMsg;

            Move(msg.m_vec3Dest);
        }
    }
#endregion

#region Event Handler
    private void OnClicked(Vector3 vec3Pos)
    {
        GameEventMoveToR moveToR = new GameEventMoveToR();
        moveToR.m_nPlayerIndex = 0;
        moveToR.m_nElapsedTime = 0;
        moveToR.m_vec3Dest = vec3Pos;

        RoomNetwork.Instance.Send(moveToR);
    }
#endregion

    private void Move(Vector3 vec3Pos)
    {
        if (!m_MapManager.IsPositionValidToMove(vec3Pos))
        {
            return;
        }

        Node start = new Node(m_MisterBae.GetPosition(), false);
        Node end = new Node(vec3Pos, false);

        m_MapManager.InsertNode(start);
        m_MapManager.InsertNode(end);

        LinkedList<Node> listPath = m_AStarAlgorithm.AStar(start, end);

        m_MapManager.RemoveNode(start);
        m_MapManager.RemoveNode(end);

        m_MisterBae.Move(SmoothPathQuick(listPath));
    }

    private LinkedList<Node> SmoothPathQuick(LinkedList<Node> listPath)
    {
        if (listPath.Count <= 2)
        {
            return listPath;
        }

        LinkedListNode<Node> node1 = listPath.First;
        LinkedListNode<Node> node2 = node1.Next;
        LinkedListNode<Node> node3 = node2.Next;

        while (node3 != null)
        {
            //  check user can walk between 1, 3
            if (m_MapManager.CanMoveStraightly(node1.Value.m_vec3Pos, node3.Value.m_vec3Pos))
            {
                listPath.Remove(node2);

                node2 = node3;
                node3 = node2.Next;
            }
            else
            {
                node1 = node2;
                node2 = node1.Next;
                node3 = node2.Next;
            }
        }

        return listPath;
    }

    private void OnDestroy()
    {
        RoomNetwork.Instance.RemoveConnectHandler(OnConnected);
        RoomNetwork.Instance.RemoveRecvMessageHandler(OnRecvMessage);

        Application.targetFrameRate = m_nOldFrameRate;

        m_InputManager.Stop();
        m_CameraController.StopFollow();
    }
}
