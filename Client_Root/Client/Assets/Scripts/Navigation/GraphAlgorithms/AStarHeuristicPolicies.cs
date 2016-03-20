//-----------------------------------------------------------------------------
//
//  Name:   AStarHeuristicPolicies.h
//
//  Author: Mat Buckland (www.ai-junkie.com)
//
//  Desc:   class templates defining a heuristic policy for use with the A*
//          search algorithm
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public interface IHeuristic
{
	double Calculate<graph_type, node_type, edge_type, extra_info> (graph_type G, int nd1, int nd2) where graph_type : IGraph<node_type, edge_type> where node_type : NavGraphNode<extra_info> where edge_type : IEdge;
}

//-----------------------------------------------------------------------------
//the euclidian heuristic (straight-line distance)
//-----------------------------------------------------------------------------
class Heuristic_Euclid : IHeuristic
{
	public Heuristic_Euclid(){}

	//calculate the straight line distance from node nd1 to node nd2
	public double Calculate<graph_type, node_type, edge_type, extra_info>(graph_type G, int nd1, int nd2) where graph_type : IGraph<node_type, edge_type> where node_type : NavGraphNode<extra_info> where edge_type : IEdge
	{
		return Vector2.Distance (G.GetNode (nd1).Pos (), G.GetNode (nd2).Pos ());
	}
};

//-----------------------------------------------------------------------------
//this uses the euclidian distance but adds in an amount of noise to the 
//result. You can use this heuristic to provide imperfect paths. This can
//be handy if you find that you frequently have lots of agents all following
//each other in single file to get from one place to another
//-----------------------------------------------------------------------------
class Heuristic_Noisy_Euclidian : IHeuristic
{
	public Heuristic_Noisy_Euclidian(){}

	//calculate the straight line distance from node nd1 to node nd2
	public double Calculate<graph_type, node_type, edge_type, extra_info>(graph_type G, int nd1, int nd2) where graph_type : IGraph<node_type, edge_type> where node_type : NavGraphNode<extra_info>  where edge_type : IEdge
	{
		return Vector2.Distance (G.GetNode (nd1).Pos (), G.GetNode (nd2).Pos ()) * Utils.RandInRange(0.9f, 1.1f);
	}
};

//-----------------------------------------------------------------------------
//you can use this class to turn the A* algorithm into Dijkstra's search.
//this is because Dijkstra's is equivalent to an A* search using a heuristic
//value that is always equal to zero.
//-----------------------------------------------------------------------------
class Heuristic_Dijkstra : IHeuristic
{
	public double Calculate<graph_type, node_type, edge_type, extra_info>(graph_type G, int nd1, int nd2) where graph_type : IGraph<node_type, edge_type> where node_type : NavGraphNode<extra_info>  where edge_type : IEdge
	{
		return 0;
	}
};