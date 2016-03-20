using System.Collections.Generic;

public interface IGraph<node_type, edge_type> where node_type : INode where edge_type : IEdge
{
	int NumNodes ();
	LinkedList<edge_type> GetEdgesOfNode (int node);
	node_type GetNode(int node);
}
