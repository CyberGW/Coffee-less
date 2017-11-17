using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

	public string newScene;
	public Vector2 newPosition;
	private static Vector2 startPosition;
	private PlayerMovement player;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerMovement> ();
		//Change the player position on load
		changePosition (startPosition);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

//	void OnTriggerEnter2D (Collider2D other) {
//		if (other.gameObject.name == "Player") {
//			loadLevel();
//		}
//	}

	public void loadLevel() {
		//Set the static start position to the new position for when next scene loads
		startPosition = newPosition;
		Initiate.Fade(newScene,Color.black,3f);
		//SceneManager.LoadScene (newScene, LoadSceneMode.Single);
	}

	public void changePosition(Vector2 newPosition) {
		//Debug.Log (newPosition);
		player.transform.position = newPosition;
	}

}
