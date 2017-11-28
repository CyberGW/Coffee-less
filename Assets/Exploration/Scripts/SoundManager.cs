using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public AudioSource BGMSource;
	public AudioSource SFXSource;
	public static SoundManager instance = null;
	private IDictionary<string, AudioClip> soundEffects;

	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);

		//Setup Sound Effects Dictionary
		soundEffects = new Dictionary<string, AudioClip> ();
		soundEffects.Add ("transition", Resources.Load ("Audio/transition", typeof(AudioClip)) as AudioClip);
		soundEffects.Add ("interact", Resources.Load ("Audio/interact", typeof(AudioClip)) as AudioClip);
	}

	public void playBGM(AudioClip clip) {
		BGMSource.clip = clip;
		BGMSource.Play ();
	}

	public void playSFX(string SFX) {
		SFXSource.clip = soundEffects [SFX];
		SFXSource.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
