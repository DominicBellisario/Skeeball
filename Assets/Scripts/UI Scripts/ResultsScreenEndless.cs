using TMPro;
using UnityEngine;

public class ResultsScreenEndless : MonoBehaviour
{
    //text that shows wether or not the player won
    [SerializeField] TextMeshProUGUI resultsText;
    [SerializeField] TextMeshProUGUI levelScoreText;
    [SerializeField] TextMeshProUGUI totalScoreText;
    [SerializeField] TextMeshProUGUI levelRemainingText;
    //the next level button.  disabled if the player lost
    [SerializeField] GameObject nextLevelButton;

    private void Start()
    {
        //check how the player did
        //if they beat the level, tell them
        if (Manager.Instance.Score >= Manager.Instance.MinScore)
        {
            resultsText.text = "Level Complete!";
        }
        //tell the player they lost and change some ui
        else
        {
            //tell them they lost
            resultsText.text = "Game Over!";
            //disable next level button and round info
            nextLevelButton.SetActive(false);
            levelRemainingText.gameObject.SetActive(false);
            //set level score color to red
            levelScoreText.color = Color.red;
        }

        //update all of the text
        levelScoreText.text = Manager.Instance.Score + " / " + Manager.Instance.MinScore;
        totalScoreText.text = Manager.Instance.TotalPoints.ToString();
        levelRemainingText.text = "completed " + Manager.Instance.CompletedLevelsInRound + " / " + Manager.Instance.LevelsInCurrentRound + " levels";
    }

    public void NextLevel()
    {
        Time.timeScale = 1;
        Manager.Instance.GoToNextEndlessLevel();
        Manager.Instance.ResetValues();
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneHandler.Instance.LoadScene("HomeScreen");
        Manager.Instance.ResetValues();
        Manager.Instance.EndlessReset();
    }
}
