using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //the place in the level the balls spawn in
    [SerializeField]
    GameObject ballSpawnPos;

    //a list of all balls currently in the level
    [SerializeField]
    List<GameObject> ballObjects;

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

    public static LevelManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        score = 0;

        //update the score and ball UI with starting values
        UpdateScore(0);
        UpdateBalls(0);
    }

    public void UpdateScore(int scoreChange)
    {
        score += scoreChange;
        
        LevelUILogic.Instance.UpdateScore(score);
    }

    public void UpdateBalls(int ballsChange)
    {
        balls += ballsChange;
        LevelUILogic.Instance.UpdateBalls(balls);

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

    /// <summary>
    /// spawns a new ball with a given position and force
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="force"></param>
    public void SpawnNewBall(Vector3 pos, Vector3 force)
    {
        //GameObject newBall = Instantiate()
    }
}
