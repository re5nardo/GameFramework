//------------------------------------------------------------------------
//
//  Name:   TimeSlicedGraphAlgorithms.h
//
//  Desc:   classes to implement graph algorithms that can be distributed
//          over multiple update-steps
//
//          Any graphs passed to these functions must conform to the
//          same interface used by the SparseGraph
//          
//  Author: Mat Buckland (fup@ai-junkie.com)
//
//------------------------------------------------------------------------

using System.Collections.Generic;

//------------------------ Graph_SearchTimeSliced -----------------------------
//
// base class to define a common interface for graph search algorithms
//-----------------------------------------------------------------------------
public abstract class Graph_SearchTimeSliced<edge_type>
{
	public enum SearchType{AStar, Dijkstra};

	private SearchType m_SearchType;


	public Graph_SearchTimeSliced(SearchType type)
	{
		m_SearchType = type;
	}

	//When called, this method runs the algorithm through one search cycle. The
	//method returns an enumerated value (target_found, target_not_found,
	//search_incomplete) indicating the status of the search
	public abstract int                           	CycleOnce();

	//returns the vector of edges that the algorithm has examined
	public abstract List<edge_type> GetSPT();


	//returns the total cost to the target
	public abstract double                         	GetCostToTarget();

	//returns a list of node indexes that comprise the shortest path
	//from the source to the target
	public abstract LinkedList<int>                	GetPathToTarget();

	//returns the path as a list of PathEdges
	public abstract LinkedList<PathEdge>           	GetPathAsPathEdges();

	public SearchType                            	GetType(){return m_SearchType;}
};




//-------------------------- Graph_SearchAStar_TS -----------------------------
//
//  a A* class that enables a search to be completed over multiple update-steps
//-----------------------------------------------------------------------------
class Graph_SearchAStar_TS<graph_type, node_type, edge_type, extra_info, heuristic> : Graph_SearchTimeSliced<edge_type> where graph_type : IGraph<node_type, edge_type> where node_type : NavGraphNode<extra_info> where edge_type : NavGraphEdge where heuristic : IHeuristic, new()
{
	private graph_type              m_Graph;

	//indexed into my node. Contains the 'real' accumulative cost to that node
	private List<double>            m_GCosts; 

	//indexed into by node. Contains the cost from adding m_GCosts[n] to
	//the heuristic cost from n to the target node. This is the vector the
	//iPQ indexes into.
	private List<double>            m_FCosts;

	private List<edge_type>       m_ShortestPathTree;
	private List<edge_type>       m_SearchFrontier;

	private int                            m_iSource;
	private int                            m_iTarget;

	//create an indexed priority queue of nodes. The nodes with the
	//lowest overall F cost (G+H) are positioned at the front.
	private IndexedPriorityQLow<double>    m_pPQ;

	private heuristic                      m_heuristic;


	public Graph_SearchAStar_TS(graph_type G, int source, int target) : base(SearchType.AStar)
	{
		m_Graph = G;
		m_ShortestPathTree = new List<edge_type>();
		for(int i = 0; i < G.NumNodes(); ++i)
		{
			m_ShortestPathTree.Add(default(edge_type));
		}
		m_SearchFrontier = new List<edge_type>();
		for(int i = 0; i < G.NumNodes(); ++i)
		{
			m_SearchFrontier.Add(default(edge_type));
		}
		m_GCosts = new List<double>();
		for(int i = 0; i < G.NumNodes(); ++i)
		{
			m_GCosts.Add(0.0);
		}
		for(int i = 0; i < G.NumNodes(); ++i)
		{
			m_FCosts.Add(0.0);
		}
		m_iSource = source;
		m_iTarget = target;

		//create the PQ   
		m_pPQ =new IndexedPriorityQLow<double>(m_FCosts, m_Graph.NumNodes());

		//put the source node on the queue
		m_pPQ.insert(m_iSource);

		m_heuristic = new heuristic ();
	}




	//When called, this method pops the next node off the PQ and examines all
	//its edges. The method returns an enumerated value (target_found,
	//target_not_found, search_incomplete) indicating the status of the search
	public override int CycleOnce()
	{
		//if the PQ is empty the target has not been found
		if (m_pPQ.empty())
		{
			return (int)Navigation.SearchResult.target_not_found;
		}

		//get lowest cost node from the queue
		int NextClosestNode = m_pPQ.Pop();

		//put the node on the SPT
		m_ShortestPathTree[NextClosestNode] = m_SearchFrontier[NextClosestNode];

		//if the target has been found exit
		if (NextClosestNode == m_iTarget)
		{
			return (int)Navigation.SearchResult.target_found;
		}

		//now to test all the edges attached to this node
		foreach(edge_type pE in m_Graph.GetEdgesOfNode(NextClosestNode))
		{
			//calculate the heuristic cost from this node to the target (H)                       
			double HCost = m_heuristic.Calculate<graph_type, node_type, edge_type, extra_info>(m_Graph, m_iTarget, pE.To()); 

			//calculate the 'real' cost to this node from the source (G)
			double GCost = m_GCosts[NextClosestNode] + pE.Cost();

			//if the node has not been added to the frontier, add it and update
			//the G and F costs
			if (m_SearchFrontier[pE.To()] == null)
			{
				m_FCosts[pE.To()] = GCost + HCost;
				m_GCosts[pE.To()] = GCost;

				m_pPQ.insert(pE.To());

				m_SearchFrontier[pE.To()] = pE;
			}

			//if this node is already on the frontier but the cost to get here
			//is cheaper than has been found previously, update the node
			//costs and frontier accordingly.
			else if ((GCost < m_GCosts[pE.To()]) && (m_ShortestPathTree[pE.To()]==null))
			{
				m_FCosts[pE.To()] = GCost + HCost;
				m_GCosts[pE.To()] = GCost;

				m_pPQ.ChangePriority(pE.To());

				m_SearchFrontier[pE.To()] = pE;
			}
		}

		//there are still nodes to explore
		return (int)Navigation.SearchResult.search_incomplete;
	}

	//returns the vector of edges that the algorithm has examined
	public override List<edge_type> GetSPT(){return m_ShortestPathTree;}

	//returns a vector of node indexes that comprise the shortest path
	//from the source to the target
	public override LinkedList<int> GetPathToTarget()
	{
		LinkedList<int> path = new LinkedList<int>();

		//just return an empty path if no target or no path found
		if (m_iTarget < 0)  return path;    

		int nd = m_iTarget;

		path.AddLast(nd);

		while ((nd != m_iSource) && (m_ShortestPathTree[nd] != null))
		{
			nd = m_ShortestPathTree[nd].From();

			path.AddFirst(nd);
		}

		return path;
	}

	//returns the path as a list of PathEdges
	public override LinkedList<PathEdge> GetPathAsPathEdges()
	{
		LinkedList<PathEdge> path = new LinkedList<PathEdge>();

		//just return an empty path if no target or no path found
		if (m_iTarget < 0)  return path;    

		int nd = m_iTarget;

		while ((nd != m_iSource) && (m_ShortestPathTree[nd] != null))
		{
			path.AddFirst(new PathEdge(m_Graph.GetNode(m_ShortestPathTree[nd].From()).Pos(),
				m_Graph.GetNode(m_ShortestPathTree[nd].To()).Pos(),
				m_ShortestPathTree[nd].Flags(),
				m_ShortestPathTree[nd].IDofIntersectingEntity()));

			nd = m_ShortestPathTree[nd].From();
		}

		return path;
	}

	//returns the total cost to the target
	public override double GetCostToTarget(){return m_GCosts[m_iTarget];}
};