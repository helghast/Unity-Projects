using UnityEngine;

public class Cube : MonoBehaviour, IPooledObject
{
	[SerializeField] private float upForce = 1.0f;
	[SerializeField] private float sideForce = 0.1f;

    public void OnObjectSpawn()
    {
		float xForce = Random.Range(-sideForce, sideForce);
		float yForce = Random.Range(upForce * 0.5f, upForce);
		float zForce = Random.Range(-sideForce, sideForce);

		GetComponent<Rigidbody>().velocity = new Vector3(xForce, yForce, zForce); 
	}
}
