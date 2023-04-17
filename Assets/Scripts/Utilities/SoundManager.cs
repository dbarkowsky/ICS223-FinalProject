using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

// To use this SoundManager, drag/drop an instance of it in your scene.
// It persists throughout scenes and you can access it via a singleton reference.
// eg:
//   SoundManager.Instance.PlaySfx(audioClip);
//   SoundManager.Instance.PlayMusic(audioClip);
//   SoundManager.Instance.StopMusic();
//
//  Volume settings are controlled by a mixer which uses 2 sound groups (sfx & music)
//  Volume settings are automatically saved in PlayerPrefs OnDestroy() and restored in Awake()
//
// To copy and use this in another project, you will also need to copy the mixer that has
// been setup to work with this object. 
//

public class SoundManager : MonoBehaviour
{
    // STATIC VARS
    static public SoundManager Instance { get; private set; } = null;

    // MEMBER VARS
    [SerializeField] private AudioSource sfxSource;     // for playing sfx
    [SerializeField] private AudioSource musicSource;   // for playing music
    [SerializeField] private AudioMixer mixer;

    private float sfxVolume = 1.0f;     // for tracking sfx volume
    private float musicVolume = 0.5f;   // for tracking music volume

    const string PP_MUSIC_VOL = "MusicVol";
    const string PP_SFX_VOL = "SfxVol";

    // MEMBER PROPERTIES
    // a property to get/set sfx volume
    public float SfxVolume
    {
        get { return sfxVolume; }
        set
        {
            sfxVolume = Mathf.Clamp(value, 0.0f, 1.0f);
            mixer.SetFloat("SfxVolume", LinearToLog(sfxVolume));
        }
    }

    // a property to get/set music volume
    public float MusicVolume
    {
        get { return musicVolume; }
        set
        {
            musicVolume = Mathf.Clamp(value, 0.0f, 1.0f);
            mixer.SetFloat("MusicVolume", LinearToLog(musicVolume));
        }
    }

    private void Awake()
    {
        if (Instance == null)                    // if Awake() has never been called before
        {
            Instance = this;                    // remember this as our (one & only) SM
            DontDestroyOnLoad(this.gameObject); // don't destroy this gameObject when a new scene loads
            Init();                             // initialize the SM
        }
        else
        {                           // else we already have a SM that exists.
            Destroy(gameObject);    // destroy the SM that was about to be built
        }
    }

    private void OnDestroy()
    {
        // Save volume slider values [0..1] to PlayerPrefs
        PlayerPrefs.SetFloat(PP_MUSIC_VOL, musicVolume);
        PlayerPrefs.SetFloat(PP_SFX_VOL, sfxVolume);
    }

    private void Init()
    {
        // Restore volume slider values [0..1] from PlayerPrefs
        MusicVolume = PlayerPrefs.GetFloat(PP_MUSIC_VOL, musicVolume);   // if not found, use 1
        SfxVolume = PlayerPrefs.GetFloat(PP_SFX_VOL, sfxVolume);       // if not found, use 1
    }

    // Play a sfx clip (fire & forget)
    public void PlaySfx(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    // Play a music clip (capable of being stopped)
    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    // Stop the music!
    public void StopMusic()
    {
        musicSource.Stop();
    }

    // Stop with fade, then start new music
    public void StartSmoothly(AudioClip clip)
    {
        StartCoroutine(FadeThenPlay(1f, clip));
    }

    // convert from linear to logarithmic scale ([0...1] to decibels)
    private float LinearToLog(float value)
    {
        return Mathf.Log10(value) * 20;
    }

    private float LogToLinear(float value)
    {
        return Mathf.Pow(10, value / 20);
    }

    private IEnumerator FadeThenPlay(float duration, AudioClip clip)
    {
        float currentTime = 0;
        float currentVol;
        mixer.GetFloat("MusicVolume", out currentVol);
        currentVol = LogToLinear(currentVol);
        float targetValue = Mathf.Clamp(0.001f, 0.0001f, 1);
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
            mixer.SetFloat("MusicVolume", LinearToLog(newVol));
            yield return null;
        }
        mixer.SetFloat("MusicVolume", LinearToLog(musicVolume));
        PlayMusic(clip);
    }
}

