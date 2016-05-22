using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//  reference : https://en.wikipedia.org/wiki/A*_search_algorithm
public class AStarAlgorithm
{
    // The set of nodes already evaluated.
    LinkedList<Node> closedSet = new LinkedList<Node>();

    // The set of currently discovered nodes still to be evaluated.
    // Initially, only the start node is known.
    LinkedList<Node> openSet = new LinkedList<Node>();

    Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
    Dictionary<Node, float> gScore = new Dictionary<Node, float>();
    Dictionary<Node, float> fScore = new Dictionary<Node, float>();

    //    f = g + h
    public LinkedList<Node> AStar(Node start, Node goal)
    {
        closedSet.Clear();
        openSet.Clear();
        openSet.AddLast(start);

        // For each node, which node it can most efficiently be reached from.
        // If a node can be reached from many nodes, cameFrom will eventually contain the
        // most efficient previous step.
        cameFrom.Clear();

        // For each node, the cost of getting from the start node to that node.
        gScore.Clear();    //    gScore := map with default value of Infinity
        // The cost of going from start to start is zero.
        gScore[start] = 0;

        // For each node, the total cost of getting from the start node to the goal
        // by passing by that node. That value is partly known, partly heuristic.
        fScore.Clear();    //fScore := map with default value of Infinity
        // For the first node, that value is completely heuristic.
        fScore[start] = heuristic_cost_estimate(start, goal);

        while(openSet.Count != 0)
        {
            Node current = FindLowestValue(openSet);
            if(current.Equals(goal))
            {
                return reconstruct_path(current);
            }

            openSet.Remove(current);
            closedSet.AddLast(current);

            foreach(Node neighbor in current.m_listNeighborNode)
            {
                if(closedSet.Contains(neighbor))
                {
                    continue;    // Ignore the neighbor which is already evaluated.
                }

                // The distance from start to a neighbor
                float tentative_gScore = gScore[current] + dist_between(current, neighbor);
                if(!openSet.Contains(neighbor))    // Discover a new node
                {
                    openSet.AddLast(neighbor);
                }
                else if(tentative_gScore >= gScore[neighbor])
                {
                    continue;        // This is not a better path.
                }

                // This path is the best until now. Record it!
                cameFrom[neighbor] = current;
                gScore[neighbor] = tentative_gScore;
                fScore[neighbor] = gScore[neighbor] + heuristic_cost_estimate(neighbor, goal);
            }
        }

        return null; //    failure
    }

    private Node FindLowestValue(LinkedList<Node> listSrc)
    {
        Node nodeFound = null;
        float fLowestValue = float.MaxValue;
        foreach(Node point in listSrc)
        {
            if(fScore[point] < fLowestValue)
            {
                fLowestValue = fScore[point];
                nodeFound = point;
            }
        }

        return nodeFound;
    }


    private LinkedList<Node> reconstruct_path(Node node)
    {
        LinkedList<Node> total_path = new LinkedList<Node>();

        Node cur = node;
        total_path.AddFirst(node);
        while(cameFrom.ContainsKey(cur))
        {
            total_path.AddFirst(cameFrom[cur]);
            cur = cameFrom[cur];
        }

        return total_path;
    }

    private float heuristic_cost_estimate(Node node1, Node node2)
    {
        return Mathf.Sqrt(Mathf.Pow((node1.m_vec3Pos.x - node2.m_vec3Pos.x), 2) + Mathf.Pow((node1.m_vec3Pos.z - node2.m_vec3Pos.z), 2));
    }

    private float dist_between(Node node1, Node node2)
    {
        return Mathf.Sqrt(Mathf.Pow((node1.m_vec3Pos.x - node2.m_vec3Pos.x), 2) + Mathf.Pow((node1.m_vec3Pos.z - node2.m_vec3Pos.z), 2));
    }
}