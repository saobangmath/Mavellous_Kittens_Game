using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnswerButton : MonoBehaviour
{
	public Text answerText;

	private GameController gameController;
	private AnswerData answerData;

	void Start()
	{
		gameController = FindObjectOfType<GameController>();
	}

	public void SetUp(AnswerData data)
	{
		answerData = data;
		answerText.text = answerData.answerText;
		this.GetComponent<Image>().color = Color.white;
	}

	public void HandleClick()
	{
		gameController.AnswerButtonClicked(answerData.isCorrect);

		if (answerData.isCorrect) {
			this.GetComponent<Image>().color = Color.green;
		} else {
			this.GetComponent<Image>().color = Color.yellow;
		}
	}

	public void disableAnswerButton()
	{
		this.GetComponent<Button>().interactable = false;
	}
}
