using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour {
	private AudioSource audioSource;
	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource>();
	}
	
	void Update()
	{
		if(!audioSource.isPlaying)
		{
			gameObject.SetActive(false);
		}
	}
}
