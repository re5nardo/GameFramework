﻿//----------------------- IndexedPriorityQLow ---------------------------
//
//  Priority queue based on an index into a set of keys. The queue is
//  maintained as a 2-way heap.
//
//  The priority in this implementation is the lowest valued key
//------------------------------------------------------------------------
using System.Collections.Generic;
using System.Diagnostics;

public class IndexedPriorityQLow<KeyType>
{
	private List<KeyType>  	m_vecKeys;
	private List<int>       m_Heap;
	private List<int>       m_invHeap;
	private int             m_iSize, m_iMaxSize;

	private void Swap(int a, int b)
	{
		int temp = m_Heap[a]; m_Heap[a] = m_Heap[b]; m_Heap[b] = temp;

		//change the handles too
		m_invHeap[m_Heap[a]] = a; m_invHeap[m_Heap[b]] = b;
	}

	private void ReorderUpwards(int nd)
	{
		if ((typeof(KeyType) is double) == false)
		{
			Debug.Assert (true, "exception!");
		}
			
		//move up the heap swapping the elements until the heap is ordered
		while ((nd > 1) && ((double)(object)m_vecKeys [m_Heap [nd / 2]] > (double)(object)m_vecKeys [m_Heap [nd]]))
		{      
			Swap (nd / 2, nd);

			nd /= 2;
		}
	}

	private void ReorderDownwards(int nd, int HeapSize)
	{
		if ((typeof(KeyType) is double) == false)
		{
			Debug.Assert (true, "exception!");
		}

		//move down the heap from node nd swapping the elements until the heap is reordered
		while (2*nd <= HeapSize)
		{
			int child = 2 * nd;

			//set child to smaller of nd's two children
			if ((child < HeapSize) && ((double)(object)m_vecKeys[m_Heap[child]] > (double)(object)m_vecKeys[m_Heap[child+1]]))
			{
				++child;
			}

			//if this nd is larger than its child, swap
			if ((double)(object)m_vecKeys[m_Heap[nd]] > (double)(object)m_vecKeys[m_Heap[child]])
			{
				Swap(child, nd);

				//move the current node down the tree
				nd = child;
			}

			else
			{
				break;
			}
		}
	}
		
	//you must pass the constructor a reference to the std::vector the PQ
	//will be indexing into and the maximum size of the queue.
	public IndexedPriorityQLow(List<KeyType> keys, int MaxSize)
	{
		m_vecKeys = keys;
		m_iMaxSize = MaxSize;
		m_iSize = 0;
		m_Heap = new List<int> ();
		for(int i = 0; i < MaxSize+1; ++i)
		{
			m_Heap.Add (0);
		}
		m_invHeap = new List<int> ();
		for(int i = 0; i < MaxSize+1; ++i)
		{
			m_invHeap.Add (0);
		}
	}

	public bool empty(){return (m_iSize==0);}

	//to insert an item into the queue it gets added to the end of the heap
	//and then the heap is reordered from the bottom up.
	public void insert(int idx)
	{
		Debug.Assert (m_iSize+1 <= m_iMaxSize);

		++m_iSize;

		m_Heap[m_iSize] = idx;

		m_invHeap[idx] = m_iSize;

		ReorderUpwards(m_iSize);
	}

	//to get the min item the first element is exchanged with the lowest
	//in the heap and then the heap is reordered from the top down. 
	public int Pop()
	{
		Swap(1, m_iSize);

		ReorderDownwards(1, m_iSize-1);

		return m_Heap[m_iSize--];
	}

	//if the value of one of the client key's changes then call this with the key's index to adjust the queue accordingly
	public void ChangePriority(int idx)
	{
		ReorderUpwards(m_invHeap[idx]);
	}
}
