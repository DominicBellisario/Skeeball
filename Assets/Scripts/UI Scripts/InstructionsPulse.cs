using UnityEngine;
using UnityEngine.UI;

public class InstructionsPulse : MonoBehaviour
{
    [SerializeField] Material pulseMaterial;

    void Awake()
    {
        //if the game has been loaded before, start pulsing the instructions material
        if (!PlayerPrefs.HasKey("firstLoad"))
        {
            GetComponent<Image>().material = pulseMaterial;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
