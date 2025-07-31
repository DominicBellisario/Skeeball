using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelUILogic : MonoBehaviour
{
    //turns off when the game is paused
    [SerializeField] GameObject eventHandler;
    [SerializeField] Canvas canvas;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI minScoreText;
    [SerializeField] RectTransform minColorSlider;
    [SerializeField] RectTransform secretColorSlider;
    [SerializeField] Material goldUIMaterial;
    [SerializeField] Color lightGreen;
    [SerializeField] float sliderLerpSpeed;
    [SerializeField] ParticleSystem goldUIParticles;

    [SerializeField] TextMeshProUGUI ballsText;
    [SerializeField] TextMeshProUGUI cameraText;

    [SerializeField] Sprite powerupButtonUnactive;
    [SerializeField] Sprite powerupButtonActive;
    [SerializeField] RectTransform goldButtonPosition;
    [SerializeField] RectTransform markedButtonPosition;
    [SerializeField] RectTransform triButtonPosition;
    [SerializeField] RectTransform lobButtonPosition;
    [SerializeField] float lerpTime;

    [SerializeField] GameObject powerupButton;
    [SerializeField] GameObject goldBallButton;
    [SerializeField] GameObject markedBallButton;
    [SerializeField] GameObject triBallButton;
    [SerializeField] GameObject lobBallButton;

    [SerializeField] GameObject LevelNumberObject;

    [SerializeField] GameObject totalScoreText;
    [SerializeField] GameObject multiplierText;
    [SerializeField] GameObject levelAndRoundText;
    [SerializeField] GameObject coinsText;

    bool expandedPowerupUI;

    float pointContainerHeight;

    public GameObject EventHandler { get { return eventHandler; } set { eventHandler = value; } }
    public static LevelUILogic Instance { get; private set; }
    public Vector3 ScoreTextPos { get { return RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, scoreText.GetComponent<RectTransform>().position); } }
    public Vector3 TotalScoreTextPos { get { return RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, totalScoreText.GetComponent<RectTransform>().position); } }
    public Vector3 MultiplierTextPos { get { return RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, multiplierText.GetComponent<RectTransform>().position); } }
    public Vector3 CoinsTextPos { get { return RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, coinsText.GetComponent<RectTransform>().position); } }
    public Vector3 BallsTextPos { get { return RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, ballsText.GetComponent<RectTransform>().position); } }
    public Vector3 GoldBallButtonPos { get { return RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, goldBallButton.GetComponent<RectTransform>().position); } }
    public Vector3 MarkedBallButtonPos { get { return RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, markedBallButton.GetComponent<RectTransform>().position); } }
    public Vector3 TriBallButtonPos { get { return RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, triBallButton.GetComponent<RectTransform>().position); } }
    public Vector3 LobBallButtonPos { get { return RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, lobBallButton.GetComponent<RectTransform>().position); } }
    public Vector3 PowerupButtonPos { get { return RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, powerupButton.GetComponent<RectTransform>().position); } }
    public bool ExpandedPowerupUI { get { return expandedPowerupUI; } }
    public Canvas Canvas { get { return canvas; } }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this); }
        else { Instance = this; }

        expandedPowerupUI = false;
        pointContainerHeight = scoreText.transform.parent.GetComponent<RectTransform>().sizeDelta.y;
    }

    private void Start()
    {
        UpdateGoldBall();
        UpdateMarkedBall();
        UpdateTriBall();
        UpdateLobBall();

        //update level number object and start fadeaway
        LevelNumberObject.GetComponentInChildren<TextMeshProUGUI>().text = "Level " + Manager.Instance.CurrentLevelNumber;
        StartCoroutine(FadeAway(LevelNumberObject, 1.5f, 1.5f));

        //activate and update endless info if in endless mode
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
    /// refresh the score UI
    /// </summary>
    public void UpdateScore()
    {
        //update the score text
        scoreText.text = Manager.Instance.Score.ToString();
        minScoreText.text = Manager.Instance.MinScore.ToString();

        //calculate where the sliders should move to
        float pointPercentage = (float)Manager.Instance.Score / (float)Manager.Instance.MinScore;
        float secretPointPercentage = (float)(Manager.Instance.Score - Manager.Instance.MinScore) / (float)(Manager.Instance.SecretScore - Manager.Instance.MinScore);
        //player is above the minimum score
        if (pointPercentage >= 1)
        {
            pointPercentage = 1;
            minColorSlider.gameObject.GetComponent<Image>().color = Color.green;
        }
        //player is below the minimum score
        else
        {
            minColorSlider.gameObject.GetComponent<Image>().color = lightGreen;
        }
        //player is above the secret score
        if (secretPointPercentage >= 1)
        {
            secretPointPercentage = 1;
            secretColorSlider.gameObject.GetComponent<Image>().material = goldUIMaterial;
            goldUIParticles.Play();
        }
        //player is below the secret score
        else
        {
            secretColorSlider.gameObject.GetComponent<Image>().material = null;
            goldUIParticles.Clear();
            goldUIParticles.Stop();
        }

        //move the color sliders to its new position
        StartCoroutine(LerpScoreSlider(minColorSlider, (pointContainerHeight * pointPercentage) - pointContainerHeight, sliderLerpSpeed));
        //minColorSlider.anchoredPosition = new Vector2(minColorSlider.anchoredPosition.x, (pointContainerHeight * pointPercentage) - pointContainerHeight);
        StartCoroutine(LerpScoreSlider(secretColorSlider, (pointContainerHeight * secretPointPercentage) - pointContainerHeight, sliderLerpSpeed));
    }

    //lerp a slider to a target position with an ease out effect
    private IEnumerator LerpScoreSlider(RectTransform slider, float targetY, float duration)
    {
        //initilize variables
        Vector2 startPos = slider.GetComponent<RectTransform>().anchoredPosition;
        Vector2 endPos = new(startPos.x, targetY);
        float elapsedTime = 0f;
        //lerp the slider
        while (elapsedTime < 1f)
        {
            slider.anchoredPosition = Vector2.Lerp(startPos, endPos, 1 - Mathf.Pow(1 - elapsedTime, 3));
            elapsedTime += Time.deltaTime / duration;
            yield return null;
        }
        slider.anchoredPosition = endPos;
    }

    /// <summary>
    /// refresh the ball UI with new value
    /// </summary>
    public void UpdateBalls()
    {
        ballsText.text = Manager.Instance.NumberOfObjects.ToString();

        Image ballsTextImage = ballsText.transform.parent.GetComponent<Image>();
        // the background changes color depending on the number of balls remaining
        if (Manager.Instance.NumberOfObjects > 2) { ballsTextImage.color = Color.green; }
        else if (Manager.Instance.NumberOfObjects == 2) { ballsTextImage.color = Color.yellow; }
        else { ballsTextImage.color = Color.red; }
    }

    public void UpdateGoldBall()
    {
        goldBallButton.GetComponentInChildren<TextMeshProUGUI>().text = Manager.Instance.GoldBallPow.ToString();
        //goldBallButton.SetActive(expandedPowerupUI);
    }
    public void UpdateMarkedBall()
    {
        markedBallButton.GetComponentInChildren<TextMeshProUGUI>().text = Manager.Instance.MarkedBallPow.ToString();
        //markedBallButton.SetActive(expandedPowerupUI);
    }
    public void UpdateTriBall()
    {
        triBallButton.GetComponentInChildren<TextMeshProUGUI>().text = Manager.Instance.TriBallPow.ToString();
        //triBallButton.SetActive(expandedPowerupUI);
    }
    public void UpdateLobBall()
    {
        lobBallButton.GetComponentInChildren<TextMeshProUGUI>().text = Manager.Instance.LobBallPow.ToString();
        //lobBallButton.SetActive(expandedPowerupUI);
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

        //update the powerup button icon and play corrispnding sound
        if (expandedPowerupUI)
        {
            powerupButton.GetComponent<Image>().sprite = powerupButtonActive;
            //move the buttons to their new position
            StartCoroutine(EaseInToTarget(goldBallButton.GetComponent<RectTransform>(),
            powerupButton.GetComponent<RectTransform>().anchoredPosition, goldButtonPosition.GetComponent<RectTransform>().anchoredPosition));
            StartCoroutine(EaseInToTarget(markedBallButton.GetComponent<RectTransform>(),
            powerupButton.GetComponent<RectTransform>().anchoredPosition, markedButtonPosition.GetComponent<RectTransform>().anchoredPosition));
            StartCoroutine(EaseInToTarget(triBallButton.GetComponent<RectTransform>(),
            powerupButton.GetComponent<RectTransform>().anchoredPosition, triButtonPosition.GetComponent<RectTransform>().anchoredPosition));
            StartCoroutine(EaseInToTarget(lobBallButton.GetComponent<RectTransform>(),
            powerupButton.GetComponent<RectTransform>().anchoredPosition, lobButtonPosition.GetComponent<RectTransform>().anchoredPosition));
            SoundManager.Instance.PlaySound(4, 19);
        }
        else
        {
            powerupButton.GetComponent<Image>().sprite = powerupButtonUnactive;
            //move the buttons back to the powerup button position
            StartCoroutine(EaseInToTarget(goldBallButton.GetComponent<RectTransform>(),
            goldButtonPosition.GetComponent<RectTransform>().anchoredPosition, powerupButton.GetComponent<RectTransform>().anchoredPosition));
            StartCoroutine(EaseInToTarget(markedBallButton.GetComponent<RectTransform>(),
            markedButtonPosition.GetComponent<RectTransform>().anchoredPosition, powerupButton.GetComponent<RectTransform>().anchoredPosition));
            StartCoroutine(EaseInToTarget(triBallButton.GetComponent<RectTransform>(),
            triButtonPosition.GetComponent<RectTransform>().anchoredPosition, powerupButton.GetComponent<RectTransform>().anchoredPosition));
            StartCoroutine(EaseInToTarget(lobBallButton.GetComponent<RectTransform>(),
            lobButtonPosition.GetComponent<RectTransform>().anchoredPosition, powerupButton.GetComponent<RectTransform>().anchoredPosition));
            SoundManager.Instance.PlaySound(4, 20);
        }
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

        //play pause sound
        SoundManager.Instance.PlaySound(4, 21);

        //pause the game
        Time.timeScale = 0.0f;
    }

    public void ToggleCamera()
    {
        if (Manager.Instance.CanToggleCamera())
        {
            Manager.Instance.SwitchCameraView(-1);
            SoundManager.Instance.PlaySound(4, 32);
        }
    }

    public void UpdateCameraText(int activeCameraNum, int numOfCamerasInLevel)
    {
        cameraText.text = (activeCameraNum + 1).ToString() + "/" + numOfCamerasInLevel;
    }

    public IEnumerator EnableEventHandler()
    {
        yield return new WaitForSeconds(0.1f);
        EventHandler.SetActive(true);
    }

    private IEnumerator EaseInToTarget(RectTransform targetObject, Vector2 startPos, Vector2 targetPos)
    {
        float t = 0;
        //lerp to target
        while (t < lerpTime)
        {
            t += Time.deltaTime;
            targetObject.anchoredPosition = new Vector2(Mathf.SmoothStep(startPos.x, targetPos.x, t / lerpTime), Mathf.SmoothStep(startPos.y, targetPos.y, t / lerpTime));
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator FadeAway(GameObject objectToFade, float timeBeforeFade, float fadeTime)
    {
        //wait for a bit before starting the fade
        yield return new WaitForSeconds(timeBeforeFade);
        float t = 0;
        Color startObjectColor = objectToFade.GetComponent<Image>().color;
        Color startTextColor = objectToFade.GetComponentInChildren<TextMeshProUGUI>().color;
        //fade the object away
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            Color newTextColor = new Color(startTextColor.r, startTextColor.g, startTextColor.b, Mathf.SmoothStep(1, 0, t / fadeTime));
            Color newObjectColor = new Color(startObjectColor.r, startObjectColor.g, startObjectColor.b, Mathf.SmoothStep(1, 0, t / fadeTime));
            objectToFade.GetComponentInChildren<TextMeshProUGUI>().color = newTextColor;
            objectToFade.GetComponent<Image>().color = newObjectColor;
            yield return new WaitForEndOfFrame();
        }
        objectToFade.SetActive(false);
    }
}
