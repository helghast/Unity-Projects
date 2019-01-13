using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewInGame : MonoBehaviour
{
	[SerializeField] private Text collectableLabel = null;
	[SerializeField] private Text scoreLabel = null;
	[SerializeField] private Text maxScoreLabel = null;

	// Update is called once per frame
	void Update()
    {
        if (GameManager.GetInstance.CurrentGameState != EGameState.menu)
        {
			int collectables = GameManager.GetInstance.CollectedObjects;
			collectableLabel.text = collectables.ToString();
        }

		if (GameManager.GetInstance.CurrentGameState == EGameState.inGame)
		{
			float distance = PlayerController.GetInstance.GetDistance();
			scoreLabel.text = StringsTable.Scoretitle + distance.ToString("f1");

			if (maxScoreLabel)
			{
				float maxscore = PlayerPrefs.GetFloat(StringsTable.maxscore, 0f);
				maxScoreLabel.text = StringsTable.MaxScoreTitle + maxscore.ToString("f1");
			}
		}
	}
}
