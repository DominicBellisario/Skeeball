using UnityEngine;

public class LevelManager : MonoBehaviour
{
    //the level number
    [SerializeField] int levelNumber;

    //the place in the level the objects spawn in
    [SerializeField] GameObject objectSpawnPos;

    //camera that follows the first object in the list
    [SerializeField] GameObject objectCamera;

    //the minimum score needed to beat a level
    [SerializeField] int minScore;
    //the minimum score needed to get a secret.  0 for no secret
    [SerializeField] int secretScore;

    //the starting number of objects
    [SerializeField] int numberOfObjects;

    //all of the holes that can be chosen to add to the multiplier (endless)
    [SerializeField] GameObject[] multiHoles;

    //all of the coin setups (endless)
    [SerializeField] GameObject[] coinPositions;

    //how many of each powerup the player has
    [SerializeField] int goldBallPow;
    [SerializeField] int markedBallPow;
    [SerializeField] int triBallPow;
    [SerializeField] int lobBallPow;

    private void Start()
    {
        //sends all starting level values to manager
        Manager.Instance.RecieveValues(levelNumber, objectSpawnPos, objectCamera, numberOfObjects, minScore, secretScore, goldBallPow, markedBallPow, triBallPow, lobBallPow, multiHoles);

        //activates a random coin position object if endless mode
        if (Manager.Instance.Endless) { coinPositions[Helper.Instance.RandomInt(0, coinPositions.Length - 1)].SetActive(true); }
    }
}
