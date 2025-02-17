using UnityEngine;
using TMPro;

public class ResultsScreenLogic : MonoBehaviour
{
    [SerializeField] GameObject restartButton;
    //text that shows wether or not the player won
    [SerializeField] TextMeshProUGUI resultsText;
    //text that shows the player's final score
    [SerializeField] TextMeshProUGUI playerScoreText;
    //text that shows the min score for the level
    [SerializeField] TextMeshProUGUI minScoreText;
    //text that shows the secret score for the level
    [SerializeField] TextMeshProUGUI secretScoreText;

    //the next level button.  disabled if the player lost
    [SerializeField] GameObject nextLevelButton;

    private void Start()
    {
        //no restart in endless mode
        if (Manager.Instance.Endless)
        {
            restartButton.SetActive(false);
        }
        //check how the player did
        //tell the player they epic win and (later), give them an unlock
        if (Manager.Instance.Score >= Manager.Instance.SecretScore)
        {
            resultsText.text = "You Win but EPIC!";
        }
        //tell the player they won
        else if (Manager.Instance.Score >= Manager.Instance.MinScore)
        {
            resultsText.text = "You Win!";
        }
        //tell the player they lost and disable next level option
        else
        {
            resultsText.text = "You Lose!";
            nextLevelButton.SetActive(false);
        }

        //show all the score stuff
        playerScoreText.text = "Your Score: " + Manager.Instance.Score;
        minScoreText.text = "Min Score: " + Manager.Instance.MinScore;
        secretScoreText.text = "Secret Score: " + Manager.Instance.SecretScore;
    }

    //bring back the level ui and unpause
    public void RetryLevel()
    {
        Time.timeScale = 1;
        SceneHandler.Instance.LoadLevel("L" + Manager.Instance.CurrentLevelNumber.ToString());
        Manager.Instance.ResetValues();
    }

    public void NextLevel()
    {
        Time.timeScale = 1;
        int nextLevelNum = Manager.Instance.CurrentLevelNumber + 1;
        SceneHandler.Instance.LoadLevel("L" + nextLevelNum.ToString());
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
