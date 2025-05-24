using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioClip[] sounds;

    public static SoundManager Instance { get; private set; }

    // plays a sound at the camera's position
    public void PlaySound(int index)
    {
        if (index < 0 || index >= sounds.Length)
        {
            Debug.LogWarning("Sound index out of range: " + index);
            return;
        }
        AudioSource.PlayClipAtPoint(sounds[index], Camera.main.transform.position);
    }
}
