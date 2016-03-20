//-----------------------------------------------------------------------------
//
//  Graph node for use in creating a navigation graph.This node contains
//  the position of the node and a pointer to a BaseGameEntity... useful
//  if you want your nodes to represent health packs, gold mines and the like
//-----------------------------------------------------------------------------
using UnityEngine;

public class NavGraphNode<extra_info> : GraphNode
{
	//the node's position
	protected Vector2     m_vPosition;

	//often you will require a navgraph node to contain additional information.
	//For example a node might represent a pickup such as armor in which
	//case m_ExtraInfo could be an enumerated value denoting the pickup type,
	//thereby enabling a search algorithm to search a graph for specific items.
	//Going one step further, m_ExtraInfo could be a pointer to the instance of
	//the item type the node is twinned with. This would allow a search algorithm
	//to test the status of the pickup during the search. 
	protected extra_info  m_ExtraInfo;

	//ctors
	public NavGraphNode()
	{
		m_ExtraInfo = default(extra_info);
	}

	public NavGraphNode(int idx, Vector2 pos) : base(idx)
	{
		m_vPosition = pos;
		m_ExtraInfo = default(extra_info);
	}

	public Vector2    Pos(){return m_vPosition;}
	public void       SetPos(Vector2 NewPosition){m_vPosition = NewPosition;}

	public extra_info ExtraInfo(){return m_ExtraInfo;}
	public void       SetExtraInfo(extra_info info){m_ExtraInfo = info;}
}
