using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMovement : MonoBehaviour
{
	[SerializeField] private bool movingForward = false;
	[SerializeField] private Enemy enemyRef = null;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag(StringsTable.Rock))
		{
			enemyRef.TurnAround = movingForward;
			movingForward = !movingForward;
		}
	}
}
