using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MazeCombinationRooms
{
	none,
	wantGeneratePassageInSameRoom,
	wantGenerateLargeRooms
}

public class Maze : MonoBehaviour {
    public IntVector2 size;
    public MazeCell cellPrefab;
    private MazeCell[,] cells;

	public MazePassage passagePrefab;
	public MazeWall[] wallPrefabs;
	public MazeDoor doorPrefab;

	public MazeRoomSettings[] roomSettings;
	private List<MazeRoom> rooms = new List<MazeRoom>();

	[Range(0f, 1f)]
	public float doorProbability;

	/** 0.01f by default */
	public bool wantGenerationDelay;
	/** false by default */
	[Range(0f, 1f)]
	public float generationStepDelay;
	/** none by default */
	public MazeCombinationRooms Combinations;

	public MazeCell GetCell(IntVector2 coordinates)
	{
		return cells[coordinates.x, coordinates.z];
	}

    public IEnumerator GenerateDelayed()
    {
        WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
        cells = new MazeCell[size.x, size.z];

		List<MazeCell> activeCells = new List<MazeCell>();
		DoFirstGenerationStep(activeCells);

		while (activeCells.Count > 0)
		{
			yield return delay;
			DoNextGenerationStep(activeCells);
		}

		StaticBatchingUtility.Combine(gameObject);
	}

    public void Generate()
    {
        cells = new MazeCell[size.x, size.z];

		List<MazeCell> activeCells = new List<MazeCell>();
		DoFirstGenerationStep(activeCells);

		while (activeCells.Count > 0)
		{
			DoNextGenerationStep(activeCells);
		}

		StaticBatchingUtility.Combine(gameObject);
	}

	private void DoFirstGenerationStep(List<MazeCell> activeCells)
	{
		MazeCell newCell = CreateCell(RandomCoordinates);
		newCell.Initialize(CreateRoom(-1));
		activeCells.Add(newCell);
	}

	private void DoNextGenerationStep(List<MazeCell> activeCells)
	{
		int currentIndex = activeCells.Count - 1;
		MazeCell currentCell = activeCells[currentIndex];

		if (currentCell.IsFullyInitialized)
		{
			activeCells.RemoveAt(currentIndex);
			return;
		}

		MazeDirection direction = currentCell.RandomUninitializedDirection;
		IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();

		if (ContainsCoordinates(coordinates))
		{
			MazeCell neighbor = GetCell(coordinates);
			if (neighbor == null)
			{
				neighbor = CreateCell(coordinates);
				CreatePassage(currentCell, neighbor, direction);
				activeCells.Add(neighbor);
			}
			else if (Combinations == MazeCombinationRooms.wantGeneratePassageInSameRoom && currentCell.room == neighbor.room)
			{
				CreatePassageInSameRoom(currentCell, neighbor, direction);
			}
			else if (Combinations == MazeCombinationRooms.wantGenerateLargeRooms && currentCell.room.settingsIndex == neighbor.room.settingsIndex)
			{
				CreateLargeRooms(currentCell, neighbor, direction);
			}
			else
			{
				CreateWall(currentCell, neighbor, direction);
			}
		}
		else
		{
			CreateWall(currentCell, null, direction);
		}
	}

	public IntVector2 RandomCoordinates
	{
		get
		{
			return new IntVector2(UnityEngine.Random.Range(0, size.x), UnityEngine.Random.Range(0, size.z));
		}
	}

	public bool ContainsCoordinates(IntVector2 coordinate)
	{
		return coordinate.x >= 0 && coordinate.x < size.x && coordinate.z >= 0 && coordinate.z < size.z;
	}

	private void CreatePassage(MazeCell currentCell, MazeCell neighbor, MazeDirection direction)
	{
		MazePassage prefab = UnityEngine.Random.value < doorProbability ? doorPrefab : passagePrefab;
		MazePassage passage = Instantiate<MazePassage>(prefab);
		passage.Initialize(currentCell, neighbor, direction);
		passage = Instantiate<MazePassage>(prefab);
		if (passage is MazeDoor)
		{
			neighbor.Initialize(CreateRoom(currentCell.room.settingsIndex));
		}
		else
		{
			neighbor.Initialize(currentCell.room);
		}
		passage.Initialize(neighbor, currentCell, direction.GetOpposite());
	}

	private void CreateWall(MazeCell currentCell, MazeCell neighbor, MazeDirection direction)
	{
		MazeWall wall = Instantiate<MazeWall>(wallPrefabs[UnityEngine.Random.Range(0, wallPrefabs.Length)]);
		wall.Initialize(currentCell, neighbor, direction);
		if (neighbor != null)
		{
			wall = Instantiate<MazeWall>(wallPrefabs[UnityEngine.Random.Range(0, wallPrefabs.Length)]);
			wall.Initialize(neighbor, currentCell, direction.GetOpposite());
		}
	}

	private MazeCell CreateCell(IntVector2 coordinates)
    {
        MazeCell newCell = Instantiate<MazeCell>(cellPrefab);
        cells[coordinates.x, coordinates.z] = newCell;
        newCell.coordinates = coordinates;
        newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z;
        newCell.transform.parent = transform;
        newCell.transform.localPosition = new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 0f, coordinates.z - size.z * 0.5f + 0.5f);
		return newCell;
    }

	private MazeRoom CreateRoom(int indexToExclude)
	{
		MazeRoom newRoom = ScriptableObject.CreateInstance<MazeRoom>();
		newRoom.settingsIndex = UnityEngine.Random.Range(0, roomSettings.Length);
		if (newRoom.settingsIndex == indexToExclude)
		{
			newRoom.settingsIndex = (newRoom.settingsIndex + 1) % roomSettings.Length;
		}
		newRoom.settings = roomSettings[newRoom.settingsIndex];
		rooms.Add(newRoom);
		return newRoom;
	}

	//this method will simply creates a passage between two cells with no chance of a door. Use a public bool to use or not in DoNextGenerationStep 
	private void CreatePassageInSameRoom(MazeCell currentCell, MazeCell neighborCell, MazeDirection direction)
	{
		MazePassage passage = Instantiate<MazePassage>(passagePrefab);
		passage.Initialize(currentCell, neighborCell, direction);
		passage = Instantiate<MazePassage>(passagePrefab);
		passage.Initialize(neighborCell, currentCell, direction.GetOpposite());
	}

	private void CreateLargeRooms(MazeCell currentCell, MazeCell neighborCell, MazeDirection direction)
	{
		MazePassage passage = Instantiate<MazePassage>(passagePrefab);
		passage.Initialize(currentCell, neighborCell, direction);
		passage = Instantiate<MazePassage>(passagePrefab);
		passage.Initialize(neighborCell, currentCell, direction.GetOpposite());

		if (currentCell.room != neighborCell.room)
		{
			MazeRoom roomToAssimilate = neighborCell.room;
			currentCell.room.Assimilate(roomToAssimilate);
			rooms.Remove(roomToAssimilate);
			Destroy(roomToAssimilate);
		}
	}
}
