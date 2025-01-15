using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //the current score
    int score;
    //the minimum score needed to beat a level
    [SerializeField]
    int minScore;
    //the minimum score needed to get a secret.  0 for no secret
    [SerializeField]
    int secretScore;

    //the current number of balls
    [SerializeField]
    int balls;

    private void Start()
    {
        //load the level UI
        SceneManager.LoadScene("LevelUI", LoadSceneMode.Additive);

        score = 0;

        //update the score and ball UI with new values
        UpdateScore(0);
        UpdateBalls(balls);
    }

    public void UpdateScore(int scoreChange)
    {
        score += scoreChange;
        LevelUILogic.Instance.UpdateScore(scoreChange);
    }

    public void UpdateBalls(int ballsChange)
    {
        balls += ballsChange;
        LevelUILogic.Instance.UpdateBalls(ballsChange);

        if (balls <= 0)
        {
            if (score >= secretScore)
            {
                Debug.Log("you win but epic");
            }
            else if (score >= minScore)
            {
                Debug.Log("you win");
            }
            else
            {
                Debug.Log("you lose");
            }
        }
    }
}
