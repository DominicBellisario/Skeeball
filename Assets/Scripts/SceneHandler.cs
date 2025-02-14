using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler Instance { get; private set; }

    private void Awake()
    {
        //create singleton instance
        if (Instance != null && Instance != this) { Destroy(this); }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneAdditively(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    /// <summary>
    /// load a playable level along with the level UI
    /// </summary>
    /// <param name="levelName"></param>
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
        SceneManager.LoadScene("LevelUI", LoadSceneMode.Additive);
    }

    /// <summary>
    /// load a playable level along with the level UI
    /// </summary>
    /// <param name="levelName"></param>
    public void LoadEndlessLevel(string levelName)
    {
        SceneManager.LoadScene("E" + levelName);
        SceneManager.LoadScene("LevelUIEndless", LoadSceneMode.Additive);
    }

    public bool UnloadScene(string sceneName)
    {
        return SceneManager.UnloadSceneAsync(sceneName).isDone;
    }
}
