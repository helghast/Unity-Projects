using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Our GameManager component simply begins the game when its Start method is called. 
 * We also let it restart the game whenever the player presses space. 
 * To support that, we need to check each update whether the space key has been pressed. */
public class GameManager : MonoBehaviour {

    public Maze mazePrefab;
    private Maze mazeInstance;

	private IEnumerator coroutine;

	public Player playerPrefab;
	private Player playerInstance;

   	// Use this for initialization
	private void Start () {
        StartCoroutine(BeginGame());
	}

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }
	}

    private IEnumerator BeginGame()
    {
		Camera.main.clearFlags = CameraClearFlags.Skybox;

		mazeInstance = Instantiate<Maze>(mazePrefab);
        if (mazeInstance.wantGenerationDelay)
        {
			coroutine = mazeInstance.GenerateDelayed();
			yield return StartCoroutine(coroutine);
		}
        else
        {
            mazeInstance.Generate();
        }

		playerInstance = Instantiate<Player>(playerPrefab);
		playerInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));

		Camera.main.clearFlags = CameraClearFlags.Depth;
		Camera.main.rect = new Rect(0f, 0f, 0.5f, 0.5f);
	}

    private void RestartGame()
    {
        //if (mazeInstance.wantGenerationDelay)
        StopAllCoroutines();
        Destroy(mazeInstance.gameObject);

		if (playerInstance != null)
		{
			Destroy(playerInstance.gameObject);
		}

		StartCoroutine(BeginGame());
    }
}
