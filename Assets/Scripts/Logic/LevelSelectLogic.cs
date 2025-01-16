using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectLogic : MonoBehaviour
{
    public void LoadHomeScreen()
    {
        SceneManager.LoadScene("HomeScreen");
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("L1");
        LoadLevelUI();
    }

    void LoadLevelUI()
    {
        SceneManager.LoadScene("LevelUI", LoadSceneMode.Additive);
    }

}
