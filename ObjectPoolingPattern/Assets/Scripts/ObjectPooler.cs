/** Original source code by https://www.youtube.com/watch?v=tdSmKaJvCoA */

using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
	#region Singleton
	private static ObjectPooler Instance = null;

	private void Awake()
	{
		Instance = this;
	}

	public static ObjectPooler GetInstace()
	{
		return Instance;
	}
	#endregion

	[SerializeField] private List<Pool> pools;
	private Dictionary<EObjectType, Queue<GameObject>> poolDictionary;

    // Start is called before the first frame update
    void Start()
    {
		poolDictionary = new Dictionary<EObjectType, Queue<GameObject>>();

		foreach (Pool pool in pools)
		{
			Queue<GameObject> objectPool = new Queue<GameObject>();

			for (int i = 0; i < pool.size; ++i)
			{
				GameObject obj = Instantiate(pool.prefab);
				obj.SetActive(false);
				objectPool.Enqueue(obj);
			}

			poolDictionary.Add(pool.tag, objectPool);
		}
    }

	public GameObject SpawnFromPool(EObjectType tag, Vector3 position, Quaternion rotation)
	{
		if (!poolDictionary.ContainsKey(tag))
		{
			Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
			return null;
		}

		GameObject objectToSpawn = poolDictionary[tag].Dequeue();
		objectToSpawn.SetActive(true);
		objectToSpawn.transform.position = position;
		objectToSpawn.transform.rotation = rotation;

		IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();
		if (pooledObj != null)
		{
			pooledObj.OnObjectSpawn();
		}

		poolDictionary[tag].Enqueue(objectToSpawn);

		return objectToSpawn;
	}
}
