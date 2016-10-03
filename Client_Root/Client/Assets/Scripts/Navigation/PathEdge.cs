//-----------------------------------------------------------------------------
//
//  Name:   PathEdge.h
//
//  Author: Mat Buckland (ai-junkie.com)
//
//  Desc:   class to represent a path edge. This path can be used by a path
//          planner in the creation of paths. 
//
//-----------------------------------------------------------------------------

using UnityEngine;

public class PathEdge
{
	//positions of the source and destination nodes this edge connects
	private Vector2 m_vSource;
	private Vector2 m_vDestination;

	//the behavior associated with traversing this edge
	private int      m_iBehavior;

	private int      m_iDoorID;

	public PathEdge(Vector2 Source, Vector2 Destination, int Behavior, int DoorID = 0)
	{
		m_vSource = Source;
		m_vDestination = Destination;
		m_iBehavior = Behavior;
		m_iDoorID = DoorID;
	}

	public Vector2  Destination(){return m_vDestination;}
	public void     SetDestination(Vector2 NewDest){m_vDestination = NewDest;}

	public Vector2  Source(){return m_vSource;}
	public void     SetSource(Vector2 NewSource){m_vSource = NewSource;}

	public int      DoorID(){return m_iDoorID;}
	public int      Behavior(){return m_iBehavior;}
}
