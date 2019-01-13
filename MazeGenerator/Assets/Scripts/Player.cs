using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private MazeCell currentCell;
	private MazeDirection currentDirection;

	public float speed;
	public float turnSpeed;

	private CharacterController physicsController;
	private Camera playerCamera;

	//initializations:
	void Awake()
	{
		physicsController = GetComponent<CharacterController>();
		playerCamera = GetComponentInChildren<Camera>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	private void Update ()
	{
		//if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
		//{
		//	Move(currentDirection);
		//}
		//else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
		//{
		//	Move(currentDirection.GetNextClockWise());
		//}
		//else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
		//{
		//	 Move(currentDirection.GetOpposite());
		//}
		//else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
		//{
		//	 Move(currentDirection.GetNextCounterClockWise());
		//}
		//else if (Input.GetKeyDown(KeyCode.E))
		//{
		//	Look(currentDirection.GetNextCounterClockWise());
		//}
		//else if (Input.GetKeyDown(KeyCode.Q))
		//{
		//	Look(currentDirection.GetNextClockWise());
		//}

		//float x = Input.GetAxis("Horizontal") * Time.deltaTime * 100.0f;
		//float z = Input.GetAxis("Vertical") * Time.deltaTime * 2.0f;
		//transform.Rotate(0, x, 0);
		//transform.Translate(0, 0, z);	

		if (Input.GetKeyDown(KeyCode.T))
		{
			Move(currentDirection);
		}
	}

	private void FixedUpdate()
	{
		if (Input.GetKey(KeyCode.D))
		{
			transform.Rotate(new Vector3(0.0f, turnSpeed, 0.0f), Space.World);
		}
		else if (Input.GetKey(KeyCode.A))
		{
			transform.Rotate(new Vector3(0.0f, -1 * turnSpeed, 0.0f), Space.World);
		}

		if (Input.GetKey(KeyCode.Q) && Vector3.Angle(playerCamera.transform.forward, Vector3.up) < 170f)
		{
			transform.Rotate(playerCamera.transform.right, turnSpeed, Space.World);
		}
		else if (Input.GetKey(KeyCode.E) && Vector3.Angle(playerCamera.transform.forward, Vector3.up) > 10f)
		{
			transform.Rotate(playerCamera.transform.right, -1 * turnSpeed, Space.World);
		}


		float moveGravity = 0.0f;
		if (!physicsController.isGrounded)
		{
			moveGravity = -0.1f;
		}

		Vector3 movement = new Vector3(0f, moveGravity, 0f);
		if (Input.GetKey(KeyCode.W))
		{
			movement += speed * ForwardVector;
		}
		else if (Input.GetKey(KeyCode.S))
		{
			movement -= speed * ForwardVector;
		}

		physicsController.Move(movement);
	}

	/// <summary>
	/// The direction that the player's camera is facing.
	/// </summary>
	private Vector3 ForwardVector
	{
		get
		{
			var ret = playerCamera.transform.forward;
			ret.y = 0f;
			return ret;
		}
	}

	private void Move(MazeDirection direction)
	{
		MazeCellEdge edge = currentCell.GetEdge(direction);
		if (edge is MazePassage)
		{
			SetLocation(edge.othercell);
		}
	}

	public void SetLocation(MazeCell cell)
	{
		if (currentCell != null)
		{
			currentCell.OnPlayerExited();
		}

		currentCell = cell;
		transform.localPosition = cell.transform.localPosition;

		currentCell.OnPlayerEntered();
	}

	private void Look(MazeDirection direction)
	{
		transform.localRotation = direction.ToRotation();
		currentDirection = direction;
	}
}
