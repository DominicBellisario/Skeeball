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
    [SerializeField]
    GameObject beanbagPrefab;

    //the place in the level the objects spawn in
    [SerializeField]
    GameObject objectSpawnPos;

    //a list of all objects currently in the level
    [SerializeField]
    List<GameObject> objects;

    GameObject startingObject;

    //camera that follows the first object in the list
    [SerializeField]
    GameObject objectCamera;
    Vector3 objectCameraOffset;

    //the current score
    int score;
    //the minimum score needed to beat a level
    [SerializeField]
    int minScore;
    //the minimum score needed to get a secret.  0 for no secret
    [SerializeField]
    int secretScore;

    //the current number of objects
    [SerializeField]
    int numberOfObjects;

    //the time between the last active objects and a new spawn
    float timeBetweenObjects;

    //how many of each powerup the player has
    [SerializeField]
    int goldBallPow;
    [SerializeField]
    int markedBallPow;
    [SerializeField]
    int triBallPow;
    [SerializeField]
    int lobBallPow;
    bool lobBallEnabled;

    public int LevelNumber { get { return levelNumber; } }
    public int Score { get { return score; } }
    public int MinScore { get { return minScore; } }
    public int SecretScore { get { return secretScore; } }
    public GameObject ObjectCamera { get { return objectCamera; } }
    public GameObject StartingObject { get { return startingObject; } }
    public int GoldBallPow { get { return goldBallPow; } set { goldBallPow = value; } }
    public int MarkedBallPow { get { return markedBallPow; } set { markedBallPow = value; } }
    public int TriBallPow { get { return triBallPow; } set { triBallPow = value; } }
    public int LobBallPow { get { return lobBallPow; } set { lobBallPow = value; } }
    public bool LobBallEnabled { get { return lobBallEnabled; } set { lobBallEnabled = value; } }

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
        timeBetweenObjects = 1;
        objectCameraOffset = new Vector3(0, 3, -5);

        //update the score and ball UI with starting values
        UpdateScore(0);
        UpdateObjects(0);

        //spawn the first ball
        StartCoroutine(SpawnNewStartingBall());
    }

    public void Update()
    {
        //ball camera follows the first ball in the list
        if (objects.Count >= 1)
        {
            objectCamera.transform.position = objects[0].transform.position + objectCameraOffset;
        }
    }

    public void UpdateScore(int scoreChange)
    {
        score += scoreChange;
        
        LevelUILogic.Instance.UpdateScore(score);
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
    IEnumerator SpawnNewStartingBall()
    {
        yield return new WaitForSeconds(timeBetweenObjects);
        SpawnNewObject(ballPrefab, objectSpawnPos.transform.position, Vector3.zero, false, false, false);
        UpdateObjects(-1);
        //switch to main camera view
        objectCamera.SetActive(false);
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
            //disable level ui event handler
            LevelUILogic.Instance.EventHandler.SetActive(false);

            //bring up the results screen
            SceneManager.LoadScene("ResultsScreen", LoadSceneMode.Additive);

            //pause the game
            Time.timeScale = 0;
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
