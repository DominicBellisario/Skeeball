using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScreenLogic : MonoBehaviour
{
    public void LoadTest()
    {
        SceneManager.LoadScene("testing");
        LoadLevelUI();

    }

    void LoadLevelUI()
    {
        SceneManager.LoadScene("LevelUI", LoadSceneMode.Additive);
    }

}
