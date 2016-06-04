using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SurvivalGame : IGame
{
    private AStarAlgorithm      m_AStarAlgorithm = new AStarAlgorithm();
    //private ICharacter[]        m_arrCharacter = new ICharacter[5]();
    public QuerySDMecanimController m_Query = null;
    private Coroutine m_crRun = null;

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

        m_CameraController.FollowTarget(m_Query.transform);

	}

    public override void OnRecvMessage(IMessage msg)
    {
        if (msg.GetID() == (ushort)Messages.REQ_MOVE_ID)
        {
            Move((msg as ReqMove).m_vec3Position);
        }
    }

    private void OnClicked(Vector3 vec3Pos)
    {
        ReqMove req = new ReqMove();
        req.m_vec3Position = vec3Pos;

        Network.Instance.Send(req);
    }

    private void Move(Vector3 vec3Pos)
    {
        if (!m_MapManager.IsMovable(m_Query.transform.position, vec3Pos))
        {
            return;
        }

        if (m_crRun != null)
        {
            StopCoroutine(m_crRun);
        }

        Node start = new Node(m_Query.transform.position, false);
        Node end = new Node(vec3Pos, false);

        m_MapManager.InsertNode(start);
        m_MapManager.InsertNode(end);

        LinkedList<Node> listPath = m_AStarAlgorithm.AStar(start, end);

        m_MapManager.RemoveNode(start);
        m_MapManager.RemoveNode(end);

        m_crRun = StartCoroutine(Run(SmoothPathQuick(listPath)));
    }

    float fSpeed = 4f / 1f;
    private IEnumerator Run(LinkedList<Node> listPath)
    {
        m_Query.ChangeAnimation(QuerySDMecanimController.QueryChanSDAnimationType.NORMAL_RUN);

        LinkedListNode<Node> node = new LinkedList<Node>(listPath).First;

        while (node.Next != null)
        {
            Vector3 start = node.Value.m_vec3Pos;
            Vector3 end = node.Next.Value.m_vec3Pos;

            float fDistance = Vector3.Distance(start, end);
            float fTime = fDistance / fSpeed;
            float fElapsedTime = 0f;

            while (fElapsedTime < fTime)
            {
                m_Query.transform.position = Vector3.Lerp(start, end, fElapsedTime / fTime);

                fElapsedTime += Time.deltaTime;

                yield return null;
            }

            m_Query.transform.position = end;
            node = node.Next;
        }
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
            if (m_MapManager.IsMovable(node1.Value.m_vec3Pos, node3.Value.m_vec3Pos))
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
