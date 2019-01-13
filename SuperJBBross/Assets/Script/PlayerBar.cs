using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBar : MonoBehaviour
{
	private Slider sliderRef;
	[SerializeField] private EBarType Type = EBarType.health;

	private void Awake()
	{
		sliderRef = GetComponent<Slider>();
	}

	// Start is called before the first frame update
	void Start()
    {
		switch (Type)
		{
			case EBarType.health:
				sliderRef.maxValue = PlayerController.GetInstance.MaxHealthPoints;
				break;
			case EBarType.mana:
				sliderRef.maxValue = PlayerController.GetInstance.MaxManaPoints;
				break;
		}
	}

    // Update is called once per frame
    void Update()
    {
		if (GameManager.GetInstance.CurrentGameState == EGameState.inGame)
		{
			switch (Type)
			{
				case EBarType.health:
					sliderRef.value = PlayerController.GetInstance.HealthPoints;
					break;
				case EBarType.mana:
					sliderRef.value = PlayerController.GetInstance.ManaPoints;
					break;
			}
		}
	}
}
