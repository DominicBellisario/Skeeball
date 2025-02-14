using UnityEngine;

public class Manager : ManagerBase
{
    public static Manager Instance { get; private set; }
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
        ManagerEndless.Instance.RecieveEditorValues(ballPrefab, beanbagPrefab, objectCameraOffset, switchCameraOnLaunch, timeBetweenObjects);
    }

    public override void RecieveValues(int _currentLevelNumber, GameObject _objectSpawnPos, GameObject _objectCamera, int _startingNumberOfObjects, int _minScore, int _secretScore, int _goldBallPow, int _markedBallPow, int _triBallPow, int _lobBallPow)
    {
        //reset power amounts
        goldBallPow = _goldBallPow;
        markedBallPow = _markedBallPow;
        triBallPow = _triBallPow;
        lobBallPow = _lobBallPow;
        base.RecieveValues(_currentLevelNumber, _objectSpawnPos, _objectCamera, _startingNumberOfObjects, _minScore, _secretScore, _goldBallPow, _markedBallPow, _triBallPow, _lobBallPow);
    }
}
