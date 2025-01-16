using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LevelUILogic : MonoBehaviour
{
    //turns off when the game is paused
    [SerializeField]
    GameObject eventHandler;
    
    [SerializeField]
    TextMeshProUGUI scoreText;
    [SerializeField]
    TextMeshProUGUI ballsText;

    public GameObject EventHandler { get { return eventHandler; } set { eventHandler = value; } }
    public static LevelUILogic Instance { get; private set; }

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

    /// <summary>
    /// loads the pause scene and pause the game if the pause button is clicked
    /// </summary>
    public void LoadPauseScreen()
    {
        SceneManager.LoadScene("PauseScreen", LoadSceneMode.Additive);

        //disable the level ui event handler
        eventHandler.SetActive(false);

        //pause the game
        Time.timeScale = 0.0f;
    }
}
