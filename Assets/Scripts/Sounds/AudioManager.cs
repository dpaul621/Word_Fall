using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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

    private Dictionary<SFXType, AudioClip> sfxDict;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        sfxDict = new Dictionary<SFXType, AudioClip>();
        foreach (var entry in sfxClips)
        {
            sfxDict[entry.type] = entry.clip;
        }

        PlayMusic();
    }

    public void PlaySFX(SFXType type, float volumeScale = 1f)
    {
        if (sfxDict.TryGetValue(type, out var clip))
        {
            sfxSource.PlayOneShot(clip, volumeScale);
        }
        else
        {
            Debug.LogWarning("SFX not found: " + type);
        }
    }

    public void PlayMusic()
    {
        if (musicTrack != null)
        {
            musicSource.clip = musicTrack;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void SetVolume(string exposedParameter, float volume)
    {
        mixer.SetFloat(exposedParameter, Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20);
    }
}

