using System;
using UnityEngine;

public class MusicManager : Singleton<MusicManager>
{
    public const string PLAYER_PREFS_MUSIC_VOLUME = "Music_Volume";

    public static float musicTime;

    public event EventHandler<float> OnMusicVolumeChanged;

    private AudioSource audioSource;
    private float musicVolume;

    public float MusicVolume
    {
        get => musicVolume;
        set
        {
            value = Mathf.Clamp01(value);
            if (Mathf.Approximately(musicVolume, value))
                return;
            musicVolume = value;
            audioSource.volume = musicVolume;
            PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, musicVolume);
            OnMusicVolumeChanged?.Invoke(this, musicVolume);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        musicVolume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, 1f);
    }

    private void Start()
    {
        audioSource.volume = musicVolume;
        audioSource.time = musicTime;
    }

    private void Update()
    {
        musicTime = audioSource.time;
    }
}
