using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script to manage all background music and sound effects in the game
/// </summary>
public class SoundManager : MonoBehaviour {

	public AudioSource BGMSource;
	public AudioSource SFXSource;
	public static SoundManager instance = null;
	private IDictionary<string, AudioClip> soundEffects;

	/// <summary>
	/// Setup object and load all sound effects into <see cref="soundEffects"/> dictionary 
	/// </summary>
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

	/// <summary>
	/// Plays a background track
	/// </summary>
	/// <param name="clip">The background music to play</param>
	public void playBGM(AudioClip clip) {
		BGMSource.clip = clip;
		BGMSource.Play ();
	}

	/// <summary>
	/// Play a sound effect
	/// </summary>
	/// <param name="SFX">The name of the sound effect to reference within <see cref="soundEffects"/> </param>
	public void playSFX(string SFX) {
		SFXSource.clip = soundEffects [SFX];
		SFXSource.Play ();
	}

	/// <summary>
	/// Turn sound on and off
	/// </summary>
	/// <param name="val">If set to <c>true</c>, plays music. Otherwise play no sounds.</param>
	public void soundOn(bool val) {
		BGMSource.mute = !val;
		SFXSource.mute = !val;
	}

}
