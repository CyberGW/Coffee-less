using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A set of functions to be called to change scenes smoothly whilst handling the player's
/// location and ability to move throw <see cref="PlayerMovement"/> 
/// </summary>
public class SceneChanger : MonoBehaviour {

	/// <summary>Set to <c>true</c> if the player's location is to change on next scene load</summary>
	public static bool movePlayer;
	/// <summary>The position to load the player at when next scene is loaded</summary>
	public static Vector2 startPosition;
	/// <summary> A static variable relating to this script so functions can be easily accessed by <c> SceneChanger.instance....</c>
	/// </summary>
	public static SceneChanger instance;
	private GameObject player;
	private PlayerMovement movementScript;
	public bool menuOpen = false;
	public string menuScene;


	void Awake() {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
			SceneManager.sceneLoaded += sceneChanged;
			player = GameObject.Find ("Player");
			movementScript = player.GetComponent<PlayerMovement> ();
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	/// <summary>
	///Removes <see cref="sceneChanged"/> as a delegate when disabled
	/// </summary>
	void OnDisable() {
		//Remove delegate if this object is ever destroyed
		SceneManager.sceneLoaded -= sceneChanged;
	}

	/// <summary>
	/// Called when a new scene is loaded, allowing the player's position to be changed and enable their movement if appropiate
	/// </summary>
	/// <param name="scene">Obligatory parameter specifying the newly loaded scene</param>
	/// <param name="mode">Obligatory parameter specifying the way the scene was loaded</param>
	private void sceneChanged(Scene scene, LoadSceneMode mode) {
		if (movePlayer) {
			//Change the player position on load
			changePosition (startPosition);
			movePlayer = false;
		}
		if (scene.name != "Battle") {
			//Allow player to move again
			movementScript.setCanMove (true);
		}
		if (menuOpen) {
			movementScript.setCanMove (false);
			if (scene.name != "GameMenu") {
				SceneManager.LoadScene ("GameMenu", LoadSceneMode.Additive);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void hidePlayer() {
		player.SetActive (false);
	}

	/// <summary>Load a new level, without moving the player. </summary>
	/// <param name="newScene">The scene to load</param>
	public void loadLevel(string newScene) {
		movePlayer = false;
		changeScene (newScene);
	}

	/// <summary>
	/// Load a new level, and moves the player upon load
	/// </summary>
	/// <param name="newScene">The scene to load</param>
	/// <param name="newPosition">The position the player should be at in the new scene</param>
	public void loadLevel(string newScene, Vector2 newPosition) {
		//Set the static start position to the new position for when next scene loads
		movePlayer = true;
		startPosition = newPosition;
		changeScene (newScene);
	}

	/// <summary>
	/// Provides a smooth transition to the new scene whilst disabling the player's movement during transition.
	/// Called by both loadLevel functions
	/// <param name="newScene">The scene to load</param>
	private void changeScene(string newScene) {
		movementScript.setCanMove (false);
		Initiate.Fade (newScene, Color.black, 3f);
	}

	/// <summary>
	/// Changes the position of the player.
	/// Called on a scene load by <see cref="sceneChanged"/> if <see cref="movePlayer"/> is <c>true</c>  
	/// </summary>
	/// <param name="position">The position the player should be located at</param>
	public void changePosition(Vector2 position) {
		//Debug.Log (newPosition);
		player.transform.position = position;
	}

}
