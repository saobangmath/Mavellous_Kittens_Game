[System.Serializable]
public class RoundData
{
	public string name;
	public string description;
	public int difficulty;
	public int timeLimitInSeconds;
	public int pointsAddedForCorrectAnswer;
	public QuestionData[] questions;
}
