using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultsScreenLogic : MonoBehaviour
{
    [SerializeField] Image backgroundImage;

    [SerializeField] GameObject restartButton;
    //text that shows wether or not the player won
    [SerializeField] TextMeshProUGUI resultsText;
    //text that shows the player's final score
    [SerializeField] TextMeshProUGUI playerScoreText;
    //text that shows the min score for the level
    [SerializeField] TextMeshProUGUI minScoreText;
    //text that shows the secret score for the level
    [SerializeField] TextMeshProUGUI secretScoreText;
    [SerializeField] GameObject reminderText;

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
        //tell the player they epic win
        if (Manager.Instance.Score >= Manager.Instance.SecretScore)
        {
            resultsText.text = "EPIC Win!";
            reminderText.SetActive(true);

            //change the background to epic win color
            backgroundImage.color = Color.yellow;
            //play epic win sound
            SoundManager.Instance.PlaySound(5, 14);
        }
        //tell the player they won
        else if (Manager.Instance.Score >= Manager.Instance.MinScore)
        {
            resultsText.text = "You Win!";
            reminderText.SetActive(false);

            //change the background to win color
            backgroundImage.color = Color.green;
            //play win sound
            SoundManager.Instance.PlaySound(5, 13);
        }
        //tell the player they lost and disable next level option
        else
        {
            resultsText.text = "You Lose!";
            reminderText.SetActive(false);
            nextLevelButton.SetActive(false);

            //change the background to loss color   
            backgroundImage.color = Color.red;
            //play loss sound
            SoundManager.Instance.PlaySound(5, 12);
        }

        //show all the score stuff
        playerScoreText.text = Manager.Instance.Score.ToString();
        minScoreText.text = Manager.Instance.MinScore.ToString();
        secretScoreText.text = Manager.Instance.SecretScore.ToString();
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
