using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
	private ObjectPooler objectPooler = null;
	private readonly EObjectType type = EObjectType.Cube;

	private void Start()
	{
		objectPooler = ObjectPooler.GetInstace();
	}

	void Update()
    {
		objectPooler.SpawnFromPool(type, transform.position, Quaternion.identity);
    }
}
