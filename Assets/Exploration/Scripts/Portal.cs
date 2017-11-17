using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

	private SceneChanger sceneManager;
	private AudioClip SFX;

	// Use this for initialization
	void Start () {
		sceneManager = GetComponent<SceneChanger> ();
		SFX = Resources.Load("Audio/transition", typeof(AudioClip)) as AudioClip;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.name == "Player") {
			SoundManager.instance.playSingle (SFX);
			sceneManager.loadLevel ();
		}
	}
}
