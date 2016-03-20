//------------------------------- Graph_SearchAStar --------------------------
//
//  this searchs a graph using the distance between the target node and the 
//  currently considered node as a heuristic.
//
//  This search is more commonly known as A* (pronounced Ay-Star)
//-----------------------------------------------------------------------------
using System.Collections.Generic;

public class Graph_SearchAStar<graph_type, node_type, edge_type, extra_info, heuristic> where graph_type : IGraph<node_type, edge_type> where node_type : NavGraphNode<extra_info> where edge_type : IEdge where heuristic : IHeuristic, new()
{
	//create a typedef for the edge type used by the graph
	//typedef typename graph_type::EdgeType Edge;

	private graph_type               m_Graph;

	//indexed into my node. Contains the 'real' accumulative cost to that node
	private List<double>             m_GCosts; 

	//indexed into by node. Contains the cost from adding m_GCosts[n] to
	//the heuristic cost from n to the target node. This is the vector the
	//iPQ indexes into.
	private List<double>             m_FCosts;

	private List<edge_type>       m_ShortestPathTree;
	private List<edge_type>       m_SearchFrontier;

	private int                            m_iSource;
	private int                            m_iTarget;

	private heuristic                      m_heuristic;

	//the A* search algorithm
	private void Search()
	{
		//create an indexed priority queue of nodes. The nodes with the
		//lowest overall F cost (G+H) are positioned at the front.
		IndexedPriorityQLow<double> pq = new IndexedPriorityQLow<double>(m_FCosts, m_Graph.NumNodes());

		//put the source node on the queue
		pq.insert(m_iSource);

		//while the queue is not empty
		while(!pq.empty())
		{
			//get lowest cost node from the queue
			int NextClosestNode = pq.Pop();

			//move this node from the frontier to the spanning tree
			m_ShortestPathTree[NextClosestNode] = m_SearchFrontier[NextClosestNode];

			//if the target has been found exit
			if (NextClosestNode == m_iTarget) return;

			//now to test all the edges attached to this node
			foreach(edge_type pE in m_Graph.GetEdgesOfNode(NextClosestNode))
			{
				//calculate the heuristic cost from this node to the target (H)                       
				double HCost = m_heuristic.Calculate<graph_type, node_type, edge_type, extra_info>(m_Graph, m_iTarget, pE.To()); 

				//calculate the 'real' cost to this node from the source (G)
				double GCost = m_GCosts[NextClosestNode] + pE.Cost();

				//if the node has not been added to the frontier, add it and update the G and F costs
				if (m_SearchFrontier[pE.To()] == null)
				{
					m_FCosts[pE.To()] = GCost + HCost;
					m_GCosts[pE.To()] = GCost;

					pq.insert(pE.To());

					m_SearchFrontier[pE.To()] = pE;
				}

				//if this node is already on the frontier but the cost to get here
				//is cheaper than has been found previously, update the node
				//costs and frontier accordingly.
				else if ((GCost < m_GCosts[pE.To()]) && (m_ShortestPathTree[pE.To()]==null))
				{
					m_FCosts[pE.To()] = GCost + HCost;
					m_GCosts[pE.To()] = GCost;

					pq.ChangePriority(pE.To());

					m_SearchFrontier[pE.To()] = pE;
				}
			}
		}
	}

	public Graph_SearchAStar(graph_type graph, int source, int target)
	{
		m_Graph = graph;
		m_ShortestPathTree = new List<edge_type> ();
		for(int nIndex = 0; nIndex < graph.NumNodes(); ++nIndex)
		{
			m_ShortestPathTree.Add (default(edge_type));
		}
		m_SearchFrontier = new List<edge_type> ();
		for(int nIndex = 0; nIndex < graph.NumNodes(); ++nIndex)
		{
			m_SearchFrontier.Add (default(edge_type));
		}        
		m_GCosts = new List<double> ();
		for(int i = 0; i < graph.NumNodes(); ++i)
		{
			m_GCosts.Add (0.0);
		}
		m_FCosts = new List<double> ();
		for(int i = 0; i < graph.NumNodes(); ++i)
		{
			m_FCosts.Add (0.0);
		}
		m_iSource = source;
		m_iTarget = target;

		m_heuristic = new heuristic ();

		Search();   
	}

	//returns the vector of edges that the algorithm has examined
	public List<edge_type> GetSPT(){return m_ShortestPathTree;}

	//returns a vector of node indexes that comprise the shortest path from the source to the target
	public LinkedList<int> GetPathToTarget()
	{
		LinkedList<int> path = new LinkedList<int>();

		//just return an empty path if no target or no path found
		if (m_iTarget < 0)  return path;    

		int nd = m_iTarget;

		path.AddFirst(nd);

		while ((nd != m_iSource) && (m_ShortestPathTree[nd] != null))
		{
			nd = m_ShortestPathTree[nd].From();

			path.AddFirst(nd);
		}

		return path;
	}

	//returns the total cost to the target
	public double GetCostToTarget(){return m_GCosts[m_iTarget];}
}