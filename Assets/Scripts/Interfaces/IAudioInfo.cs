using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAudioInfo {
	AudioClip GetSong();
	float GetBpm();
	float GetSongPosition();
}
