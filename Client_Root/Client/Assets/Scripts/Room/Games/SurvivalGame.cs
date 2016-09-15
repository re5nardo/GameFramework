using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SurvivalGame : IGame
{
    private AStarAlgorithm      m_AStarAlgorithm = new AStarAlgorithm();

    private MisterBae           m_MisterBae = null;

    private void Start()
    {
        //  temp
        StartGame();
    }

	private void StartGame()
	{
        StartCoroutine(PrepareGame());
	}

	private IEnumerator PrepareGame()
	{
		//	prefare for game
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

    public override void OnRecvMessage(IMessage msg)
    {
        if (msg.GetID() == GameEventMoveToC.MESSAGE_ID)
        {
            OnRecvGameEvent(msg);
        }
        else if (msg.GetID() == GameStartToC.MESSAGE_ID)
        {
            GameStartToC resp = (GameStartToC)msg;



            StartGame();
        }
    }

    private void OnRecvGameEvent(IMessage msg)
    {
        if (msg.GetID() == GameEventMoveToC.MESSAGE_ID)
        {
            Move((msg as GameEventMoveToC).m_vec3Dest);
        }
    }

    private void OnClicked(Vector3 vec3Pos)
    {
        GameEventMoveToS moveToS = new GameEventMoveToS();
        moveToS.m_nPlayerIndex = 0;
        moveToS.m_nElapsedTime = 0;
        moveToS.m_vec3Dest = vec3Pos;

        Network.Instance.Send(moveToS);
    }

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
        m_InputManager.Stop();
        m_CameraController.StopFollow();
    }
}
