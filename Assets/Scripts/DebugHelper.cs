using UnityEngine;

public class DebugHelper : MonoBehaviour
{
    public void ResetHighscore()
    {
        PlayerPrefs.SetInt("highscore", 0);
        PlayerPrefs.Save();
    }

    public void ResetUnlockedLevels()
    {
        for (int i = 1; i <= Manager.Instance.NumberOfLevels; i++)
        {
            PlayerPrefs.SetInt("unlockLevel_" + i, 0);
            PlayerPrefs.SetInt("unlockSecret_" + i, 0);
        }
        PlayerPrefs.SetInt("unlockLevel_1", 1);
        PlayerPrefs.SetInt("selectedSkin", 0);
        PlayerPrefs.Save();
    }

    public void UnlockAllLevels()
    {
        for (int i = 1; i <= Manager.Instance.NumberOfLevels; i++)
        {
            PlayerPrefs.SetInt("unlockLevel_" + i, 1);
        }
        PlayerPrefs.Save();
    }

    public void UnlockAllSecrets()
    {
        for (int i = 0; i <= Manager.Instance.NumberOfLevels; i++)
        {
            PlayerPrefs.SetInt("unlockSecret_" + i, 1);
        }
        PlayerPrefs.Save();
    }
}
