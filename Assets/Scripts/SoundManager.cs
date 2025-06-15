using UnityEngine;

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

    public void SetMasterVolume(float volume)
    {
        PlayerPrefs.SetFloat("masterVolume", volume);
        PlayerPrefs.Save();

        volume /= 10f;
        AudioListener.volume = volume;
        // Play a sound to test the volume change
        Instance.PlaySound(0, 3); 
    }

    // plays a sound at the camera's position
    public void PlaySound(int sourceIndex, int soundIndex, float volume = 1.0f, float pitch = 1.0f)
    {
        audioSources[sourceIndex].clip = sounds[soundIndex];
        audioSources[sourceIndex].volume = volume;
        audioSources[sourceIndex].pitch = pitch;
        audioSources[sourceIndex].Play();
        //Debug.Log("Playing sound: " + sounds[soundIndex].name + " from source: " + sourceIndex);
    }

    public void PlayUISound(int soundToPlay)
    {
        Instance.PlaySound(4, soundToPlay);
    }

    public bool SourceIsPlaying(int sourceIndex)
    {
        return audioSources[sourceIndex].isPlaying;
    }

    public void StopSound(int sourceIndex)
    {
        audioSources[sourceIndex].Pause();
    }
}
