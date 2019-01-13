using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
	[SerializeField] private float speed = 0.0f;
	// use for clouds, p.ej. when player is not moving, cloud will move to the left of screen.
	[SerializeField] private bool moveOnPlayerStop = false;

	private Rigidbody2D rigidbodyRef;
	private float endPoint = 20.4f;

	private void Awake()
	{
		rigidbodyRef = GetComponent<Rigidbody2D>();
	}

    // Update is called once per frame
    void Update()
    {
		// only when player moves
		float playerXVelocity = PlayerController.GetInstance.RigidbodyRef.velocity.x;
		if (playerXVelocity == 0f)
		{
			rigidbodyRef.velocity = moveOnPlayerStop ? new Vector2(-speed, 0) : Vector2.zero;
		}
		else 
		{
			rigidbodyRef.velocity = new Vector2(playerXVelocity > 0f ? -speed : speed, 0);
		}

		float parentPosition = transform.parent.transform.position.x;
		if (transform.position.x - parentPosition <= -endPoint)
		{
			transform.position = new Vector3(parentPosition + endPoint, transform.position.y, transform.position.z);
		}
	}
}
