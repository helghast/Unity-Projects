using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	private CharacterController characterController;
	private Animator animator;

	[SerializeField] private float forwardMoveSpeed = 7.5f;
	[SerializeField] private float backwardMoveSpeed = 3f;
	[SerializeField] private float turnSpeed = 150f;

	private void Awake()
	{
		characterController = GetComponent<CharacterController>();
		animator = GetComponentInChildren<Animator>();

		// make this in another script
		Cursor.lockState = CursorLockMode.Locked;
	}

	// Update is called once per frame
	private void Update()
    {
		// read input
		float horizontal = Input.GetAxis("Mouse X"); //Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		Vector3 movement = new Vector3(horizontal, 0, vertical);

		/** Primitive character movement. v.1 **
		// based movement on framerate
		characterController.SimpleMove(movement * Time.deltaTime * moveSpeed);

		// update blend animation
		animator.SetFloat("Speed", movement.magnitude);

		// only if movement changes. to avoid reset position to forward
		if (movement.magnitude > 0)
		{
			// set rotation of dummy
			Quaternion newDirection = Quaternion.LookRotation(movement);

			// smooth change rotation
			transform.rotation = Quaternion.Slerp(transform.rotation, newDirection, Time.deltaTime * turnSpeed);
		}
		**/

		/** Character movement v.2 */
		animator.SetFloat("Speed", vertical);

		transform.Rotate(Vector3.up, horizontal * turnSpeed * Time.deltaTime); // framerate independent

		if (vertical != 0)
		{
			float moveSpeedToUse = (vertical > 0) ? forwardMoveSpeed : backwardMoveSpeed;
			characterController.SimpleMove(transform.forward * moveSpeedToUse * vertical);
		}
	}
}
