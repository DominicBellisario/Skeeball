using JetBrains.Annotations;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class ManagerEndless : ManagerBase
{
    [SerializeField] int[] easyLevels;
    [SerializeField] int[] mediumLevels;
    [SerializeField] int[] hardLevels;
    [SerializeField] int totalPoints = 0;
    [SerializeField] float multiplier = 1f;
    int currentLevel = 0;
    List<int> playedLevels = new();

    public static ManagerEndless Instance { get; private set; }
    public int TotalPoints { get { return totalPoints; } set { totalPoints = value; } }
    public float Multiplier { get { return multiplier; } set { multiplier = value; } }

    protected override void Awake()
    {
        base.Awake();
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
    }

    public void RecieveEditorValues(GameObject _ballPrefab, GameObject _beanbagPrefab, Vector3 _objectCameraOffset, bool _switchCameraOnLaunch, float _timeBetweenObjects)
    {
        ballPrefab = _ballPrefab;
        beanbagPrefab = _beanbagPrefab;
        objectCameraOffset = _objectCameraOffset;
        switchCameraOnLaunch = _switchCameraOnLaunch;
        timeBetweenObjects = _timeBetweenObjects;
    }

    public override void RecieveValues(int _currentLevelNumber, GameObject _objectSpawnPos, GameObject _objectCamera, int _startingNumberOfObjects, int _minScore, int _secretScore, int _goldBallPow, int _markedBallPow, int _triBallPow, int _lobBallPow)
    {
        //add to power amounts
        goldBallPow += _goldBallPow;
        markedBallPow += _markedBallPow;
        triBallPow += _triBallPow;
        lobBallPow += _lobBallPow;
        base.RecieveValues(_currentLevelNumber, _objectSpawnPos, _objectCamera, _startingNumberOfObjects, _minScore, _secretScore, _goldBallPow, _markedBallPow, _triBallPow, _lobBallPow);
    }

    public override void ResetValues()
    {
        base.ResetValues();
        playedLevels.Clear();
        currentLevel = 0;
        totalPoints = 0;
        multiplier = 1;
    }

    public void BeginEndlessMode()
    {
        Instance.LoadNewLevel(easyLevels);
    }

    private void LoadNewLevel(int[] difficulty)
    {
        //make a list of all unplayed levels of the selected difficulty
        List<int> unplayedLevels = new();
        foreach (int levelNum in difficulty)
        {
            if (!playedLevels.Contains(levelNum))
            {
                unplayedLevels.Add(levelNum);
            }
        }
        //pick a random number from the unplayed levels
        int levelNumToLoad = unplayedLevels[Helper.Instance.RandomInt(0, unplayedLevels.Count)];
        //add it to played levels
        playedLevels.Add(levelNumToLoad);
        //set it as the current level
        currentLevel = levelNumToLoad;
        //load a random level from these levels
        SceneHandler.Instance.LoadLevel("EL" + levelNumToLoad.ToString());
    }
}
