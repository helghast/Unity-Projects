using System;
using UnityEngine;

public class MazeCell : MonoBehaviour {

	public IntVector2 coordinates;
	private readonly MazeCellEdge[] edges = new MazeCellEdge[MazeDirections.Count];
	private int initializedEdgeCount;
	public MazeRoom room;

	public bool IsFullyInitialized
	{
		get
		{
			return initializedEdgeCount == MazeDirections.Count;
		}
	}

	public MazeDirection RandomUninitializedDirection
	{
		get
		{
			int skips = UnityEngine.Random.Range(0, MazeDirections.Count - initializedEdgeCount);
			for (int i = 0; i < MazeDirections.Count; ++i)
			{
				if (edges[i] == null)
				{
					if (skips == 0)
					{
						return (MazeDirection)i;
					}
					skips -= 1;
				}
			}
			throw new System.InvalidOperationException("MazeCell hasn o uninitiazlied directions left.");
		}
	}

	public void SetEdge(MazeDirection direction, MazeCellEdge edge)
	{
		edges[(int)direction] = edge;
		initializedEdgeCount += 1;
	}

	public MazeCellEdge GetEdge(MazeDirection direction)
	{
		return edges[(int)direction];
	}

	public void Initialize(MazeRoom room)
	{
		room.Add(this);
		transform.GetChild(0).GetComponent<Renderer>().material = room.settings.floorMaterial;
	}

	public void OnPlayerEntered()
	{
		for (int i = 0; i < edges.Length; ++i)
		{
			edges[i].OnPlayerEntered();
		}
	}

	public void OnPlayerExited()
	{
		for (int i = 0; i < edges.Length; ++i)
		{
			edges[i].OnPlayerExited();
		}
	}
}
