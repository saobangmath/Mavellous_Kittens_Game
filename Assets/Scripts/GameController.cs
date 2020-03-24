using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public SimpleObjectPool answerButtonObjectPool;
    public Text questionText;
    public Text scoreDisplay;
    public Text feedback;
    public Text timeRemainingDisplay;
    public Transform answerButtonParent;

    public Button nextButton;

    public GameObject questionDisplay;
    public GameObject roundEndDisplay;
    public Text highScoreDisplay;

    private DataController dataController;
    private RoundData currentRoundData;
    private QuestionData[] questionPool;

    private bool isRoundActive = false;
    private float timeRemaining;
    private float currentTimeRemaining;
    private int playerScore;
    private int questionIndex;
    private int correctAnswerIndex;
    public string correctAnswer;
    private List<GameObject> answerButtonGameObjects = new List<GameObject>();

    void Start()
    {
        dataController = FindObjectOfType<DataController>();                                // Store a reference to the DataController so we can request the data we need for this round

        currentRoundData = dataController.GetCurrentRoundData();                            // Ask the DataController for the data for the current round. At the moment, we only have one round - but we could extend this
        questionPool = currentRoundData.questions;                                            // Take a copy of the questions so we could shuffle the pool or drop questions from it without affecting the original RoundData object

        timeRemaining = currentRoundData.timeLimitInSeconds;                                // Set the time limit for this round based on the RoundData object
        UpdateTimeRemainingDisplay();
        playerScore = 0;
        questionIndex = 0;

        ShowQuestion();
        isRoundActive = true;

        // set up next question button
        Button nextButton = this.nextButton.GetComponent<Button>();
        nextButton.onClick.AddListener(displayNextQuestion);
    }

    void Update()
    {
        if (isRoundActive)
        {
            timeRemaining -= Time.deltaTime;                                                // If the round is active, subtract the time since Update() was last called from timeRemaining
            UpdateTimeRemainingDisplay();

            if (timeRemaining <= 0f)                                                        // If timeRemaining is 0 or less, the round ends
            {
                EndRound();
            }
        }
    }

    void ShowQuestion()
    {
        RemoveAnswerButtons();

        QuestionData questionData = questionPool[questionIndex];                            // Get the QuestionData for the current question
        questionText.text = questionData.questionText;                                        // Update questionText with the correct text

        for (int i = 0; i < questionData.answers.Length; i ++)                                // For every AnswerData in the current QuestionData...
        {
            GameObject answerButtonGameObject = answerButtonObjectPool.GetObject();            // Spawn an AnswerButton from the object pool
            answerButtonGameObjects.Add(answerButtonGameObject);
            answerButtonGameObject.transform.SetParent(answerButtonParent);
            answerButtonGameObject.transform.localScale = Vector3.one;

            AnswerButton answerButton = answerButtonGameObject.GetComponent<AnswerButton>();
            answerButton.SetUp(questionData.answers[i]);                                    // Pass the AnswerData to the AnswerButton so the AnswerButton knows what text to display and whether it is the correct answer

            // search for the correct answer text
            if (questionData.answers[i].isCorrect) {
              correctAnswer = questionData.answers[i].answerText;
              correctAnswerIndex = i;
            }

            // make answer button interactable
            answerButtonGameObjects[i].GetComponent<Button>().interactable = true;

        }

        currentTimeRemaining = timeRemaining;
    }

    void RemoveAnswerButtons()
    {
        while (answerButtonGameObjects.Count > 0)                                            // Return all spawned AnswerButtons to the object pool
        {
            answerButtonObjectPool.ReturnObject(answerButtonGameObjects[0]);
            answerButtonGameObjects.RemoveAt(0);
        }
    }

    public void AnswerButtonClicked(bool isCorrect)
    {
        if (isCorrect)
        {
          // If the AnswerButton that was clicked was the correct answer, add points
          playerScore += currentRoundData.pointsAddedForCorrectAnswer;
          if (currentTimeRemaining - timeRemaining <= 5) {
            playerScore += 2;
          } else if (currentTimeRemaining - timeRemaining <= 10) {
            playerScore += 1;
          }
          // display feedback that answer was correct
          scoreDisplay.text = playerScore.ToString();
          feedback.text = "Correct Answer!";
        } else {
          // display feedback that answer was wrong & highlight correct answer
          feedback.text = "Wrong! Correct Answer is " + correctAnswer;
          answerButtonGameObjects[correctAnswerIndex].GetComponent<Image>().color = Color.green;
        }

        // disable answer buttons
        for (int i = 0; i < answerButtonGameObjects.Count; i++) {
          answerButtonGameObjects[i].GetComponent<Button>().interactable = false;;
        }

        // pause time
        Time.timeScale = 0f;
    }

    private void UpdateTimeRemainingDisplay()
    {
        timeRemainingDisplay.text = Mathf.Round(timeRemaining).ToString();
    }

    public void EndRound()
    {
        isRoundActive = false;
        // disable button display
        nextButton.gameObject.SetActive(false);

        dataController.SubmitNewPlayerScore(playerScore);
        highScoreDisplay.text = dataController.GetHighestPlayerScore().ToString();

        questionDisplay.SetActive(false);
        roundEndDisplay.SetActive(true);


    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MenuScreen");
    }

    public void displayNextQuestion() {
      // start timer again
      Time.timeScale = 1.0f;

      // remove feedback text
      feedback.text = "";

      if(questionPool.Length > questionIndex + 1)                                            // If there are more questions, show the next question
      {
          questionIndex++;
          ShowQuestion();
      }
      else                                                                                // If there are no more questions, the round ends
      {
          EndRound();
      }
    }
}
