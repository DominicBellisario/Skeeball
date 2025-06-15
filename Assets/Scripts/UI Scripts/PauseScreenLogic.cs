using UnityEngine;

public class PauseScreenLogic : MonoBehaviour
{
    [SerializeField] GameObject restartButton;

    private void Start()
    {
        Manager.Instance.IsPaused = true;
        //no restarting level for endless mode
        if (Manager.Instance.Endless)
        {
            restartButton.SetActive(false);
        }
    }

    //unpause, resume game without resetting
    public void ResumeGame()
    {
        //enable level UI event handler after this scene is unloaded
        LevelUILogic.Instance.StartCoroutine(LevelUILogic.Instance.EnableEventHandler());
        Time.timeScale = 1;
        Manager.Instance.IsPaused = false;
        //play unpause sound
        SoundManager.Instance.PlaySound(4, 22);
        SceneHandler.Instance.UnloadScene("PauseScreen");
    }

    //unpause, reset level
    public void RestartLevel()
    {
        Time.timeScale = 1;
        Manager.Instance.IsPaused = false;
        //play unpause sound
        SoundManager.Instance.PlaySound(4, 22);
        SceneHandler.Instance.LoadLevel("L" + Manager.Instance.CurrentLevelNumber.ToString());
        SceneHandler.Instance.UnloadScene("PauseScreen");
        Manager.Instance.ResetValues();
    }

    //unpause, load main menu
    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        Manager.Instance.IsPaused = false;
        //play main menu sound
        SoundManager.Instance.PlaySound(4, 23);
        SceneHandler.Instance.LoadScene("HomeScreen");
        Manager.Instance.ResetValues();
        Manager.Instance.EndlessReset();
    }
}
