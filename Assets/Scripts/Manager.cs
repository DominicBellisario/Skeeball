using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Unity.VisualScripting;
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
    [SerializeField] int score; //serialise debug
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
    string currentDifficulty;
    int[] levelsInCurrentDifficulty;
    [SerializeField] int totalPoints = 0; //serialise debug
    [SerializeField] float multiplier = 1f; //serialise debug
    [SerializeField] float multiplierIncreaseAmt = 0.25f;
    [SerializeField] int coins = 0;
    int numberOfCompletedLevelsInRound = 0;
    int levelsInCurrentRound = 3;
    int currentRoundNumber = 1;
    List<int> playedLevels = new();
    GameObject[] multiHoles;
    List<GameObject> activatedMultiHoles = new();
    int maxActiveHoles = 1;
    bool scored = true;


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
    public int Coins { get { return coins; } set { coins = value; } }
    public float Multiplier { get { return multiplier; } }
    public float MultiplierIncreaseAmt { get { return multiplierIncreaseAmt; } }
    public int NumberOfCompletedLevelsInRound { get { return numberOfCompletedLevelsInRound; } }
    public int LevelsInCurrentRound { get { return levelsInCurrentRound; } }
    public int CurrentRoundNumber { get { return currentRoundNumber; } }
    public bool Scored { get { return scored; } set { scored = value; } }
    public List<GameObject> ActivatedMultiHoles { get { return activatedMultiHoles; } }

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

    public virtual void RecieveValues(int _currentLevelNumber, GameObject _objectSpawnPos, GameObject _objectCamera, int _startingNumberOfObjects, int _minScore, int _secretScore, int _goldBallPow, int _markedBallPow, int _triBallPow, int _lobBallPow, GameObject[] _multiHoles)
    {
        currentLevelNumber = _currentLevelNumber;
        objectSpawnPos = _objectSpawnPos;
        objectCamera = _objectCamera;
        numberOfObjects = _startingNumberOfObjects;
        minScore = _minScore;
        secretScore = _secretScore;
        //if normal mode
        if (!endless)
        {
            //reset powerups
            goldBallPow = _goldBallPow;
            markedBallPow = _markedBallPow;
            triBallPow = _triBallPow;
            lobBallPow = _lobBallPow;
            //no multiholes
            multiHoles = new GameObject[0];
        }
        else
        {
            //add to powerups
            goldBallPow += _goldBallPow;
            markedBallPow += _markedBallPow;
            triBallPow += _triBallPow;
            lobBallPow += _lobBallPow;
            multiHoles = _multiHoles;
        }


        //update UI with starting values
        UpdateScore(0);
        UpdateObjects(0);
        UpdateCoins(0);


        //spawn the first ball
        StartCoroutine(SpawnNewStartingBall());
    }

    //reset level-specific values and ball states
    public virtual void ResetValues()
    {
        objects.Clear();
        ResetScore();
        LobBallEnabled = false;
        StopAllCoroutines();
        scored = true;
    }

    //reset endless values
    public virtual void EndlessReset()
    {
        endless = false;
        playedLevels.Clear();
        currentLevelNumber = 0;
        totalPoints = 0;
        multiplier = 1;
        coins = 0;
        numberOfCompletedLevelsInRound = 0;
        currentRoundNumber = 1;
        goldBallPow = 0;
        markedBallPow = 0;
        triBallPow = 0;
        lobBallPow = 0;
    }

    public void BeginEndlessMode()
    {
        Instance.endless = true;
        Instance.levelsInCurrentDifficulty = easyLevels;
        Instance.currentDifficulty = "easy";
        Instance.numberOfCompletedLevelsInRound = 0; //SET TO 0 WHEN NOT DEBUGGING
        Instance.GoToNextEndlessLevel();
    }

    public void NextRound()
    {
        Instance.currentRoundNumber++;
        Instance.numberOfCompletedLevelsInRound = 0; //SET TO 0 WHEN NOT DEBUGGING
        Instance.GoToNextEndlessLevel();
    }

    public void GoToNextEndlessLevel()
    {
        //if the round is over, go to shop
        if (numberOfCompletedLevelsInRound == levelsInCurrentRound)
        {
            SceneHandler.Instance.LoadScene("Shop");
            return;
        }
        
        //make a list of all unplayed levels of the selected difficulty
        List<int> unplayedLevels = new();
        foreach (int levelNum in levelsInCurrentDifficulty)
        {
            if (!playedLevels.Contains(levelNum))
            {
                unplayedLevels.Add(levelNum);
            }
        }
        //go to next difficulty if all levels are played in the current difficulty
        if (unplayedLevels.Count <= 0)
        {
            if (currentDifficulty == "easy")
            {
                levelsInCurrentDifficulty = mediumLevels;
                currentDifficulty = "medium";
                GoToNextEndlessLevel();
                return;
            }
        }
        numberOfCompletedLevelsInRound++;
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

    private void ResetScore()
    {
        score = 0;
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

    public void UpdateMultiplier(float multiplierChange)
    {
        multiplier += multiplierChange;
        LevelUILogic.Instance.UpdateMultiplier(multiplier);
    }

    public void UpdateCoins(int coinsChange)
    {
        coins += coinsChange;
        LevelUILogic.Instance.UpdateCoins(coins);
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
        //if in endless mode
        if (endless)
        {
            scored = false;
            //assign new multiplier holes
            activatedMultiHoles.Clear();
            for (int i = 0; i < maxActiveHoles; i++)
            {
                GameObject selectedHole = multiHoles[Helper.Instance.RandomInt(0, multiHoles.Count() - 1)];
                if (!activatedMultiHoles.Contains(selectedHole))
                {
                    activatedMultiHoles.Add(selectedHole);
                }
            }
            foreach (GameObject hole in multiHoles)
            {
                if (activatedMultiHoles.Contains(hole))
                {
                    hole.GetComponent<HoleVariables>().MakeMultiHole();
                }
                else if (hole.GetComponent<HoleVariables>().Marked)
                {
                    hole.GetComponent<HoleVariables>().MakeMarkedHole(false);
                }
                else
                {
                    hole.GetComponent<HoleVariables>().MakeNormalHole();
                }
            }
        }
    }

    private IEnumerator EndLevel()
    {
        yield return new WaitForSeconds(timeBetweenObjects);
        //disable level ui event handler
        LevelUILogic.Instance.EventHandler.SetActive(false);
        //bring up the results screen
        if (!endless) { SceneHandler.Instance.LoadSceneAdditively("ResultsScreen"); }
        else { SceneHandler.Instance.LoadSceneAdditively("ResultsScreenEndless"); }

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

        //reset multiplier if player did not score last ball
        if (objects.Count <= 0 && !scored)
        {
            //reset multiplier if player did not score last ball
            UpdateMultiplier(-multiplier + 1);
        }
        //if there are no more objects in play and the player still has more objects
        if (objects.Count <= 0 && numberOfObjects > 0)
        {
            //reset multiplier if player did not score last ball
            if (!scored)
            {
                UpdateMultiplier(-multiplier + 1);
            }
            //spawn a new ball (after a delay)
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
