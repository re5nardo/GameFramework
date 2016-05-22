using UnityEngine;
using System.Collections.Generic;

public class QuadTree
{
    //  QuadTree of which level is equal to this has nodes  
    private const int LEVEL_OF_HAVING_NODES = 5;

    // Boundary of this quad tree
    private Rect2D boundary = new Rect2D();

    //  level of this quad tree
    private int level = 0;

    // Nodes in this quad tree node
    private List<Node> m_listNode = new List<Node>();

    // Children
    private QuadTree northWest = null;
    private QuadTree northEast = null;
    private QuadTree southWest = null;
    private QuadTree southEast = null;

    public QuadTree(Rect2D boundary, int level)
    {
        this.boundary = boundary;
        this.level = level;
    }

    // Insert a node into the QuadTree
    public bool Insert(Node node)
    {
        // Ignore objects that do not belong in this quad tree
        if (!boundary.Contains(new Vector2(node.m_vec3Pos.x, node.m_vec3Pos.z)))
            return false; // object cannot be added

        if (level >= LEVEL_OF_HAVING_NODES)
        {
            m_listNode.Add(node);

            return true;
        }
        else
        {
            if (HasChild())
            {
                return InsertIntoChild(node);
            }
            else
            {
                if (m_listNode.Count > 0)
                {
                    Subdivide();

                    foreach(Node nodeOld in m_listNode)
                    {
                        InsertIntoChild(nodeOld);
                    }
                    m_listNode.Clear();

                    return InsertIntoChild(node);
                }
                else
                {
                    m_listNode.Add(node);

                    return true;
                }
            }
        }
    }

    // Remove a node in the QuadTree
    public bool Remove(Node node)
    {
        // Check objects at this quad level
        for (int p = 0; p < m_listNode.Count; p++)
        {
            if (m_listNode[p] == node)
            {
                m_listNode.Remove(node);
                return true;
            }
        }

        // Terminate here, if there are no children
        if (northWest == null)
            return false;

        // Otherwise, add the points from the children
        if(northWest.Remove(node))
            return true;
        if(northEast.Remove(node))
            return true;
        if(southWest.Remove(node))
            return true;
        if(southEast.Remove(node))
            return true;
        
        return false;
    }

    // Create four children that fully divide this quad into four quads of equal area
    private void Subdivide()
    {
        northWest = new QuadTree(new Rect2D(boundary.center.x - boundary.width * 0.25f, boundary.center.y + boundary.height * 0.25f, boundary.width * 0.5f, boundary.height * 0.5f), level + 1);
        northEast = new QuadTree(new Rect2D(boundary.center.x + boundary.width * 0.25f, boundary.center.y + boundary.height * 0.25f, boundary.width * 0.5f, boundary.height * 0.5f), level + 1);
        southWest = new QuadTree(new Rect2D(boundary.center.x - boundary.width * 0.25f, boundary.center.y - boundary.height * 0.25f, boundary.width * 0.5f, boundary.height * 0.5f), level + 1);
        southEast = new QuadTree(new Rect2D(boundary.center.x + boundary.width * 0.25f, boundary.center.y - boundary.height * 0.25f, boundary.width * 0.5f, boundary.height * 0.5f), level + 1);
    }

    // Find all nodes that appear within a range
    public List<Node> QueryRange(Rect2D range)
    {
        // Prepare an array of results
        List<Node> nodesInRange = new List<Node>();

        // Automatically abort if the range does not intersect this quad
        if (!boundary.Overlaps(range))
            return nodesInRange; // empty list

        // Check objects at this quad level
        for (int p = 0; p < m_listNode.Count; p++)
        {
            if (range.Contains(new Vector2(m_listNode[p].m_vec3Pos.x, m_listNode[p].m_vec3Pos.z)))
                nodesInRange.Add(m_listNode[p]);
        }

        // Terminate here, if there are no children
        if (northWest == null)
            return nodesInRange;

        // Otherwise, add the points from the children
        nodesInRange.AddRange(northWest.QueryRange(range));
        nodesInRange.AddRange(northEast.QueryRange(range));
        nodesInRange.AddRange(southWest.QueryRange(range));
        nodesInRange.AddRange(southEast.QueryRange(range));

        return nodesInRange;
    }

    private bool InsertIntoChild(Node node)
    {
        if (northWest.Insert(node))
            return true;
        if (northEast.Insert(node))
            return true;
        if (southWest.Insert(node))
            return true;
        if (southEast.Insert(node))
            return true;

        // Otherwise, the point cannot be inserted for some unknown reason (this should never happen)
        return false;
    }

    private bool HasChild()
    {
        return northWest != null;
    }
}

//  reference site : https://en.wikipedia.org/wiki/Quadtree