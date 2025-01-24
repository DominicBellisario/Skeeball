using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //the level number
    [SerializeField]
    int levelNumber;

    //the ball prefab that will be spawned when needed
    [SerializeField]
    GameObject ballPrefab;

    //the place in the level the balls spawn in
    [SerializeField]
    GameObject ballSpawnPos;

    //a list of all balls currently in the level
    [SerializeField]
    List<GameObject> ballObjects;

    GameObject startingBall;

    //camera that follows the first ball in the list
    [SerializeField]
    GameObject ballCamera;
    Vector3 ballCameraOffset;

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

    //the time between the last active ball and a new spawn
    float timeBetweenBalls;

    //how many of each powerup the player has
    [SerializeField]
    int goldBallPow;
    [SerializeField]
    int markedBallPow;
    [SerializeField]
    int triBallPow;

    public int LevelNumber { get { return levelNumber; } }
    public int Score { get { return score; } }
    public int MinScore { get { return minScore; } }
    public int SecretScore { get { return secretScore; } }
    public GameObject BallCamera { get { return ballCamera; } }
    public GameObject StartingBall { get { return startingBall; } }
    public int GoldBallPow { get { return goldBallPow; } set { goldBallPow = value; } }
    public int MarkedBallPow { get { return markedBallPow; } set { markedBallPow = value; } }
    public int TriBallPow { get { return triBallPow; } set { triBallPow = value; } }

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
        }
    }

    private void Start()
    {
        score = 0;
        timeBetweenBalls = 1;
        ballCameraOffset = new Vector3(0, 3, -5);

        //update the score and ball UI with starting values
        UpdateScore(0);
        UpdateBalls(0);

        //spawn the first ball
        StartCoroutine(SpawnNewStartingBall());
    }

    public void Update()
    {
        //ball camera follows the first ball in the list
        if (ballObjects.Count >= 1)
        {
            ballCamera.transform.position = ballObjects[0].transform.position + ballCameraOffset;
        }
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
    public void SpawnNewBall(Vector3 spawnPos, Vector3 force, bool gold, bool marked, bool tri)
    {
        GameObject newBall = Instantiate(ballPrefab);

        //set its position and velocity
        newBall.transform.position = spawnPos;
        newBall.GetComponent<Rigidbody>().AddForce(force);

        //set the powerup states
        if (gold) { newBall.GetComponent<BallEffects>().ToggleGoldBall(); }
        if (marked) { newBall.GetComponent<BallEffects>().ToggleMarkedBall(); }

        //if this is the first ball, set it
        if (ballObjects.Count <= 0)
        {
            startingBall = newBall;
        }
        //starting ball only enables trail when launched, not when spawn
        else
        {
            newBall.GetComponent<BallEffects>().ActivateParticleTrail();
        }

        

        //add the ball to the list
        ballObjects.Add(newBall);
    }

    /// <summary>
    /// spawns a new ball after a delay and reduces the ball count
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnNewStartingBall()
    {
        yield return new WaitForSeconds(timeBetweenBalls);
        SpawnNewBall(ballSpawnPos.transform.position, Vector3.zero, false, false, false);
        UpdateBalls(-1);
        //switch to main camera view
        BallCamera.SetActive(false);
    }

    /// <summary>
    /// destroy a ball
    /// </summary>
    /// <param name="ball"></param>
    public void DestroyBall(GameObject ball)
    {
        ballObjects.Remove(ball);
        ball.GetComponent<BallEffects>().SeparateParticleSystem();
        Destroy(ball);

        //if there are no more balls in play and the player still has more balls
        if (ballObjects.Count <= 0 && numberOfBalls > 0)
        {
            //spawn a new ball
            StartCoroutine(SpawnNewStartingBall());
        }
        //if there are no balls in play and the player has no more balls
        else if (numberOfBalls <= 0 && ballObjects.Count <= 0)
        {
            //disable level ui event handler
            LevelUILogic.Instance.EventHandler.SetActive(false);

            //bring up the results screen
            SceneManager.LoadScene("ResultsScreen", LoadSceneMode.Additive);

            //pause the game
            Time.timeScale = 0;
        }
    }

    public void SwitchCameraView()
    {
        ballCamera.SetActive(!ballCamera.activeInHierarchy);
    }
}
