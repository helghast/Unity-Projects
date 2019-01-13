using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBlock : MonoBehaviour
{
	[SerializeField] private Transform startPoint = null;
	[SerializeField] private Transform exitPoint = null;

	public Transform StartPoint { get => startPoint; }
	public Transform ExitPoint { get => exitPoint; }
}
