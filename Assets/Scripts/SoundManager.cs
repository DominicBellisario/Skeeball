using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioClip[] sounds;

    //0: general, 1: windup, 2: holes, 3: score, 4: UI, 5: music, 6: fan, 7: gold, 8: marked, 9: tri, 10: lob
    [SerializeField] AudioSource[] audioSources;

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

    void Start()
    {
        //set master volume from PlayerPrefs
        //SetMasterVolume(PlayerPrefs.GetFloat("masterVolume"));
    }

    public void SetMasterVolume(float volume)
    {
        PlayerPrefs.SetFloat("masterVolume", volume);
        PlayerPrefs.Save();

        volume /= 10f;
        AudioListener.volume = volume;
        UnityEngine.Debug.Log("yo");
    }

    // plays a sound at the camera's position
    public void PlaySound(int sourceIndex, int soundIndex, float volume = 1.0f, float pitch = 1.0f)
    {
        if (soundIndex < 0 || soundIndex >= sounds.Length)
        {
            UnityEngine.Debug.LogWarning("Sound index out of range: " + soundIndex);
            return;
        }
        audioSources[sourceIndex].clip = sounds[soundIndex];
        audioSources[sourceIndex].volume = volume;
        audioSources[sourceIndex].pitch = pitch;
        audioSources[sourceIndex].Play();
    }

    public void PlayUISound(int soundToPlay)
    {
        Instance.PlaySound(4, soundToPlay);
    }

    public bool SourceIsPlaying(int sourceIndex)
    {
        return audioSources[sourceIndex].isPlaying;
    }
}
