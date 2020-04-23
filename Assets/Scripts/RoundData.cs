using System.Collections.Generic;

/// <summary>
/// This data structure stores the information (e.g. level id, list of qns) for each level. 
/// </summary>
[System.Serializable]
public class RoundData
{
	public string lvlId;
	public string name;
	public string boss;
	public int difficulty;
	public int timeLimitInSeconds;
	public int pointsAddedForCorrectAnswer;
	public List<QnA> questions= new List<QnA>();
}
