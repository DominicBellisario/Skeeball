using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] Vector2 screenSize;
    [SerializeField] int numberOfLevels;
    [SerializeField] GameObject ballPrefab;
    [SerializeField] GameObject beanbagPrefab;
    [SerializeField] GameObject imageCollectEffect;
    [SerializeField] GameObject textCollectEffect;
    [SerializeField] Vector3 objectCameraOffset;
    /// <summary>
    /// when true, camera automaticaly switches to ball cam when launched
    /// </summary>
    bool switchCameraOnLaunch;
    bool colorblind;
    bool enablePaths;
    /// <summary>
    /// the time between the last active objects and a new spawn
    /// </summary>
    [SerializeField] float timeBetweenObjects;
    /// <summary>
    /// the place in the level the objects spawn in
    /// </summary>
    GameObject objectSpawnPos;
    /// <summary>
    /// a list of all objects currently in the level
    /// </summary>
    [SerializeField] List<GameObject> objects;
    GameObject startingObject;
    /// <summary>
    /// all camera positions in the current level
    /// </summary>
    List<GameObject> cameraPositions;
    int currentCameraPosition;
    GameObject mainCamera;
    /// <summary>
    /// how fast the camera moves between camera positions
    /// </summary>
    [SerializeField] float cameraMoveSpeed; //serialise debug
    bool canSwitchCamera;

    /// <summary>
    /// the current level score
    /// </summary>
    [SerializeField] int score; //serialise debug
    /// <summary>
    /// the minimum score needed to beat the current level
    /// </summary>
    int minScore;
    /// <summary>
    /// the minimum score needed to get a secret.  0 for no secret
    /// </summary>
    int secretScore;

    /// <summary>
    /// the current number of extra balls the player has
    /// </summary>
    [SerializeField] int numberOfObjects; //serialise debug
    /// <summary>
    /// the ID of the level being played
    /// </summary>
    int currentLevelNumber;

    /// <summary>
    /// number of gold ball powwerups
    /// </summary>
    public int goldBallPow; //serialise debug
    /// <summary>
    /// number of marked ball powwerups
    /// </summary>
    public int markedBallPow; //serialise debug
    /// <summary>
    /// number of tri ball powwerups
    /// </summary>
    public int triBallPow; //serialise debug
    /// <summary>
    /// number of lob ball powwerups
    /// </summary>
    public int lobBallPow; //serialise debug
    /// <summary>
    /// wether or not the current ball is a lobball
    /// </summary>
    bool lobBallEnabled;

    /// <summary>
    /// wether or not the game is paused
    /// </summary>
    bool isPaused;

    bool gameJustStarted;

    //endless
    [SerializeField] int[] easyLevels;
    [SerializeField] int[] mediumLevels;
    [SerializeField] int[] hardLevels;
    [SerializeField] int[] secretLevels;
    int[] randomLevels;
    string currentDifficulty;
    int[] levelsInCurrentDifficulty;
    [SerializeField] bool nextLevelIsSecret; //serialise debug
    GameObject starHole;

    int starHoleChanceUpgradesBought = 0;
    [SerializeField] float starHoleUpgradeIncreaseAmt;

    /// <summary>
    /// the players total point count
    /// </summary>
    [SerializeField] int totalPoints = 0; //serialise debug
    [SerializeField] float multiplier = 1f; //serialise debug
    /// <summary>
    /// the amount the multiplier increases by when a multi hole is hit
    /// </summary>
    [SerializeField] float multiplierIncreaseAmt = 0.25f;
    /// <summary>
    /// the current number of coins
    /// </summary>
    [SerializeField] int coins = 0; //serialise debug
    [SerializeField] int maxPowerups;
    /// <summary>
    /// the chance for the star hole in a level that has a star hole to be visible
    /// </summary>
    [SerializeField] float starHoleChance; //serialise debug
    [SerializeField] float startingStarHoleChance; //serialise debug
    /// <summary>
    /// how much the star hole chance increases each round
    /// </summary>
    [SerializeField] float starHoleChanceIncreaseAmount; //serialise debug

    /// <summary>
    /// the number of levels the player completed in the current round
    /// </summary>
    int numberOfCompletedLevelsInRound = 0;
    /// <summary>
    /// the number of levels in each round
    /// </summary>
    int levelsInCurrentRound = 3;
    int currentRoundNumber = 1;
    /// <summary>
    /// a list of all previously played levels
    /// </summary>
    List<int> playedLevels = new();
    /// <summary>
    /// all of the holes in a level that can be selected as a multi hole
    /// </summary>
    GameObject[] multiHoles;
    /// <summary>
    /// all of the holes in a level that are active multi holes
    /// </summary>
    List<GameObject> activatedMultiHoles = new();
    /// <summary>
    /// the maximum active holes possible in a level
    /// </summary>
    int maxActiveHoles = 1;
    /// <summary>
    /// scored = in a launch, at least one ball went into a good hole 
    /// </summary>
    bool scored = true;
    /// <summary>
    /// wether or not the player is in endless mode or level select
    /// </summary>
    bool endless;

    public static Manager Instance { get; private set; }
    public int NumberOfLevels { get { return numberOfLevels; } }
    public int CurrentLevelNumber { get { return currentLevelNumber; } }
    public int NumberOfObjects { get { return numberOfObjects; } }
    public int Score { get { return score; } }
    public int MinScore { get { return minScore; } }
    public int SecretScore { get { return secretScore; } }
    public int CurrentCameraPos { get { return currentCameraPosition; } }
    public GameObject StartingObject { get { return startingObject; } }
    public bool SwitchCameraOnLaunch
    {
        get { return switchCameraOnLaunch; }
        set
        {
            Instance.switchCameraOnLaunch = value;
            PlayerPrefs.SetInt("autoBallCam", Instance.switchCameraOnLaunch ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    public bool Colorblind
    {
        get { return colorblind; }
        set
        {
            Instance.colorblind = value;
            PlayerPrefs.SetInt("colorblind", Instance.colorblind ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    public bool EnablePaths
    {
        get { return enablePaths; }
        set
        {
            Instance.enablePaths = value;
            PlayerPrefs.SetInt("enablePaths", Instance.enablePaths ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public int GoldBallPow { get { return goldBallPow; } set { goldBallPow = value; } }
    public int MarkedBallPow { get { return markedBallPow; } set { markedBallPow = value; } }
    public int TriBallPow { get { return triBallPow; } set { triBallPow = value; } }
    public int LobBallPow { get { return lobBallPow; } set { lobBallPow = value; } }
    public bool LobBallEnabled { get { return lobBallEnabled; } set { lobBallEnabled = value; } }
    //endless
    public bool Endless { get { return endless; } }
    public bool NextLevelIsSecret { get { return nextLevelIsSecret; } set { nextLevelIsSecret = value; } }
    public int TotalPoints { get { return totalPoints; } }
    public int Coins { get { return coins; } set { coins = value; } }
    public float Multiplier { get { return multiplier; } set { multiplier = value; } }
    public float MultiplierIncreaseAmt { get { return multiplierIncreaseAmt; } }
    public int NumberOfCompletedLevelsInRound { get { return numberOfCompletedLevelsInRound; } }
    public int LevelsInCurrentRound { get { return levelsInCurrentRound; } }
    public int CurrentRoundNumber { get { return currentRoundNumber; } }
    public bool Scored { get { return scored; } set { scored = value; } }
    public int MaxPowerups { get { return maxPowerups; } }
    public List<GameObject> ActivatedMultiHoles { get { return activatedMultiHoles; } }
    public int StarHoleChanceUpgradesBought { get { return starHoleChanceUpgradesBought; } set { starHoleChanceUpgradesBought = value; } }
    public bool IsPaused { get { return isPaused; } set { isPaused = value; } }
    public bool GameJustStarted { get { return gameJustStarted; } set { Instance.gameJustStarted = value; } }

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
        nextLevelIsSecret = false;
        //random levels are all levels in endless mode
        randomLevels = new int[easyLevels.Count() + mediumLevels.Count() + hardLevels.Count()];
        for (int i = 0; i < randomLevels.Count(); i++)
        {
            randomLevels[i] = i + 1;
        }
        objects = new List<GameObject>();
        starHoleChance = startingStarHoleChance;
        //Screen.SetResolution((int)screenSize.x, (int)screenSize.y, false);
        gameJustStarted = true; //set to true so that the title screen can do the fade in animation
    }

    protected void Start()
    {
        switchCameraOnLaunch = PlayerPrefs.GetInt("autoBallCam") == 1;
    }

    public void Update()
    {
        //ball cam pos follows the first ball in the list
        if (objects.Count >= 1 && currentCameraPosition == 1)
        {
            cameraPositions[1].transform.position = objects[0].transform.position + objectCameraOffset;
            mainCamera.transform.position = cameraPositions[1].transform.position;
            mainCamera.transform.rotation = cameraPositions[1].transform.rotation;
        }
    }

    /// <summary>
    /// when a new level is loaded, send its values to manager and start the level
    /// </summary>
    /// <param name="_currentLevelNumber"></param>
    /// <param name="_objectSpawnPos"></param>
    /// <param name="_objectCamera"></param>
    /// <param name="_startingNumberOfObjects"></param>
    /// <param name="_minScore"></param>
    /// <param name="_secretScore"></param>
    /// <param name="_goldBallPow"></param>
    /// <param name="_markedBallPow"></param>
    /// <param name="_triBallPow"></param>
    /// <param name="_lobBallPow"></param>
    /// <param name="_multiHoles"></param>
    public virtual void RecieveValues(int _currentLevelNumber, GameObject _objectSpawnPos, GameObject _mainCamera, List<GameObject> _cameraPositions, int _startingNumberOfObjects, int _minScore, int _secretScore, int _goldBallPow, int _markedBallPow, int _triBallPow, int _lobBallPow, GameObject[] _multiHoles, GameObject _starHole)
    {
        currentLevelNumber = _currentLevelNumber;
        objectSpawnPos = _objectSpawnPos;
        mainCamera = _mainCamera;
        cameraPositions = _cameraPositions;
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
            //get multiholes
            multiHoles = _multiHoles;
            //get star hole
            starHole = _starHole;
            //if the level has a star hole, see if it is visible to the player
            if (starHole != null && Helper.Instance.RandomInt(1, 100) <= starHoleChance * 100)
            {
                starHole.SetActive(true);
            }
        }


        //update UI with starting values
        LevelUILogic.Instance.UpdateScore();
        LevelUILogic.Instance.UpdateBalls();
        LevelUILogic.Instance.UpdateCoins();
        LevelUILogic.Instance.UpdateCameraText(0, cameraPositions.Count);


        //spawn the first ball
        StartCoroutine(SpawnNewStartingBall());
    }

    /// <summary>
    /// reset level-specific values and ball states
    /// </summary>
    public virtual void ResetValues()
    {
        objects.Clear();
        ResetScore();
        LobBallEnabled = false;
        StopAllCoroutines();
        scored = true;
        currentCameraPosition = 0;
        canSwitchCamera = true;
        nextLevelIsSecret = false;
    }

    /// <summary>
    /// reset endless mode
    /// </summary>
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
        starHoleChance = startingStarHoleChance;
    }

    /// <summary>
    /// starts endless mode with easy levels
    /// </summary>
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
        //increase the chance for a star hole up to a point
        if (Instance.currentRoundNumber < 6)
        {
            Instance.starHoleChance += Instance.starHoleChanceIncreaseAmount;
        }
        Instance.numberOfCompletedLevelsInRound = 0; //SET TO 0 WHEN NOT DEBUGGING

        //apply round-based upgrades
        Instance.starHoleChance += Instance.starHoleChanceUpgradesBought * Instance.starHoleUpgradeIncreaseAmt;

        Instance.GoToNextEndlessLevel();
    }

    /// <summary>
    /// load a random, unplayed level in the difficulty, or go to the next difficulty if all levels are played
    /// </summary>
    public void GoToNextEndlessLevel()
    {
        if (nextLevelIsSecret)
        {
            SceneHandler.Instance.LoadLevel("ELS" + secretLevels[Helper.Instance.RandomInt(0, secretLevels.Count() - 1)]);
            return;
        }
        //if the round is over, go to shop
        if (numberOfCompletedLevelsInRound == levelsInCurrentRound)
        {
            //disable all round-based upgrades that the player got
            starHoleChance -= starHoleChanceUpgradesBought * starHoleUpgradeIncreaseAmt;
            starHoleChanceUpgradesBought = 0;

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
            else if (currentDifficulty == "medium")
            {
                levelsInCurrentDifficulty = hardLevels;
                currentDifficulty = "hard";
                GoToNextEndlessLevel();
                return;
            }
            else if (currentDifficulty == "hard")
            {
                levelsInCurrentDifficulty = randomLevels;
                currentDifficulty = "random";
                GoToNextEndlessLevel();
                return;
            }
            else if (currentDifficulty == "random")
            {
                playedLevels.Clear();
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

    /// <summary>
    /// update the score and UI
    /// </summary>
    /// <param name="scoreChange"></param>
    public void UpdateScore(int scoreChange)
    {
        score += scoreChange;
    }

    /// <summary>
    /// reset the score when transitioning levels
    /// </summary>
    private void ResetScore()
    {
        score = 0;
    }

    /// <summary>
    /// update total score and UI
    /// </summary>
    /// <param name="scoreChange"></param>
    public void UpdateTotalScore(int scoreChange)
    {
        scoreChange = Mathf.RoundToInt(scoreChange * multiplier);
        totalPoints += scoreChange;
        if (totalPoints > PlayerPrefs.GetInt("highscore"))
        {
            PlayerPrefs.SetInt("highscore", totalPoints);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// update ball count and UI
    /// </summary>
    /// <param name="ballsChange"></param>
    public void UpdateObjects(int ballsChange)
    {
        numberOfObjects += ballsChange;
        LevelUILogic.Instance.UpdateBalls();
    }

    /// <summary>
    /// update multiplier and UI
    /// </summary>
    /// <param name="multiplierChange"></param>
    public void UpdateMultiplier(float multiplierChange)
    {
        multiplier += multiplierChange;
    }

    /// <summary>
    /// update coins and UI
    /// </summary>
    /// <param name="coinsChange"></param>
    public void UpdateCoins(int coinsChange)
    {
        coins += coinsChange;
    }

    public bool UpdatePowerup(ref int powerupReference, int powerupChange)
    {
        //not over max powerups yet
        if (powerupReference + powerupChange <= maxPowerups)
        {
            powerupReference += powerupChange;
            return false;
        }
        //exeded max powerups
        else
        {
            powerupReference = maxPowerups;
            return true;
        }
    }

    /// <summary>
    /// spawns a new ball
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="force"></param>
    public GameObject SpawnNewObject(GameObject objectPrefab, Vector3 spawnPos, Vector3 force, bool gold, bool marked, bool tri)
    {
        GameObject newObject = Instantiate(objectPrefab, spawnPos, Quaternion.identity);

        //set its position and velocity
        //newObject.transform.position = spawnPos;
        newObject.GetComponent<Rigidbody>().AddForce(force);

        //set the powerup states
        if (gold) { newObject.GetComponent<ObjectEffects>().ToggleGoldBall(false); }
        if (marked) { newObject.GetComponent<ObjectEffects>().ToggleMarkedBall(false); }
        if (tri) { newObject.GetComponent<ObjectEffects>().ToggleTriBall(false); }

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
        //switch to main camera view
        SwitchCameraView(0);
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

    /// <summary>
    /// bring up the results screen after a delay
    /// </summary>
    /// <returns></returns>
    private IEnumerator EndLevel()
    {
        yield return new WaitForSeconds(timeBetweenObjects + 0.9f);
        //update level UI
        LevelUILogic.Instance.UpdateScore();
        LevelUILogic.Instance.UpdateTotalScore();
        LevelUILogic.Instance.UpdateMultiplier();
        //disable level ui event handler
        LevelUILogic.Instance.EventHandler.SetActive(false);
        if (!endless)
        {
            //unlock the next level if they won
            if (score >= minScore && currentLevelNumber != numberOfLevels)
            {
                PlayerPrefs.SetInt("unlockLevel_" + (currentLevelNumber + 1), 1);
            }
            //unlock the secret if they got secret score
            if (score >= secretScore)
            {
                PlayerPrefs.SetInt("unlockSecret_" + currentLevelNumber, 1);
            }
            PlayerPrefs.Save();

            //bring up the results screen
            SceneHandler.Instance.LoadSceneAdditively("ResultsScreen");
        }
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
            LevelUILogic.Instance.UpdateMultiplier();
        }
        //if there are no more objects in play and the player still has more objects
        if (objects.Count <= 0 && numberOfObjects > 0)
        {
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
    /// manages everything that happens with wether or not lobball is turned on or off
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

            newBeanbag.GetComponent<ObjectEffects>().ToggleLobBallEffects(lobBallEnabled);
        }
        else
        {
            //replace the starting object with a ball with the same powerup states
            GameObject newBall = SpawnNewObject(ballPrefab, objectSpawnPos.transform.position, Vector3.zero, objects[0].GetComponent<ObjectEffects>().GoldBallEnabled,
                objects[0].GetComponent<ObjectEffects>().MarkedBallEnabled, objects[0].GetComponent<ObjectEffects>().TriBallEnabled);

            //make ball the starting object
            objects.Clear();
            objects.Add(newBall);
            Destroy(startingObject);
            startingObject = newBall;

            newBall.GetComponent<ObjectEffects>().ToggleLobBallEffects(lobBallEnabled);
        }
    }

    /// <summary>
    /// toggle between ball and main camera
    /// </summary>
    public void SwitchCameraView(int activeCameraPosition)
    {
        //stop any current camera lerp
        StopAllCoroutines();

        //go to next cam position if a cam is not given
        if (activeCameraPosition == -1)
        {
            currentCameraPosition++;
            if (currentCameraPosition >= cameraPositions.Count)
            {
                currentCameraPosition = 0;
            }
        }
        //otherwise, switch to the specified position
        else
        {
            currentCameraPosition = activeCameraPosition;
        }

        //lerp camera to the active positon
        StartCoroutine(LerpCamera(cameraPositions[currentCameraPosition].transform));
        //update the UI
        LevelUILogic.Instance.UpdateCameraText(currentCameraPosition, cameraPositions.Count);
    }

    IEnumerator LerpCamera(Transform targetTransform)
    {
        //continue moving until it reaches its target
        while (mainCamera.transform.position != targetTransform.position)
        {
            //cannot spam the camera move button
            canSwitchCamera = false;

            float step = cameraMoveSpeed * Time.deltaTime;
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetTransform.position, step);
            mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, targetTransform.rotation, step);
            if (Vector3.Distance(mainCamera.transform.position, targetTransform.position) <= 0.05)
            {
                mainCamera.transform.position = targetTransform.position;
            }
            yield return new WaitForEndOfFrame();
        }
        canSwitchCamera = true;
    }

    public bool CanToggleCamera()
    {
        if (canSwitchCamera && objects.Count != 0)
        {
            return true;
        }
        return false;
    }

    public void SpawnCollectEffect(Vector3 _worldStartPoint, Vector3 _screenEndPoint, GameObject _parent, int _spriteNum, bool _alreadyAtMax, string _functionToCall)
    {
        GameObject newCollectEffect = Instantiate(imageCollectEffect);
        newCollectEffect.transform.SetParent(_parent.transform);
        newCollectEffect.GetComponent<ImageCollectEffect>().SetValuesAndStart(LevelUILogic.Instance.Canvas, Camera.main, LevelUILogic.Instance.Canvas.worldCamera, _worldStartPoint, _screenEndPoint, _spriteNum, _alreadyAtMax, _functionToCall);
    }

    public void SpawnTextEffect(Vector3 _worldStartPoint, Vector3 _screenEndPoint, float _verticalOffset, GameObject _parent, Color _textColor, string _text, string _functionToCall)
    {
        GameObject newTextEffect = Instantiate(textCollectEffect);
        newTextEffect.transform.SetParent(_parent.transform);
        newTextEffect.GetComponent<TextCollectEffect>().SetValuesAndStart(LevelUILogic.Instance.Canvas, Camera.main, LevelUILogic.Instance.Canvas.worldCamera, _worldStartPoint, _screenEndPoint, _verticalOffset, _textColor, _text, _functionToCall);
    }
}
