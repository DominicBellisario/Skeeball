using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreenLogic : MonoBehaviour
{

    //bring back the level ui and unpause
    public void ResumeGame()
    {
        Time.timeScale = 1;
        LevelUILogic.Instance.EventHandler.SetActive(true);
        SceneManager.UnloadSceneAsync("PauseScreen");
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(LevelManager.Instance.gameObject.scene.name);
        SceneManager.LoadScene("LevelUI", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("PauseScreen");
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("HomeScreen");
    }
}
