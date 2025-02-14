using UnityEngine;

public class EndlessManager : MonoBehaviour
{
    [SerializeField] int[] easyLevels;
    [SerializeField] int[] mediumLevels;
    [SerializeField] int[] hardLevels;

    [SerializeField] SceneHandler sceneHandler;
    [SerializeField] int totalPoints = 0;
    [SerializeField] float multiplier = 1f;

    int currentLevel = 0;
    int completedLevels = 0;

    EndlessManager instance;

    public EndlessManager Instance { get { return instance; } set { instance = value; } }
    public int TotalPoints { get { return totalPoints; } set { totalPoints = value; } }
    public float Multiplier { get { return multiplier; } set { multiplier = value; } }

    /// <summary>
    /// ran when the endless mode button is pressed
    /// </summary>
    public void StartEndlessMode()
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
        //set up first level
        SceneHandler.Instance.LoadScene("EL" + Helper.Instance.RandomInt(0, easyLevels.Length).ToString());
        SceneHandler.Instance.LoadSceneAdditively("LevelUIEndless");
    }
}
