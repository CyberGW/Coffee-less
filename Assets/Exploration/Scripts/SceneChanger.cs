using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

	public static bool movePlayer;
	public static Vector2 startPosition;
	public static SceneChanger instance;
	private GameObject player;


	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
		SceneManager.sceneLoaded += sceneChanged;

		player = GameObject.Find ("Player");
	}

	void OnDisable() {
		//Remove delegate if this object is ever destroyed
		SceneManager.sceneLoaded -= sceneChanged;
	}

	private void sceneChanged(Scene scene, LoadSceneMode mode) {
		if (movePlayer) {
			//Change the player position on load
			changePosition (startPosition);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void loadLevel(string newScene) {
		movePlayer = false;
		Initiate.Fade (newScene, Color.black, 3f);
	}

	public void loadLevel(string newScene, Vector2 newPosition) {
		//Set the static start position to the new position for when next scene loads
		movePlayer = true;
		startPosition = newPosition;
		Initiate.Fade (newScene, Color.black, 3f);
	}

	public void changePosition(Vector2 position) {
		//Debug.Log (newPosition);
		player.transform.position = position;
	}

}
