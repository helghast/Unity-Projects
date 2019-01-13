using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
	private static LevelGenerator sharedInstance = null;
	public static LevelGenerator GetInstance { get => sharedInstance; }

	[SerializeField] private List<LevelBlock> allTheLevelBlocks = new List<LevelBlock>();
	[SerializeField] private List<LevelBlock> currentBlocks = new List<LevelBlock>();
	[SerializeField] private Transform levelStartPoint = null;

	private void Awake()
	{
		sharedInstance = this;
	}

	private void Start()
	{
		GenerateInitialBlocks();
	}

	public void AddLevelBlock()
	{
		int randomIndex = Random.Range(0, allTheLevelBlocks.Count);

		LevelBlock currentBlock = Instantiate<LevelBlock>(allTheLevelBlocks[randomIndex]);
		currentBlock.transform.SetParent(this.transform, false);

		Vector3 spawnPosition = (currentBlocks.Count == 0) ? levelStartPoint.position : currentBlocks[currentBlocks.Count - 1].ExitPoint.position;

		// resta de componentes de vector
		//Vector3 correction = new Vector3(spawnPosition.x - currentBlock.startPoint.position.x, spawnPosition.y - currentBlock.startPoint.position.y);
		// resta directa de vectores
		Vector3 correction = spawnPosition - currentBlock.StartPoint.position;
		
		currentBlock.transform.position = correction;
		currentBlocks.Add(currentBlock);
	}

	public void RemoveOldestLevelBlock()
	{
		LevelBlock oldestBlock = currentBlocks[0];
		currentBlocks.Remove(oldestBlock);
		Destroy(oldestBlock.gameObject);
	}

	public void RemoveAllTheBlocks()
	{
		while (currentBlocks.Count > 0)
		{
			RemoveOldestLevelBlock();
		}
	}

	public void GenerateInitialBlocks()
	{
		for (int i = 0; i < 2; ++i)
		{
			AddLevelBlock();
		}
	}
}
