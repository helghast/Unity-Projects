using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveZone : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag(StringsTable.Player))
		{
			LevelGenerator.GetInstance.AddLevelBlock();
			LevelGenerator.GetInstance.RemoveOldestLevelBlock();
		}
	}
}
