using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	//singleton pattern
	private static GameManager sharedInstance = null;
	public static GameManager GetInstance { get => sharedInstance; }

	private int collectedObjects = 0;

	[SerializeField] private EGameState currentGameState = EGameState.menu;
	[SerializeField] private Canvas menuCanvas = null;
	[SerializeField] private Canvas gameCanvas = null;
	[SerializeField] private Canvas gameOverCanvas = null;

	private void Awake()
	{
		// due inherint from Monobehavior, you cannot use constructor. Use Awake instead
		sharedInstance = this;
	}

	private void Start()
	{
		BackToMenu();
	}

	private void Update()
	{
		//if (Input.GetButtonDown("Start") && currentGameState != EGameState.inGame)
		//{
		//	StartGame();
		//}

		if (Input.GetButtonDown(StringsTable.Pause))
		{
			BackToMenu();
		}

		// never runs in editor or webplayer. dont use on iOS.
#if UNITY_EDITOR
		if (Input.GetButtonDown(StringsTable.ExitGame))
		{
			ExitGame();
		}
#endif
	}

	public void StartGame()
	{
		SetGameState(EGameState.inGame);

		// only remove and add new blocks after players deads and restart
		if (!PlayerController.GetInstance.IsValidInitialPosition())
		{
			LevelGenerator.GetInstance.RemoveAllTheBlocks();
			LevelGenerator.GetInstance.GenerateInitialBlocks();
		}

		// set player initial anims and start position
		PlayerController.GetInstance.StartGame();

		CameraFollow.GetInstance.ResetCameraPosition();

		collectedObjects = 0;
	}

	public void GameOver()
	{
		SetGameState(EGameState.gameOver);
	}

	public void BackToMenu()
	{
		SetGameState(EGameState.menu);
	}

	public void ExitGame()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	private void SetGameState(EGameState newGameState)
	{
		switch(newGameState)
		{
			case EGameState.menu:
				menuCanvas.enabled = true;
				gameCanvas.enabled = gameOverCanvas.enabled = !menuCanvas.enabled;
				break;
			case EGameState.inGame:
				gameCanvas.enabled = true;
				menuCanvas.enabled = gameOverCanvas.enabled = !gameCanvas.enabled;
				break;
			case EGameState.gameOver:
				gameOverCanvas.enabled = true;
				menuCanvas.enabled = gameCanvas.enabled = !gameOverCanvas.enabled;
				break;
		};

		currentGameState = newGameState;
	}

	public int CollectedObjects { get => collectedObjects; set => collectedObjects += value; }
	public EGameState CurrentGameState { get => currentGameState; }
}
