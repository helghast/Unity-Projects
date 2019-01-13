using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
	[SerializeField] private int Value = 0;
	[SerializeField] private ECollectableType Type = ECollectableType.money;
	[SerializeField] private AudioClip collectSound = null;

	private SpriteRenderer spriteRendererRef = null;
	private CircleCollider2D collider2DRef = null;
	private AudioSource audioSourceRef = null;

	private void Awake()
	{
		spriteRendererRef = GetComponent<SpriteRenderer>();
		collider2DRef = GetComponent<CircleCollider2D>();
		audioSourceRef = GetComponent<AudioSource>();
		Visibility(true);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag(StringsTable.Player) && GameManager.GetInstance.CurrentGameState == EGameState.inGame)
		{
			Collect();
		}
	}

	private void Visibility(bool Value)
	{
		spriteRendererRef.enabled = collider2DRef.enabled = Value;
	}

	private void Collect()
	{
		Visibility(false);

		if (audioSourceRef != null && collectSound != null)
		{
			audioSourceRef.PlayOneShot(collectSound);
		}

		switch (Type)
		{
			case ECollectableType.health:
				PlayerController.GetInstance.HealthPoints = Value;
				break;
			case ECollectableType.mana:
				PlayerController.GetInstance.ManaPoints = Value;
				break;
			case ECollectableType.money:
				GameManager.GetInstance.CollectedObjects = Value;
				break;
		}		
	}
}
