using UnityEngine;

public abstract class MazeCellEdge : MonoBehaviour {

	public MazeCell cell, othercell;
	public MazeDirection direction;

	public virtual void Initialize(MazeCell cell, MazeCell othercell, MazeDirection direction)
	{
		this.cell = cell;
		this.othercell = othercell;
		this.direction = direction;
		this.cell.SetEdge(this.direction, this);
		transform.parent = this.cell.transform;
		transform.localPosition = Vector3.zero;
		transform.localRotation = this.direction.ToRotation();
	}

	public virtual void OnPlayerEntered() { }
	public virtual void OnPlayerExited() { }
}
