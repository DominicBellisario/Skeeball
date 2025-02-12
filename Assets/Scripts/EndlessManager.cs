using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndlessManager : MonoBehaviour
{
    [SerializeField] int[] easyLevels;
    [SerializeField] int[] mediumLevels;
    [SerializeField] int[] hardLevels;

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
        GenerateNewLevel(RandomNumber(0, easyLevels.Length));
    }

    private void GenerateNewLevel(int levelNumber)
    {
        SceneManager.LoadScene(levelNumber);
    }

    /// <summary>
    /// returns a random number rounded to an int
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    private int RandomNumber(float min, float max)
    {
        return Mathf.RoundToInt(Random.Range(min, max));
    }
}
