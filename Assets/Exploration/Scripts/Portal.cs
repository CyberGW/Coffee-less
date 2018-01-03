using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A component to add to an object, such as when the player touches it they are teleported to a new scene
/// </summary>
public class Portal : MonoBehaviour {

	/// <summary>The name of the scene to load when the portal is activated</summary>
	public string newScene;
	/// <summary>The position the player should be set to within the new scene</summary>
	public Vector2 newPosition;
	private SceneChanger sceneChanger;
	/// <summary>Sound effect to play when transitioning</summary>
	private AudioClip SFX;

	// Use this for initialization
	void Start () {
		sceneChanger = GameObject.Find("SceneChanger").GetComponent<SceneChanger> ();
	}

	/// <summary>
	/// Plays the sound effect and loads the new scene when this object collides with the player
	/// </summary>
	/// <param name="other">The object this object has collided with. Functionality is activated when this
	/// is the player</param>
	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.name == "Player") {
			SoundManager.instance.playSFX ("transition");
			sceneChanger.loadLevel (newScene, newPosition);
		}
	}
}
