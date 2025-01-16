using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class LevelUILogic : MonoBehaviour
{
    public static LevelUILogic Instance { get; private set; }

    [SerializeField]
    TextMeshProUGUI scoreText;
    [SerializeField]
    TextMeshProUGUI ballsText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    /// <summary>
    /// refresh the score UI with new value
    /// </summary>
    public void UpdateScore(int updatedScore)
    {
        scoreText.text = "Score: " + updatedScore;
    }

    /// <summary>
    /// refresh the ball UI with new value
    /// </summary>
    public void UpdateBalls(int updatedBallCount)
    { 
        ballsText.text = "Extra Balls: " + updatedBallCount;
    }
}
