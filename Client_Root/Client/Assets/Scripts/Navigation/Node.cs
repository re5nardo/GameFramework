﻿using UnityEngine;
using System.Collections.Generic;

public class Node
{
    public List<Node>   m_listNeighborNode = new List<Node>();
    public bool         m_bFixed = true;

    private Vector3     m_vec3Pos_ = Vector3.zero;
    public Vector3      m_vec3Pos
    {
        get
        {
            return m_vec3Pos_;
        }
        set
        {
            m_vec3Pos_ = value;
        }
    }

    public Node()
    {
    }

    public Node(Vector3 vec3Pos, bool bFixed = true)
    {
        m_vec3Pos = vec3Pos;
        m_bFixed = bFixed;
    }
}
