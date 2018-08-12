using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioOutputController : MonoBehaviour, IAudioInfo, IAudioReceiver
{
    private AudioSource mainThemeSource;
    private List<GameObject> audioPlayers;
    private SoundManager soundManager;

	[SerializeField] private AudioClip mainTheme;
    [SerializeField] private float bpm;
    [SerializeField] private float pitch;
    private float songDSPtime;

    public float GetBpm()
    {
        return bpm;
    }

    public float GetSongPosition()
    {
		return (float)(AudioSettings.dspTime - songDSPtime) * pitch;
    }

    public AudioClip GetSong()
    {
		return mainTheme;
    }

    private void PlayMainThemeSong(AudioClip theme)
    {
		mainThemeSource.clip = theme;
        mainThemeSource.Play();
    }

    void Awake()
    {
        audioPlayers = new List<GameObject>();
        mainThemeSource = GetComponent<AudioSource>();
        soundManager = GetComponent<SoundManager>();
        mainThemeSource.pitch = pitch;
        songDSPtime = (float)AudioSettings.dspTime;
		PlayMainThemeSong(mainTheme);
    }

    public void PlaySound(string key)
    {
        soundManager.Play2DSFX(key);
    }

    public void StopMainTheme()
    {
        mainThemeSource.Stop();
    }
}
