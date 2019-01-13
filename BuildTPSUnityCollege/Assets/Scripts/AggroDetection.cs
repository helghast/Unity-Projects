using System;
using UnityEngine;
//using UnityEngine.AI;

public class AggroDetection : MonoBehaviour
{
	public event Action<Transform> OnAggro = delegate { };

	private void OnTriggerEnter(Collider other)
	{
		PlayerMovement player = other.GetComponent<PlayerMovement>();
		if (player != null)
		{
			Debug.Log("Aggrod");

			OnAggro(player.transform);

			//GetComponent<NavMeshAgent>().SetDestination(player.transform.position);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		
	}
}
