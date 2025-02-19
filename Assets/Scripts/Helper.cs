using UnityEngine;

public class Helper : MonoBehaviour
{
    public static Helper Instance { get; private set; }

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

    /// <summary>
    /// returns a random number rounded to an int
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public int RandomInt(float min, float max)
    {
        Debug.Log(min + ", " + max);
        return Mathf.RoundToInt(Random.Range(min, max));
    }
}
