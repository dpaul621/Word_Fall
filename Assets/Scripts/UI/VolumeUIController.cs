using UnityEngine;
using UnityEngine.UI;

public class VolumeUIController : MonoBehaviour
{
    [SerializeField] private Slider musicSlider; // 0..1
    [SerializeField] private Slider sfxSlider;   // 0..1
    [SerializeField] private Toggle musicToggle; // checked = unmuted
    [SerializeField] private Toggle sfxToggle;   // checked = unmuted

    void Start()
    {
        // initialize from saved values (AudioManager.ApplySavedVolumes already ran in Awake)
        float mv = PlayerPrefs.GetFloat("vol_music", 1f);
        float sv = PlayerPrefs.GetFloat("vol_sfx",   1f);

        musicSlider.value = mv;
        sfxSlider.value   = sv;

        musicToggle.isOn = mv > 0.0001f;
        sfxToggle.isOn   = sv > 0.0001f;

        UpdateInteractable();

        // push once so sources match the sliders for this session
        OnMusicSlider(mv);
        OnSfxSlider(sv);
    }

    public void OnMusicSlider(float v) {
        Debug.Log("Music slider " + v);
        AudioManager.Instance.SetMusicVolume01(v);
        if (v > 0f) musicToggle.isOn = true;
        UpdateInteractable();
    }

    public void OnSfxSlider(float v)
    {
        Debug.Log("SFX slider " + v);
        AudioManager.Instance.SetSfxVolume01(v);
        if (v > 0f) sfxToggle.isOn = true;
        UpdateInteractable();
    }

    // toggles act as mute (checked = unmuted)
    public void OnMusicToggle(bool on)
    {
        AudioManager.Instance.MuteMusic(on);
        musicSlider.value = on ? PlayerPrefs.GetFloat("vol_music", 1f) : 0f;
        UpdateInteractable();
    }

    public void OnSFXToggle(bool on)
    {
        AudioManager.Instance.MuteSfx(on);
        sfxSlider.value = on ? PlayerPrefs.GetFloat("vol_sfx", 1f) : 0f;
        UpdateInteractable();
    }

    void UpdateInteractable()
    {
        musicSlider.interactable = musicToggle.isOn;
        sfxSlider.interactable   = sfxToggle.isOn;
        PlayerPrefs.Save();
    }
}

