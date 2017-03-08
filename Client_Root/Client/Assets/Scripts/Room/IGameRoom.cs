using UnityEngine;
using System.Collections.Generic;

public abstract class IGameRoom : MonoSingleton<IGameRoom>
{
    [SerializeField] protected MapManager   m_MapManager = null;

    protected List<IBehaviorBasedObjectAI> m_listObstacle = new List<IBehaviorBasedObjectAI>();

    private AStarAlgorithm  m_AStarAlgorithm = new AStarAlgorithm();

    public abstract float GetElapsedTime();  //  Second
    public abstract int GetPlayerIndex();

    public virtual float GetCorrectionTime()
    {
        return 0.2f;
    }

    public virtual float GetCorrectionThreshold()
    {
        return 1f;
    }

    public bool RegisterObstacle(IBehaviorBasedObjectAI obstacle)
    {
        if (m_listObstacle.Contains(obstacle))
        {
            return false;
        }

        m_listObstacle.Add(obstacle);

        return true;
    }

    public bool UnRegisterObstacle(IBehaviorBasedObjectAI obstacle)
    {
        if (!m_listObstacle.Contains(obstacle))
        {
            return false;
        }

        m_listObstacle.Remove(obstacle);

        return true;
    }

    public LinkedList<Node> GetMovePath(Vector3 vec3Start, Vector3 vec3Dest)
    {
        if (!m_MapManager.IsPositionValidToMove(vec3Dest))
        {
            return null;
        }

        Node start = new Node(vec3Start, false);
        Node end = new Node(vec3Dest, false);

        m_MapManager.InsertNode(start);
        m_MapManager.InsertNode(end);

        LinkedList<Node> listPath = m_AStarAlgorithm.AStar(start, end);

        m_MapManager.RemoveNode(start);
        m_MapManager.RemoveNode(end);

        return SmoothPathQuick(listPath);
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
}
