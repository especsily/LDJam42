using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioOutputController : MonoBehaviour, IAudioOutputReceiver, IAudioInfo
{
    [SerializeField] private GameObject audioPlayerPrefab;

    private AudioSource mainThemeSource;
    private List<GameObject> audioPlayers;

	[SerializeField] private AudioClip mainTheme;
    [SerializeField] private float bpm;
    private float songDSPtime;

    public float GetBpm()
    {
        return bpm;
    }

    public float GetSongPosition()
    {
		return (float)(AudioSettings.dspTime - songDSPtime);
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

    public void PlaySound(AudioClip sound)
    {
        //pool
        bool needCreate = true;
		GameObject player = null;
        foreach (var audioPlayer in audioPlayers)
        {
            if (audioPlayer.activeSelf)
            {
                continue;
            }
            else
            {
                needCreate = false;
				player = audioPlayer;
                player.SetActive(true);
            }
        }

        if (needCreate)
        {
			player = Instantiate(audioPlayerPrefab, Vector3.zero, Quaternion.identity, transform);
        }
        var audio = player.GetComponent<AudioSource>();
        audio.clip = sound;
        audio.Play();
    }

    void Awake()
    {
        audioPlayers = new List<GameObject>();
        mainThemeSource = GetComponent<AudioSource>();
        songDSPtime = (float)AudioSettings.dspTime;
		PlayMainThemeSong(mainTheme);
    }
}
