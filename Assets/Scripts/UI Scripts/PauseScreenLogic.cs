using System.Collections;
using UnityEngine;

public class PauseScreenLogic : MonoBehaviour
{
    IEnumerator EnableEventHandler()
    {
        yield return new WaitForEndOfFrame();
        LevelUILogic.Instance.EventHandler.SetActive(true);
    }
    //unpause, resume game without resetting
    public void ResumeGame()
    {
        StartCoroutine(EnableEventHandler());
        Time.timeScale = 1;
        SceneHandler.Instance.UnloadScene("PauseScreen");
    }

    //unpause, reset level
    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneHandler.Instance.LoadLevel("L" + Manager.Instance.CurrentLevelNumber.ToString());
        SceneHandler.Instance.UnloadScene("PauseScreen");
        Manager.Instance.ResetValues();
    }

    //unpause, load main menu
    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneHandler.Instance.LoadScene("HomeScreen");
        Manager.Instance.ResetValues();
    }
}
