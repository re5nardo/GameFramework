//-----------------------------------------------------------------------------
//
//  Name:   Raven_PathPlanner.h
//
//  Author: Mat Buckland (www.ai-junkie.com)
//
//  Desc:   class to handle the creation of paths through a navigation graph
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

public class PathPlanner : IPathPlanner
{
	//A pointer to the owner of this class
	//private Bot                          m_pOwner;

	//a reference to the navgraph
	private SparseGraph<GraphNode, NavGraphEdge>          m_NavGraph;

	//a pointer to an instance of the current graph search algorithm.
	private Graph_SearchTimeSliced<NavGraphEdge>  m_pCurrentSearch;

	//this is the position the bot wishes to plan a path to reach
	private Vector2                            m_vDestinationPos;


	//returns the index of the closest visible and unobstructed graph node to
	//the given position
	private int   GetClosestNodeToPosition(Vector2 pos)
	{
		return 0;
	}

	//smooths a path by removing extraneous edges. (may not remove all extraneous edges)
	private void  SmoothPathEdgesQuick(LinkedList<PathEdge> path)
	{
	}

	//smooths a path by removing extraneous edges. (removes *all* extraneous edges)
	private void  SmoothPathEdgesPrecise(LinkedList<PathEdge> path)
	{
	}

	//called at the commencement of a new search request. It clears up the 
	//appropriate lists and memory in preparation for a new search request
	private void  GetReadyForNewSearch()
	{
	}

	public PathPlanner()
	{
		//m_NavGraph = owner.getw
		m_pCurrentSearch = null;
	}

//	public PathPlanner(Bot owner)
//	{
//		m_pOwner = owner;
//		//m_NavGraph = owner.getw
//		m_pCurrentSearch = null;
//	}

	//creates an instance of the A* time-sliced search and registers it with the path manager
	public bool       RequestPathToItem(uint ItemType)
	{
		return false;
	}

	//creates an instance of the Dijkstra's time-sliced search and registers it with the path manager
	public bool       RequestPathToPosition(Vector2 TargetPos)
	{
		return false;
	}

	//called by an agent after it has been notified that a search has terminated
	//successfully. The method extracts the path from m_pCurrentSearch, adds
	//additional edges appropriate to the search type and returns it as a list of
	//PathEdges.
	public LinkedList<PathEdge>       GetPath()
	{
		return null;
	}

	//returns the cost to travel from the bot's current position to a specific 
	//graph node. This method makes use of the pre-calculated lookup table
	//created by Raven_Game
	public double      GetCostToNode(uint NodeIdx)
	{
		return 0.0;
	}

	//returns the cost to the closest instance of the GiverType. This method
	//also makes use of the pre-calculated lookup table. Returns -1 if no active
	//trigger found
	public double      GetCostToClosestItem(uint GiverType)
	{
		return 0.0;
	}


	//the path manager calls this to iterate once though the search cycle
	//of the currently assigned search algorithm. When a search is terminated
	//the method messages the owner with either the msg_NoPathAvailable or
	//msg_PathReady messages
	public int        CycleOnce()
	{
		return 0;
	}

	public Vector2    GetDestination(){return m_vDestinationPos;}
	public void       SetDestination(Vector2 NewPos){m_vDestinationPos = NewPos;}

	//used to retrieve the position of a graph node from its index. (takes
	//into account the enumerations 'non_graph_source_node' and 
	//'non_graph_target_node'
	public Vector2   GetNodePosition(int idx)
	{
		return Vector2.zero;
	}
}
