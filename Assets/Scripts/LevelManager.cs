using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //the ball prefab that will be spawned when needed
    [SerializeField]
    GameObject ballPrefab;

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
    int numberOfBalls;

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

        //spawn the first ball
        SpawnNewBall(ballSpawnPos, Vector3.zero);
    }

    public void UpdateScore(int scoreChange)
    {
        score += scoreChange;
        
        LevelUILogic.Instance.UpdateScore(score);
    }

    public void UpdateBalls(int ballsChange)
    {
        numberOfBalls += ballsChange;
        LevelUILogic.Instance.UpdateBalls(numberOfBalls);
    }

    /// <summary>
    /// spawns a new ball
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="force"></param>
    public void SpawnNewBall(GameObject spawnPos, Vector3 velocity)
    {
        GameObject newBall = Instantiate(ballPrefab);
        //set its position and velocity
        newBall.transform.position = spawnPos.transform.position;
        newBall.GetComponent<Rigidbody>().velocity = velocity;

        //add the ball to the list
        ballObjects.Add(newBall);
    }

    /// <summary>
    /// destroy a ball
    /// </summary>
    /// <param name="ball"></param>
    public void DestroyBall(GameObject ball)
    {
        ballObjects.Remove(ball);
        Destroy(ball);

        //if there are no more balls in play and the player still has more balls
        if (ballObjects.Count <= 0 && numberOfBalls > 0)
        {
            //spawn another ball and reduce the ball count
            SpawnNewBall(ballSpawnPos, Vector3.zero);
            UpdateBalls(-1);
        }
        //if there are no balls in play and the player has no more balls, check the win con
        else if (numberOfBalls <= 0 && ballObjects.Count <= 0)
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
