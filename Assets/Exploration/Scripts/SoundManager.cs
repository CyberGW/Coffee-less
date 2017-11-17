using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public AudioSource BGMSource;
	public AudioSource SFXSource;
	public static SoundManager instance = null;

	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
	}

	public void playSingle(AudioClip clip) {
		SFXSource.clip = clip;
		SFXSource.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
