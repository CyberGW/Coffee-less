using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

	public string newScene;
	public Vector2 newPosition;
	private SceneChanger sceneChanger;
	private AudioClip SFX;

	// Use this for initialization
	void Start () {
		sceneChanger = GameObject.Find("SceneChanger").GetComponent<SceneChanger> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.name == "Player") {
			SoundManager.instance.playSFX ("transition");
			sceneChanger.loadLevel (newScene, newPosition);
		}
	}
}
