using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] private float runningSpeed = 4f;
	[SerializeField] private int damage = -10;

	private Rigidbody2D rigidbodyRef = null;
	private bool turnAround = false;
	
	private void Awake()
	{
		rigidbodyRef = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		if (GameManager.GetInstance.CurrentGameState != EGameState.inGame)
		{
			rigidbodyRef.velocity = Vector2.zero;
			return;
		}

		float currentRunningSpeed = turnAround ? runningSpeed : -runningSpeed;

		transform.eulerAngles = new Vector3(0, turnAround ? 180.0f : 0, 0);

		rigidbodyRef.velocity = new Vector2(currentRunningSpeed, rigidbodyRef.velocity.y);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag(StringsTable.Player))
		{
			PlayerController.GetInstance.HealthPoints = damage;
		}
	}

	public bool TurnAround { get => turnAround; set => turnAround = value; }
}
