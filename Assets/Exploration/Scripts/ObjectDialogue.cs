using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDialogue : MonoBehaviour {

	public string[] dialogue;
	public static AudioClip SFX;
	//For testing
	public bool pseudoKeyPress;
	private DialogueScript dManager;
	private PlayerMovement movementScript;

	// Use this for initialization
	void Start () {
		dManager = FindObjectOfType<DialogueScript> ();
		movementScript = FindObjectOfType<PlayerMovement> ();
	}	

	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.name == "Player" && keyPressed() && !dManager.dialogueActive) {
			SoundManager.instance.playSFX ("interact");
			dManager.showDialogue (dialogue);
			movementScript.setCanMove (false);
		}
	}

	public bool keyPressed() {
		bool val = Input.GetKeyDown (KeyCode.Space) || pseudoKeyPress;
		pseudoKeyPress = false;
		return val;
	}
		
}
