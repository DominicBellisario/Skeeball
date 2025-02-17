using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] GameObject ballPrefab;
    [SerializeField] GameObject beanbagPrefab;
    [SerializeField] Vector3 objectCameraOffset;
    [SerializeField] bool switchCameraOnLaunch;
    //the time between the last active objects and a new spawn
    [SerializeField] float timeBetweenObjects;

    //the place in the level the objects spawn in
    GameObject objectSpawnPos;
    //a list of all objects currently in the level
    [SerializeField] List<GameObject> objects;
    GameObject startingObject;
    //camera that follows the first object in the list
    GameObject objectCamera;


    //the current score
    int score;
    //the minimum score needed to beat a level
    int minScore;
    //the minimum score needed to get a secret.  0 for no secret
    int secretScore;

    //the current number of objects
    int numberOfObjects;
    int currentLevelNumber;

    //how many of each powerup the player has
    int goldBallPow;
    int markedBallPow;
    int triBallPow;
    int lobBallPow;
    bool lobBallEnabled;

    //endless
    [SerializeField] int[] easyLevels;
    [SerializeField] int[] mediumLevels;
    [SerializeField] int[] hardLevels;
    int[] currentDifficulty;
    [SerializeField] int totalPoints = 0;
    [SerializeField] float multiplier = 1f;
    int completedLevelsInRound = 0;
    int levelsInCurrentRound = 4;
    int currentRoundNumber = 1;
    List<int> playedLevels = new();


    bool endless;
    public static Manager Instance { get; private set; }
    public int CurrentLevelNumber { get { return currentLevelNumber; } }
    public int Score { get { return score; } }
    public int MinScore { get { return minScore; } }
    public int SecretScore { get { return secretScore; } }
    public GameObject ObjectCamera { get { return objectCamera; } }
    public GameObject StartingObject { get { return startingObject; } }
    public bool SwitchCameraOnLaunch { get { return switchCameraOnLaunch; } }
    public int GoldBallPow { get { return goldBallPow; } set { goldBallPow = value; } }
    public int MarkedBallPow { get { return markedBallPow; } set { markedBallPow = value; } }
    public int TriBallPow { get { return triBallPow; } set { triBallPow = value; } }
    public int LobBallPow { get { return lobBallPow; } set { lobBallPow = value; } }
    public bool LobBallEnabled { get { return lobBallEnabled; } set { lobBallEnabled = value; } }
    //endless
    public bool Endless { get { return endless; } }
    public int TotalPoints { get { return totalPoints; } }
    public float Multiplier { get { return multiplier; } }
    public int CompletedLevelsInRound { get { return completedLevelsInRound; } }
    public int LevelsInCurrentRound { get { return levelsInCurrentRound; } }
    public int CurrentRoundNumber { get { return currentRoundNumber; } }

    protected virtual void Awake()
    {
        //create singleton instance
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        endless = false;
        objects = new List<GameObject>();
    }

    public void Update()
    {
        //ball camera follows the first ball in the list
        if (objects.Count >= 1)
        {
            objectCamera.transform.position = objects[0].transform.position + objectCameraOffset;
        }
    }

    public virtual void RecieveValues(int _currentLevelNumber, GameObject _objectSpawnPos, GameObject _objectCamera, int _startingNumberOfObjects, int _minScore, int _secretScore, int _goldBallPow, int _markedBallPow, int _triBallPow, int _lobBallPow)
    {
        currentLevelNumber = _currentLevelNumber;
        objectSpawnPos = _objectSpawnPos;
        objectCamera = _objectCamera;
        numberOfObjects = _startingNumberOfObjects;
        minScore = _minScore;
        secretScore = _secretScore;
        goldBallPow = _goldBallPow;
        markedBallPow = _markedBallPow;
        triBallPow = _triBallPow;
        lobBallPow = _lobBallPow;

        //update the score and ball UI with starting values
        UpdateScore(0);
        UpdateObjects(0);

        //spawn the first ball
        StartCoroutine(SpawnNewStartingBall());
    }

    public virtual void ResetValues()
    {
        objects.Clear();
        UpdateScore(-score);
        LobBallEnabled = false;
        StopAllCoroutines();
    }

    public virtual void EndlessReset()
    {
        endless = false;
        playedLevels.Clear();
        currentLevelNumber = 0;
        totalPoints = 0;
        multiplier = 1;
    }

    public void BeginEndlessMode()
    {
        Instance.endless = true;
        Instance.currentDifficulty = easyLevels;
        Instance.GoToNextEndlessLevel();
    }

    public void GoToNextEndlessLevel()
    {
        //make a list of all unplayed levels of the selected difficulty
        List<int> unplayedLevels = new();
        foreach (int levelNum in currentDifficulty)
        {
            if (!playedLevels.Contains(levelNum))
            {
                unplayedLevels.Add(levelNum);
            }
        }
        //go to next difficulty if all levels are played in the current difficulty
        if (unplayedLevels.Count <= 0)
        {
            Debug.Log("done with easy");
        }
        //pick a random number from the unplayed levels
        int levelNumToLoad = unplayedLevels[Helper.Instance.RandomInt(0, unplayedLevels.Count - 1)];
        
        //add it to played levels
        playedLevels.Add(levelNumToLoad);
        //set it as the current level
        currentLevelNumber = levelNumToLoad;
        //load a random level from these levels
        SceneHandler.Instance.LoadLevel("EL" + levelNumToLoad.ToString());
    }

    public void UpdateScore(int scoreChange)
    {
        if (endless) { scoreChange = Mathf.RoundToInt(scoreChange * multiplier); }
        score += scoreChange;
        LevelUILogic.Instance.UpdateScore(score);
    }

    public void UpdateTotalScore(int scoreChange)
    {
        scoreChange = Mathf.RoundToInt(scoreChange * multiplier);
        totalPoints += scoreChange;
        LevelUILogic.Instance.UpdateTotalScore(totalPoints);
    }

    public void UpdateObjects(int ballsChange)
    {
        numberOfObjects += ballsChange;
        LevelUILogic.Instance.UpdateBalls(numberOfObjects);
    }

    /// <summary>
    /// spawns a new ball
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="force"></param>
    public GameObject SpawnNewObject(GameObject objectPrefab, Vector3 spawnPos, Vector3 force, bool gold, bool marked, bool tri)
    {
        GameObject newObject = Instantiate(objectPrefab);

        //set its position and velocity
        newObject.transform.position = spawnPos;
        newObject.GetComponent<Rigidbody>().AddForce(force);

        //set the powerup states
        if (gold) { newObject.GetComponent<ObjectEffects>().ToggleGoldBall(); }
        if (marked) { newObject.GetComponent<ObjectEffects>().ToggleMarkedBall(); }
        if (tri) { newObject.GetComponent<ObjectEffects>().ToggleTriBall(); }

        //if this is the first ball, set it
        if (objects.Count <= 0)
        {
            startingObject = newObject;
        }
        //starting ball only enables trail when launched, not when spawn
        else
        {
            newObject.GetComponent<ObjectEffects>().ActivateParticleTrail();
        }
        //add the ball to the list
        objects.Add(newObject);

        return newObject;
    }

    /// <summary>
    /// spawns a new ball after a delay and reduces the ball count
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnNewStartingBall()
    {
        yield return new WaitForSeconds(timeBetweenObjects);
        SpawnNewObject(ballPrefab, objectSpawnPos.transform.position, Vector3.zero, false, false, false);
        UpdateObjects(-1);
        //switch to main camera view
        objectCamera.SetActive(false);
    }

    private IEnumerator EndLevel()
    {
        yield return new WaitForSeconds(timeBetweenObjects);
        //disable level ui event handler
        LevelUILogic.Instance.EventHandler.SetActive(false);
        //bring up the results screen
        if (!endless) { SceneHandler.Instance.LoadSceneAdditively("ResultsScreen"); }
        else 
        { 
            completedLevelsInRound++;
            SceneHandler.Instance.LoadSceneAdditively("ResultsScreenEndless"); 
        }
        
        //pause the game
        Time.timeScale = 0;
    }

    /// <summary>
    /// destroy an object
    /// </summary>
    /// <param name="ball"></param>
    public void DestroyObject(GameObject objectToDestroy)
    {
        objects.Remove(objectToDestroy);
        objectToDestroy.GetComponent<ObjectEffects>().SeparateParticleSystem();
        Destroy(objectToDestroy);

        //if there are no more objects in play and the player still has more objects
        if (objects.Count <= 0 && numberOfObjects > 0)
        {
            //spawn a new ball
            StartCoroutine(SpawnNewStartingBall());
        }
        //if there are no objects in play and the player has no more objects
        else if (numberOfObjects <= 0 && objects.Count <= 0)
        {
            //bring up results (after a delay)
            StartCoroutine(EndLevel());
        }
    }

    /// <summary>
    /// manages everything that happens with wther or not lobball is turned on or off
    /// </summary>
    public void ToggleLobBall()
    {
        lobBallEnabled = !lobBallEnabled;

        if (lobBallEnabled)
        {
            //replace the starting object with a beanbag with the same powerup states
            GameObject newBeanbag = SpawnNewObject(beanbagPrefab, objectSpawnPos.transform.position, Vector3.zero, objects[0].GetComponent<ObjectEffects>().GoldBallEnabled,
                objects[0].GetComponent<ObjectEffects>().MarkedBallEnabled, objects[0].GetComponent<ObjectEffects>().TriBallEnabled);

            //make beanbag the starting object
            objects.Clear();
            objects.Add(newBeanbag);
            Destroy(startingObject);
            startingObject = newBeanbag;
        }
        else
        {
            //replace the starting object with a ball with the same powerup states
            GameObject newBall = SpawnNewObject(ballPrefab, objectSpawnPos.transform.position, Vector3.zero, objects[0].GetComponent<ObjectEffects>().GoldBallEnabled,
                objects[0].GetComponent<ObjectEffects>().MarkedBallEnabled, objects[0].GetComponent<ObjectEffects>().TriBallEnabled);

            //make beanbag the starting object
            objects.Clear();
            objects.Add(newBall);
            Destroy(startingObject);
            startingObject = newBall;
        }
    }

    public void SwitchCameraView()
    {
        objectCamera.SetActive(!objectCamera.activeInHierarchy);
    }
}
