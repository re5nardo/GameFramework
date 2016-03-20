//------------------------------------------------------------------------
//
//  Name:   SparseGraph.h
//
//  Desc:   Graph class using the adjacency list representation.
//
//  Author: Mat Buckland (fup@ai-junkie.com)
//
//------------------------------------------------------------------------
using System.Collections.Generic;
using System.Diagnostics;

public class SparseGraph<node_type, edge_type> : IGraph<node_type, edge_type> where node_type : INode where edge_type : IEdge
{
	//the nodes that comprise this graph
	private List<node_type>      		m_Nodes;

	//a vector of adjacency edge lists. (each node index keys into the list of edges associated with that node)
	private List<LinkedList<edge_type>> m_Edges;

	//is this a directed graph?
	private bool            			m_bDigraph;

	//the index of the next node to be added
	private int             			m_iNextNodeIndex;


	//returns true if an edge is not already present in the graph. Used
	//when adding edges to make sure no duplicates are created.
	private bool UniqueEdge(int from, int to)
	{
		foreach(edge_type curEdge in m_Edges[from])
		{
			if (curEdge.To() == to && curEdge.From() == from)
			{
				return false;
			}
		}

		return true;		
	}

	//iterates through all the edges in the graph and removes any that point
	//to an invalidated node
	private void CullInvalidEdges()
	{
		foreach(LinkedList<edge_type> curEdgeList in m_Edges)
		{
			foreach(edge_type curEdge in curEdgeList)
			{
				if (m_Nodes[curEdge.To()].Index() == Navigation.Defines.INVALID_NODE_INDEX || 
					m_Nodes[curEdge.From()].Index() == Navigation.Defines.INVALID_NODE_INDEX)
				{
					curEdgeList.Remove (curEdge);
				}
			}
		}
	}

	//ctor
	public SparseGraph(bool digraph)
	{
		m_iNextNodeIndex = 0;
		m_bDigraph = digraph;
	}

	//returns the node at the given index
	public node_type GetNode(int idx)
	{
		Debug.Assert(idx < m_Nodes.Count && idx >= 0, "<SparseGraph::GetNode>: invalid index");
	
		return m_Nodes[idx];
	}

	//const method for obtaining a reference to an edge
	public edge_type GetEdge(int from, int to)
	{
		Debug.Assert(from < m_Nodes.Count && from >= 0 && m_Nodes[from].Index() != Navigation.Defines.INVALID_NODE_INDEX, "<SparseGraph::GetEdge>: invalid 'from' index");

		Debug.Assert(to < m_Nodes.Count && to >= 0 && m_Nodes[to].Index() != Navigation.Defines.INVALID_NODE_INDEX, "<SparseGraph::GetEdge>: invalid 'to' index");

		foreach(edge_type curEdge in m_Edges[from])
		{
			if (curEdge.To() == to) return curEdge;
		}

		Debug.Assert (false, "<SparseGraph::GetEdge>: edge does not exist");
		return default(edge_type);
	}


	//retrieves the next free node index
	public int GetNextFreeNodeIndex(){return m_iNextNodeIndex;}

	//-------------------------- AddNode -------------------------------------
	//
	//  Given a node this method first checks to see if the node has been added
	//  previously but is now innactive. If it is, it is reactivated.
	//
	//  If the node has not been added previously, it is checked to make sure its
	//  index matches the next node index before being added to the graph
	//------------------------------------------------------------------------
	public int AddNode(node_type node)
	{
		if (node.Index() < m_Nodes.Count)
		{
			//make sure the client is not trying to add a node with the same ID as a currently active node
			Debug.Assert (m_Nodes[node.Index()].Index() == Navigation.Defines.INVALID_NODE_INDEX,
				"<SparseGraph::AddNode>: Attempting to add a node with a duplicate ID");

			m_Nodes[node.Index()] = node;

			return m_iNextNodeIndex;
		}
		else
		{
			//make sure the new node has been indexed correctly
			Debug.Assert (node.Index() == m_iNextNodeIndex, "<SparseGraph::AddNode>:invalid index");

			m_Nodes.Add (node);
			m_Edges.Add (new LinkedList<edge_type> ());

			return m_iNextNodeIndex++;
		}
	}

	//removes a node by setting its index to invalid_node_index
	public void RemoveNode(int node)
	{
		Debug.Assert(node < m_Nodes.Count, "<SparseGraph::RemoveNode>: invalid node index");

		//set this node's index to invalid_node_index
		m_Nodes[node].SetIndex(Navigation.Defines.INVALID_NODE_INDEX);

		//if the graph is not directed remove all edges leading to this node and then clear the edges leading from the node
		if (!m_bDigraph)
		{    
			//visit each neighbour and erase any edges leading to this node
			foreach(edge_type curEdge in m_Edges[node])
			{
				foreach(edge_type curE in m_Edges[curEdge.To()])
				{
					if (curE.To() == node)
					{
						m_Edges [curEdge.To ()].Remove (curE);

						break;
					}
				}
			}

			//finally, clear this node's edges
			m_Edges[node].Clear();
		}

		//if a digraph remove the edges the slow way
		else
		{
			CullInvalidEdges();
		}
	}

	//Use this to add an edge to the graph. The method will ensure that the
	//edge passed as a parameter is valid before adding it to the graph. If the
	//graph is a digraph then a similar edge connecting the nodes in the opposite
	//direction will be automatically added.
	public void AddEdge(edge_type edge)
	{
		//first make sure the from and to nodes exist within the graph 
		Debug.Assert(edge.From() < m_iNextNodeIndex && edge.To() < m_iNextNodeIndex, "<SparseGraph::AddEdge>: invalid node index");

		//make sure both nodes are active before adding the edge
		if (m_Nodes[edge.To()].Index() != Navigation.Defines.INVALID_NODE_INDEX && m_Nodes[edge.From()].Index() != Navigation.Defines.INVALID_NODE_INDEX)
		{
			//add the edge, first making sure it is unique
			if (UniqueEdge(edge.From(), edge.To()))
			{
				m_Edges[edge.From()].AddLast(edge);
			}

			//if the graph is undirected we must add another connection in the opposite direction
			if (!m_bDigraph)
			{
				//check to make sure the edge is unique before adding
				if (UniqueEdge(edge.To(), edge.From()))
				{
					edge_type NewEdge = edge;

					NewEdge.SetTo(edge.From());
					NewEdge.SetFrom(edge.To());

					m_Edges[edge.To()].AddLast(NewEdge);
				}
			}
		}
	}

	//removes the edge connecting from and to from the graph (if present). If
	//a digraph then the edge connecting the nodes in the opposite direction 
	//will also be removed.
	public void RemoveEdge(int from, int to)
	{
		Debug.Assert (from < m_Nodes.Count && to < m_Nodes.Count, "<SparseGraph::RemoveEdge>:invalid node index");

		if (!m_bDigraph)
		{
			foreach(edge_type curEdge in m_Edges[to])
			{
				if (curEdge.To() == from){m_Edges[to].Remove(curEdge);break;}
			}
		}

		foreach(edge_type curEdge in m_Edges[from])
		{
			if (curEdge.To() == to){m_Edges[from].Remove(curEdge);break;}
		}
	}

	//sets the cost of an edge
	public void SetEdgeCost(int from, int to, double cost)
	{
		//make sure the nodes given are valid
		Debug.Assert(from < m_Nodes.Count && to < m_Nodes.Count, "<SparseGraph::SetEdgeCost>: invalid index");

		//visit each neighbour and erase any edges leading to this node
		foreach(edge_type curEdge in m_Edges[from])
		{
			if (curEdge.To() == to)
			{
				curEdge.SetCost(cost);
				break;
			}
		}
	}

	//returns the number of active + inactive nodes present in the graph
	public int NumNodes(){return m_Nodes.Count;}
		
	//returns the number of active nodes present in the graph (this method's
	//performance can be improved greatly by caching the value)
	public int NumActiveNodes()
	{
		int count = 0;

		for (int n=0; n<m_Nodes.Count; ++n) if (m_Nodes[n].Index() != Navigation.Defines.INVALID_NODE_INDEX) ++count;

		return count;
	}

	//returns the total number of edges present in the graph
	public int NumEdges()
	{
		int tot = 0;

		foreach(LinkedList<edge_type> curEdge in m_Edges)
		{
			tot += curEdge.Count;
		}

		return tot;
	}

	//returns true if the graph is directed
	public bool isDigraph(){return m_bDigraph;}

	//returns true if the graph contains no nodes
	public bool isEmpty(){return m_Nodes.Count == 0;}

	//returns true if a node with the given index is present in the graph
	public bool isNodePresent(int nd)
	{
		if ((nd >= m_Nodes.Count || (m_Nodes[nd].Index() == Navigation.Defines.INVALID_NODE_INDEX)))
		{
			return false;
		}
		else return true;
	}

	//returns true if an edge connecting the nodes 'to' and 'from' is present in the graph
	public bool isEdgePresent(int from, int to)
	{
		if (isNodePresent(from) && isNodePresent(from))
		{
			foreach(edge_type curEdge in m_Edges[from])
			{
				if (curEdge.To() == to) return true;
			}

			return false;
		}
		else return false;
	}

	//clears the graph ready for new node insertions
	public void Clear(){m_iNextNodeIndex = 0; m_Nodes.Clear(); m_Edges.Clear();}

	public void RemoveEdges()
	{
		foreach(LinkedList<edge_type> it in m_Edges)
		{
			it.Clear();
		}
	}

	public LinkedList<edge_type> GetEdgesOfNode (int node)
	{
		return m_Edges [node];
	}
}
