//-----------------------------------------------------------------------------
//
//  Name:   GraphNodeTypes.h
//
//  Author: Mat Buckland (www.ai-junkie.com)
//
//  Desc:   Node classes to be used with graphs
//-----------------------------------------------------------------------------

public class GraphNode : INode
{
	//every node has an index. A valid index is >= 0
	protected int m_iIndex;

	public GraphNode()
	{
		m_iIndex = Navigation.Defines.INVALID_NODE_INDEX;
	}

	public GraphNode(int idx)
	{
		m_iIndex = idx;
	}
		
	public int  Index(){return m_iIndex;}
	public void SetIndex(int NewIndex){m_iIndex = NewIndex;}
}
