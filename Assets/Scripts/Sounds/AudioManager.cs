using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

[System.Serializable]
public struct SoundEntry
{
    public SFXType type;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [Header("Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource environmentSource;

    [Header("Mixer")]
    [SerializeField] private AudioMixer mixer;

    [Header("Clips")]
    [SerializeField] private List<SoundEntry> sfxClips;
    [SerializeField] private AudioClip musicTrack;
    [Header("Volume UI Controller")]
    [SerializeField, Range(0f, 1f)] private float defaultMusicVol = 1f;
    [SerializeField, Range(0f, 1f)] private float defaultSfxVol = 1f;
    const string K_Music = "vol_music";
    const string K_Sfx = "vol_sfx";
    const string K_MusicLast = "vol_music_last";
    const string K_SfxLast = "vol_sfx_last";
    public float GetMusicVolume01() => musicSource ? musicSource.volume : 1f;
    public float GetSfxVolume01()   => sfxSource   ? sfxSource.volume   : 1f;

    private Dictionary<SFXType, AudioClip> sfxDict;
    StompActivator stompActivator;
    public float waitForBossToPlay = 2f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        //DontDestroyOnLoad(gameObject);

        // build dict...
        sfxDict = new Dictionary<SFXType, AudioClip>();
        foreach (var e in sfxClips) sfxDict[e.type] = e.clip;

        // apply saved volumes
        ApplySavedVolumes();
    }

    //when a new scene loads
    private void Start()
    {
        Debug.Log("SONG SHOULD START");
        // FIND STOMP ACTIVATOR, BUT ITS OK IF YOU DON'T
        StompActivator stompScript = FindObjectOfType<StompActivator>();
        if (stompScript != null)
        {
            stompActivator = stompScript;
        }
        StartCoroutine(PlayMusic());
    }
    //when a scene ends
    private void OnDestroy()
    {
        //stop song from playing
        musicSource.Stop();
    }

    public void PlaySFX(SFXType type, float volumeScale = 1f)
    {
        if (sfxDict.TryGetValue(type, out var clip))
        {
            sfxSource.PlayOneShot(clip, volumeScale);
            //            Debug.Log($"SOUND PLAYED: {type}");
        }
        else
        {
            Debug.LogWarning("SFX not found: " + type);
        }
    }

    public IEnumerator PlayMusic()
    {
        yield return new WaitForEndOfFrame(); // Ensure the game is fully initialized before playing music
        if (musicTrack != null)
        {
            if (stompActivator != null)
            {
                if (stompActivator.bossLevel)
                {
                    yield return new WaitForSeconds(waitForBossToPlay);
                    musicSource.clip = musicTrack;
                    musicSource.loop = true;
                    musicSource.Play();
                }
                else
                {
                    musicSource.clip = musicTrack;
                    musicSource.loop = true;
                    musicSource.Play();
                }
            }
            else
            {
                musicSource.clip = musicTrack;
                musicSource.loop = true;
                musicSource.Play();
            }


        }
    }


    public void SetVolume(string exposedParameter, float volume)
    {
        mixer.SetFloat(exposedParameter, Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20);
    }

    public void ApplySavedVolumes()
    {
        float mv = PlayerPrefs.GetFloat(K_Music, defaultMusicVol);
        float sv = PlayerPrefs.GetFloat(K_Sfx, defaultSfxVol);
        SetMusicVolume01(mv);
        SetSfxVolume01(sv);
    }

    public void SetMusicVolume01(float v)
    {
        v = Mathf.Clamp01(v);
        if (musicSource) musicSource.volume = v;
        PlayerPrefs.SetFloat(K_Music, v);
    }

    public void SetSfxVolume01(float v)
    {
        v = Mathf.Clamp01(v);
        if (sfxSource) sfxSource.volume = v;
        PlayerPrefs.SetFloat(K_Sfx, v);
    }

    public void MuteMusic(bool unmute)
    {
        if (unmute)
        {
            float last = Mathf.Max(0.0001f, PlayerPrefs.GetFloat(K_MusicLast, defaultMusicVol));
            SetMusicVolume01(last);
        }
        else
        {
            PlayerPrefs.SetFloat(K_MusicLast, PlayerPrefs.GetFloat(K_Music, defaultMusicVol));
            SetMusicVolume01(0f);
        }
    }

    public void MuteSfx(bool unmute)
    {
        if (unmute)
        {
            float last = Mathf.Max(0.0001f, PlayerPrefs.GetFloat(K_SfxLast, defaultSfxVol));
            SetSfxVolume01(last);
        }
        else
        {
            PlayerPrefs.SetFloat(K_SfxLast, PlayerPrefs.GetFloat(K_Sfx, defaultSfxVol));
            SetSfxVolume01(0f);
        }
    }


}

