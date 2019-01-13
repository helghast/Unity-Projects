using UnityEngine;

public class MazeDoor : MazePassage {

	public Transform hinge;

	private static Quaternion
		normalRotation = Quaternion.Euler(0f, -90f, 0f),
		mirroredRotation = Quaternion.Euler(0f, 90f, 0f);

	private bool isMirrored;

	private MazeDoor OtherSideOfDoor
	{
		get
		{
			return othercell.GetEdge(direction.GetOpposite()) as MazeDoor;
		}
	}

	public override void Initialize(MazeCell cell, MazeCell othercell, MazeDirection direction)
	{
		base.Initialize(cell, othercell, direction);

		if (OtherSideOfDoor != null)
		{
			isMirrored = true;
			/* this will throw and warning: BoxColliders does not support negative scale or size. 
			The effective box size has been forced positive and is likely to give unexpected collision geometry.
			If you absolutely need to use negative scaling you can use the convex MeshCollider.
			
			replace BoxCollider by Convex Mesh Collider*/
			hinge.localScale = new Vector3(-1f, 1f, 1f);
			Vector3 p = hinge.localPosition;
			p.x = -p.x;
			hinge.localPosition = p;

			//another solution
			//float newZScale = Mathf.Abs(hinge.localScale.z - 1f);
			//hinge.localScale = new Vector3(hinge.localScale.x, hinge.localScale.y, newZScale);
		}

		for (int i = 0; i < transform.childCount; ++i)
		{
			Transform child = transform.GetChild(i);
			if (child != hinge)
			{
				child.GetComponent<Renderer>().material = cell.room.settings.wallMaterial;
			}
		}
	}

	public override void OnPlayerEntered()
	{
		OtherSideOfDoor.hinge.localRotation = hinge.localRotation = isMirrored ? mirroredRotation : normalRotation;
	}

	public override void OnPlayerExited()
	{
		OtherSideOfDoor.hinge.localRotation = hinge.localRotation = Quaternion.identity;
	}
}
