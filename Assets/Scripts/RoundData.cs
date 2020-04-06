using System.Collections.Generic;

[System.Serializable]
public class RoundData
{
	public string name;
	public string boss;
	public int difficulty;
	public int timeLimitInSeconds;
	public int pointsAddedForCorrectAnswer;
	public List<QnA> questions;
}
