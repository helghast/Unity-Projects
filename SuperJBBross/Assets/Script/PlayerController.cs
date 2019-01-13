using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float jumpForce = 5f;
	[SerializeField] private float runningSpeed = 5f;
	[SerializeField] private Animator animatorRef = null;
	[SerializeField] private LayerMask groundLayer = -1;
	[SerializeField] private SpriteRenderer spriteRendererRef = null;

	private Rigidbody2D rigidbodyRef;
	private Vector3 startPosition;

	[SerializeField] private int maxHealthPoints = 100;
	[SerializeField] private int minHealthPoints = 10;
	[SerializeField] private int maxManaPoints = 10;
	[SerializeField] private int consumeManaOnJump = 2;
	[SerializeField] private float jumpForceWithMana = 1.5f;
	[SerializeField] private float consumeHealthPerTick = 0.1f;
	[SerializeField] private int initialHealthPoints = 100;
	[SerializeField] private int initialManaPoints = 10;
	//[SerializeField] private float minRunningSpeed = 0.5f;

	// healthpoint to more run speed. manapoints to more jump height
	private int currentHealthPoints;
	private int currentManaPoints;
	private bool sprintCoroutineRunning;
	private float raycastToFloorDistance = 0.2f;

	//singleton pattern
	private static PlayerController sharedInstance = null;
	public static PlayerController GetInstance { get => sharedInstance; }

	private void Awake()
	{
		// due inherint from Monobehavior, you cannot use constructor. Use Awake instead
		sharedInstance = this;

		rigidbodyRef = GetComponent<Rigidbody2D>();
		startPosition = transform.position;
	}

	// Start is called before the first frame update
	public void StartGame()
    {
		animatorRef.SetBool(StringsTable.isAlive, true);
		animatorRef.SetBool(StringsTable.isGrounded, true);

		transform.position = startPosition;
		currentHealthPoints = initialHealthPoints;
		currentManaPoints = initialManaPoints;
		sprintCoroutineRunning = false;
	}

    // Update is called once per frame
    void Update()
    {
		if (GameManager.GetInstance.CurrentGameState != EGameState.inGame)
		{
			return;
		}
		
		bool Isgrounded = IsTouchingTheGround();
		if ((Input.GetButtonDown(StringsTable.Jump) || Input.GetMouseButtonDown(0)) && Isgrounded)
        {
			Jump(false);
        }

		if (Input.GetMouseButtonDown(1) && Isgrounded)
		{
			Jump(true);
		}

		animatorRef.speed = Isgrounded ? Mathf.Clamp(Mathf.Abs(rigidbodyRef.velocity.x), 0f, 1f) : 1f;
		animatorRef.SetBool(StringsTable.isGrounded, Isgrounded);

		// use tireplayer coroutine to make a sprint
		if (Input.GetButtonDown(StringsTable.Sprint) && Isgrounded && !sprintCoroutineRunning)
		{
			StartCoroutine(StringsTable.TirePlayer);
		}
	}

	private void FixedUpdate()
	{
		if (GameManager.GetInstance.CurrentGameState != EGameState.inGame)
		{
			return;
		}

		if (Input.GetButton(StringsTable.Horizontal))
		{
			float currentSpeed = GetRunningSpeed();
			if (Input.GetAxisRaw(StringsTable.Horizontal) > 0)
			{
				if (rigidbodyRef.velocity.x < currentSpeed)
				{
					rigidbodyRef.velocity = new Vector2(currentSpeed, rigidbodyRef.velocity.y);
				}

				FlipAnimationSprite(false);
			}
			else if (Input.GetAxisRaw(StringsTable.Horizontal) < 0)
			{
				if (rigidbodyRef.velocity.x > -currentSpeed)
				{
					rigidbodyRef.velocity = new Vector2(-currentSpeed, rigidbodyRef.velocity.y);
				}

				FlipAnimationSprite(true);
			}
		}
	}

	private IEnumerator TirePlayer()
	{
		sprintCoroutineRunning = true;
		while (currentHealthPoints > minHealthPoints)
		{
			currentHealthPoints--;
			yield return new WaitForSeconds(consumeHealthPerTick);
		}
		yield return null;
		sprintCoroutineRunning = false;
	}

	private float GetRunningSpeed()
	{
		return sprintCoroutineRunning ? runningSpeed + GetCurrentRunningSpeed() : runningSpeed;
	}

	private float GetCurrentRunningSpeed()
	{
		// si a maxhealth va a X speed, a cuanta speed va por cada punto de vida
		return (runningSpeed/* - minRunningSpeed*/) * currentHealthPoints / maxHealthPoints;
	}

	private void Jump(bool isSuperJump)
	{
		if (isSuperJump && currentManaPoints >= consumeManaOnJump)
		{
			currentManaPoints -= consumeManaOnJump;
			// segunda ley de newton: F = m * a. Addforce -> a = F / m
			rigidbodyRef.AddForce(Vector2.up * jumpForce * jumpForceWithMana, ForceMode2D.Impulse);
		}
		else
		{
			rigidbodyRef.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
		}
	}

	private bool IsTouchingTheGround()
	{
		return Physics2D.Raycast(transform.position, Vector2.down, raycastToFloorDistance, groundLayer);
	}

	private void FlipAnimationSprite(bool flip)
	{
		if (spriteRendererRef.flipX != flip)
		{
			spriteRendererRef.flipX = flip;
		}
	}

	public void Kill()
	{
		GameManager.GetInstance.GameOver();
		animatorRef.SetBool(StringsTable.isAlive, false);

		float currentMaxScore = PlayerPrefs.GetFloat(StringsTable.maxscore, 0f);
		float currentDistance = GetDistance();
		if (currentMaxScore < currentDistance)
		{
			PlayerPrefs.SetFloat(StringsTable.maxscore, currentDistance);
		}

		StopCoroutine(StringsTable.TirePlayer);
	}

	public bool IsValidInitialPosition()
	{
		return transform.position.x == startPosition.x;
	}

	public float GetDistance()
	{
		// transform.position.x - startPosition.x
		//return transform.position.x - startPosition.x;
		float travelledDistance = Vector2.Distance(new Vector2(startPosition.x, 0f), new Vector2(transform.position.x, 0f));
		return travelledDistance;
	}

	public int HealthPoints
	{
		get => currentHealthPoints;
		set
		{
			currentHealthPoints += value;
			if (currentHealthPoints > maxHealthPoints)
			{
				currentHealthPoints = maxHealthPoints;
			}

			if (currentHealthPoints <= 0 && GameManager.GetInstance.CurrentGameState == EGameState.inGame)
			{
				Kill();
			}
		}
	}

	public int ManaPoints
	{
		get => currentManaPoints;
		set
		{
			currentManaPoints += value;
			if (currentManaPoints > maxManaPoints)
			{
				currentManaPoints = maxManaPoints;
			}
		}
	}

	public int MaxHealthPoints { get => maxHealthPoints; }
	public int MaxManaPoints { get => maxManaPoints; }
	public Rigidbody2D RigidbodyRef { get => rigidbodyRef; }
}
