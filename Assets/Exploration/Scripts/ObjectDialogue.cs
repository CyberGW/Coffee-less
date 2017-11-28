using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDialogue : MonoBehaviour {

	public string[] dialogue;
	public static AudioClip SFX;
	private DialogueScript dManager;
	private PlayerMovement movementScript;

	// Use this for initialization
	void Start () {
		dManager = FindObjectOfType<DialogueScript> ();
		movementScript = FindObjectOfType<PlayerMovement> ();
	}	

	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.name == "Player" && Input.GetKeyDown (KeyCode.Space) && !dManager.dialogueActive) {
			SoundManager.instance.playSFX ("interact");
			dManager.showDialogue (dialogue);
			movementScript.setCanMove (false);
		}
	}
}
