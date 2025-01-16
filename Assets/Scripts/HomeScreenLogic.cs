using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScreenLogic : MonoBehaviour
{
    public void LoadTest()
    {
        SceneManager.LoadScene("Testing");
        LoadLevelUI();
    }

    public void LoadLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    void LoadLevelUI()
    {
        SceneManager.LoadScene("LevelUI", LoadSceneMode.Additive);
    }

}
