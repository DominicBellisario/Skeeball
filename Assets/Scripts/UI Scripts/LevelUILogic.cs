using System.Collections;
using TMPro;
using UnityEngine;

public class LevelUILogic : MonoBehaviour
{
    //turns off when the game is paused
    [SerializeField] GameObject eventHandler;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI ballsText;

    [SerializeField] GameObject powerupButton;
    [SerializeField] GameObject goldBallButton;
    [SerializeField] GameObject markedBallButton;
    [SerializeField] GameObject triBallButton;
    [SerializeField] GameObject lobBallButton;

    [SerializeField] GameObject totalScoreText;
    [SerializeField] GameObject multiplierText;
    [SerializeField] GameObject levelAndRoundText;
    [SerializeField] GameObject coinsText;

    bool expandedPowerupUI;

    public GameObject EventHandler { get { return eventHandler; } set { eventHandler = value; } }
    public static LevelUILogic Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this); }
        else { Instance = this; }
    }

    private void Start()
    {
        expandedPowerupUI = false;
        //activate and update endless info if in endless mode
        UpdatePowerups();
        if (Manager.Instance.Endless) { SetUpEndlessUI(); }
    }

    public void SetUpEndlessUI()
    {
        totalScoreText.SetActive(true);
        multiplierText.SetActive(true);
        levelAndRoundText.SetActive(true);
        coinsText.SetActive(true);
        UpdateTotalScore(Manager.Instance.TotalPoints);
        UpdateMultiplier(Manager.Instance.Multiplier);
        UpdateLevelAndRoundText(Manager.Instance.NumberOfCompletedLevelsInRound, Manager.Instance.LevelsInCurrentRound, Manager.Instance.CurrentRoundNumber);
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

    public void UpdateTotalScore(int totalScore)
    {
        totalScoreText.GetComponentInChildren<TextMeshProUGUI>().text = "Total Score: " + totalScore;
    }

    public void UpdateMultiplier(float multiplier)
    {
        multiplierText.GetComponentInChildren<TextMeshProUGUI>().text = multiplier + "x multiplier";
    }

    public void UpdateLevelAndRoundText(int completedLevels, int totalLevels, int currentRound)
    {
        levelAndRoundText.GetComponentInChildren<TextMeshProUGUI>().text = "L: " + completedLevels + "/" + totalLevels + "  R: " + currentRound;
    }
    public void UpdateCoins(float multiplier)
    {
        multiplierText.GetComponentInChildren<TextMeshProUGUI>().text = "Multiplier: " + multiplier;
    }

    public void UpdateCoins(int coins)
    {
        coinsText.GetComponentInChildren<TextMeshProUGUI>().text = "Coins: " + coins;
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
        SceneHandler.Instance.LoadSceneAdditively("PauseScreen");

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
