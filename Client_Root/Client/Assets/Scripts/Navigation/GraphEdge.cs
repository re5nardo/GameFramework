//-----------------------------------------------------------------------------
//
//  Name:   GraphEdgeTypes.h
//
//  Author: Mat Buckland (www.ai-junkie.com)
//
//  Desc:   Class to define an edge connecting two nodes.
//          
//          An edge has an associated cost.
//-----------------------------------------------------------------------------

public class GraphEdge
{
	//An edge connects two nodes. Valid node indices are always positive.
	protected int 		m_iFrom;
	protected int 		m_iTo;

	//the cost of traversing the edge
	protected double 	m_dCost;

	//ctors
	public GraphEdge(int from, int to, double cost)
	{
		m_dCost = cost;
		m_iFrom = from;
		m_iTo = to;
	}

	public GraphEdge(int from, int  to)
	{
		m_dCost = 1.0;
		m_iFrom = from;
		m_iTo = to;
	}

	public GraphEdge()
	{
		m_dCost = 1.0;
		m_iFrom = Navigation.INVALID_NODE_INDEX;
		m_iTo = Navigation.INVALID_NODE_INDEX;
	}

	public int   From(){return m_iFrom;}
	public void  SetFrom(int NewIndex){m_iFrom = NewIndex;}

	public int   To(){return m_iTo;}
	public void  SetTo(int NewIndex){m_iTo = NewIndex;}

	public double Cost(){return m_dCost;}
	public void   SetCost(double NewCost){m_dCost = NewCost;}

	//these two operators are required
	public static bool operator==(GraphEdge edge1, GraphEdge edge2)
	{
		return edge1.m_iFrom == edge2.m_iFrom &&
			edge1.m_iTo   == edge2.m_iTo   &&
			edge1.m_dCost == edge2.m_dCost;
	}

	public static bool operator!=(GraphEdge edge1, GraphEdge edge2)
	{
		return !(edge1 == edge2);
	}
}
