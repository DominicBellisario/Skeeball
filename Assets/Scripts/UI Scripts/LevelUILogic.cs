using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UIElements;

public class LevelUILogic : MonoBehaviour
{
    //turns off when the game is paused
    [SerializeField]
    GameObject eventHandler;

    [SerializeField]
    TextMeshProUGUI scoreText;
    [SerializeField]
    TextMeshProUGUI ballsText;

    [SerializeField]
    GameObject powerupButton;
    [SerializeField]
    GameObject goldBallButton;
    [SerializeField]
    GameObject markedBallButton;
    [SerializeField]
    GameObject triBallButton;
    [SerializeField]
    GameObject lobBallButton;

    bool expandedPowerupUI;

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

    private void Start()
    {
        expandedPowerupUI = false;
        UpdatePowerups();
    }

    /// <summary>
    /// refresh the score UI with new value
    /// </summary>
    public void UpdateScore(int updatedScore)
    {
        scoreText.text = "Score: " + updatedScore + " / " + Manager.Instance.MinScore;
    }

    /// <summary>
    /// refresh the ball UI with new value
    /// </summary>
    public void UpdateBalls(int updatedBallCount)
    {
        ballsText.text = "Extra Balls: " + updatedBallCount;
    }

    /// <summary>
    /// refresh the powerup inventory UI to reflect current values
    /// </summary>
    public void UpdatePowerups()
    {
        Manager manager = Manager.Instance;
        goldBallButton.GetComponentInChildren<TextMeshProUGUI>().text = "Gold Ball: " + manager.GoldBallPow;
        goldBallButton.SetActive(expandedPowerupUI);
        markedBallButton.GetComponentInChildren<TextMeshProUGUI>().text = "Marked Ball: " + manager.MarkedBallPow;
        markedBallButton.SetActive(expandedPowerupUI);
        triBallButton.GetComponentInChildren<TextMeshProUGUI>().text = "Tri Ball: " + manager.TriBallPow;
        triBallButton.SetActive(expandedPowerupUI);
        lobBallButton.GetComponentInChildren<TextMeshProUGUI>().text = "Lob Ball: " + manager.LobBallPow;
        lobBallButton.SetActive(expandedPowerupUI);
    }

    /// <summary>
    /// show or hide the powerup ui when the powerups button is clicked
    /// </summary>
    public void ToggleExpandedPowerupUI()
    {
        expandedPowerupUI = !expandedPowerupUI;
        UpdatePowerups();
    }

    public void ToggleGoldBallPowerup()
    {
        Manager manager = Manager.Instance;
        if (manager.StartingObject != null)
        {
            if (manager.GoldBallPow > 0 && !manager.StartingObject.GetComponent<ObjectControls>().IsLaunched)
            {
                manager.StartingObject.GetComponent<ObjectEffects>().ToggleGoldBall();
            }
        }
    }

    public void ToggleMarkedBallPowerup()
    {
        Manager manager = Manager.Instance;
        if (manager.StartingObject != null)
        {
            if (manager.MarkedBallPow > 0 && !manager.StartingObject.GetComponent<ObjectControls>().IsLaunched)
            {
                manager.StartingObject.GetComponent<ObjectEffects>().ToggleMarkedBall();
            }
        }
    }

    public void ToggleTriBallPowerup()
    {
        Manager manager = Manager.Instance;
        if (manager.StartingObject != null)
        {
            if (manager.TriBallPow > 0 && !manager.StartingObject.GetComponent<ObjectControls>().IsLaunched)
            {
                manager.StartingObject.GetComponent<ObjectEffects>().ToggleTriBall();
            }
        }
    }

    public void ToggleLobBallPowerup()
    {
        Manager manager = Manager.Instance;
        if (manager.StartingObject != null)
        {
            if (manager.LobBallPow > 0 && !manager.StartingObject.GetComponent<ObjectControls>().IsLaunched)
            {
                manager.ToggleLobBall();
            }
        }
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

    public void ToggleCamera()
    {
        Manager.Instance.SwitchCameraView();
    }
}
