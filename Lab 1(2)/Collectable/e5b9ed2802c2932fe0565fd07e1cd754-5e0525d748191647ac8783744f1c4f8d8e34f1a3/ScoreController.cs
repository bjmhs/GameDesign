using System;
using UnityEngine;

public class ScoreController
{
	public event Action<int> OnScoreChanged = delegate { };

	private int currentScore;

	public void AddScore(int amountToAdd)
	{
		currentScore += amountToAdd;
		OnScoreChanged(currentScore);
		Debug.Log("New Score: " + currentScore);
	}
}