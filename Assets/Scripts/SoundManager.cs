using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioClip[] sounds;

    public static SoundManager Instance { get; private set; }


    void Awake()
    {
        //create singleton instance
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
    }

    // plays a sound at the camera's position
    public void PlaySound(int index, float volume = 1.0f, float pitch = 1.0f)
    {
        if (index < 0 || index >= sounds.Length)
        {
            Debug.LogWarning("Sound index out of range: " + index);
            return;
        }
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = sounds[index];
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.Play();
    }
}
