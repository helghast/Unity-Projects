using System;
using UnityEngine;

public class MazeWall : MazeCellEdge
{
	public Transform wall;

	public override void Initialize(MazeCell cell, MazeCell othercell, MazeDirection direction)
	{
		base.Initialize(cell, othercell, direction);
		wall.GetComponent<Renderer>().material = cell.room.settings.wallMaterial;
	}
}
