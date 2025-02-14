using UnityEngine;

public class ManagerEndless : ManagerBase
{
    [SerializeField] int[] easyLevels;
    [SerializeField] int[] mediumLevels;
    [SerializeField] int[] hardLevels;
    [SerializeField] int totalPoints = 0;
    [SerializeField] float multiplier = 1f;
    int currentLevel = 0;
    int completedLevels = 0;

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
}
