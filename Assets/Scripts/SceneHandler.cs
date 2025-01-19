using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    /// <summary>
    /// unload all scenes and load the home screen
    /// </summary>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// load a playable level along with the level UI
    /// </summary>
    /// <param name="levelName"></param>
    public void LoadLevelAndLevelUI(string levelName)
    {
        SceneManager.LoadScene(levelName);
        SceneManager.LoadScene("LevelUI", LoadSceneMode.Additive);
    }
}
