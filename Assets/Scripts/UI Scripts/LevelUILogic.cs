using System.Collections;
using TMPro;
using UnityEngine;

public class LevelUILogic : MonoBehaviour
{
    //turns off when the game is paused
    [SerializeField] GameObject eventHandler;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI ballsText;
    [SerializeField] TextMeshProUGUI cameraText;

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
    public Vector3 ScoreTextPos { get { return scoreText.GetComponent<RectTransform>().anchoredPosition; } }
    public Vector3 TotalScoreTextPos { get { return totalScoreText.GetComponent<RectTransform>().anchoredPosition; } }
    public Vector3 MultiplierTextPos { get { return multiplierText.GetComponent<RectTransform>().anchoredPosition; } }
    public Vector3 CoinsTextPos { get { return coinsText.GetComponent<RectTransform>().anchoredPosition; } }
    public Vector3 BallsTextPos { get { return ballsText.GetComponent<RectTransform>().anchoredPosition; } }
    public Vector3 GoldBallButtonPos { get { return goldBallButton.GetComponent<RectTransform>().anchoredPosition; } }
    public Vector3 MarkedBallButtonPos { get { return markedBallButton.GetComponent<RectTransform>().anchoredPosition; } }
    public Vector3 TriBallButtonPos { get { return triBallButton.GetComponent<RectTransform>().anchoredPosition; } }
    public Vector3 LobBallButtonPos { get { return lobBallButton.GetComponent<RectTransform>().anchoredPosition; } }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this); }
        else { Instance = this; }
    }

    private void Start()
    {
        expandedPowerupUI = false;
        //activate and update endless info if in endless mode
        UpdateGoldBall();
        UpdateMarkedBall();
        UpdateTriBall();
        UpdateLobBall();
        if (Manager.Instance.Endless) { SetUpEndlessUI(); }
    }

    public void SetUpEndlessUI()
    {
        totalScoreText.SetActive(true);
        multiplierText.SetActive(true);
        levelAndRoundText.SetActive(true);
        coinsText.SetActive(true);
        UpdateTotalScore();
        UpdateMultiplier();
        UpdateLevelAndRoundText();
    }

    /// <summary>
    /// refresh the score UI with new value
    /// </summary>
    public void UpdateScore()
    {
        scoreText.text = "Score: " + Manager.Instance.Score + " / " + Manager.Instance.MinScore;
    }

    /// <summary>
    /// refresh the ball UI with new value
    /// </summary>
    public void UpdateBalls()
    {
        ballsText.text = "Balls: " + Manager.Instance.NumberOfObjects;
    }

    public void UpdateGoldBall()
    {
        goldBallButton.GetComponentInChildren<TextMeshProUGUI>().text = "Gold Ball: " + Manager.Instance.GoldBallPow;
        goldBallButton.SetActive(expandedPowerupUI);
    }
    public void UpdateMarkedBall()
    {
        markedBallButton.GetComponentInChildren<TextMeshProUGUI>().text = "Marked Ball: " + Manager.Instance.MarkedBallPow;
        markedBallButton.SetActive(expandedPowerupUI);
    }
    public void UpdateTriBall()
    {
        triBallButton.GetComponentInChildren<TextMeshProUGUI>().text = "Tri Ball: " + Manager.Instance.TriBallPow;
        triBallButton.SetActive(expandedPowerupUI);
    }
    public void UpdateLobBall()
    {
        lobBallButton.GetComponentInChildren<TextMeshProUGUI>().text = "Lob Ball: " + Manager.Instance.LobBallPow;
        lobBallButton.SetActive(expandedPowerupUI);
    }

    public void UpdateTotalScore()
    {
        totalScoreText.GetComponentInChildren<TextMeshProUGUI>().text = "Total Score: " + Manager.Instance.TotalPoints;
    }

    public void UpdateMultiplier()
    {
        multiplierText.GetComponentInChildren<TextMeshProUGUI>().text = Manager.Instance.Multiplier + "x multiplier";
    }

    public void UpdateLevelAndRoundText()
    {
        levelAndRoundText.GetComponentInChildren<TextMeshProUGUI>().text = "L: " + Manager.Instance.NumberOfCompletedLevelsInRound + "/" + Manager.Instance.LevelsInCurrentRound + "  R: " + Manager.Instance.CurrentRoundNumber;
    }

    public void UpdateCoins()
    {
        coinsText.GetComponentInChildren<TextMeshProUGUI>().text = "Coins: " + Manager.Instance.Coins;
    }

    /// <summary>
    /// show or hide the powerup ui when the powerups button is clicked
    /// </summary>
    public void ToggleExpandedPowerupUI()
    {
        expandedPowerupUI = !expandedPowerupUI;
        UpdateGoldBall();
        UpdateMarkedBall();
        UpdateTriBall();
        UpdateLobBall();
    }

    public void ToggleGoldBallPowerup()
    {
        Manager manager = Manager.Instance;
        if (manager.StartingObject != null)
        {
            if (manager.GoldBallPow > 0 && !manager.StartingObject.GetComponent<ObjectControls>().IsLaunched)
            {
                manager.StartingObject.GetComponent<ObjectEffects>().ToggleGoldBall(true);
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
                manager.StartingObject.GetComponent<ObjectEffects>().ToggleMarkedBall(true);
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
                manager.StartingObject.GetComponent<ObjectEffects>().ToggleTriBall(true);
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
        if (Manager.Instance.CanToggleCamera())
        {
            Manager.Instance.SwitchCameraView(-1);
        }
    }

    public void UpdateCameraText(int activeCameraNum, int numOfCamerasInLevel)
    {
        cameraText.text = "Switch Camera:\n\n" + (activeCameraNum + 1) + "/" + numOfCamerasInLevel;
    }
}
