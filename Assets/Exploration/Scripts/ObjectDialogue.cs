using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDialogue : MonoBehaviour {

	public string dialogue;
	public static AudioClip SFX;
	private DialogueScript dManager;
	private PlayerMovement movementScript;

	// Use this for initialization
	void Start () {
		SFX = Resources.Load("Audio/interact", typeof(AudioClip)) as AudioClip;
		dManager = FindObjectOfType<DialogueScript> ();
		movementScript = FindObjectOfType<PlayerMovement> ();
	}	

	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.name == "Player" && Input.GetKeyUp (KeyCode.Space) && !dManager.dialogueActive) {
			SoundManager.instance.playSingle (SFX);
			dManager.showDialogue (dialogue);
			movementScript.setCanMove (false);
		}
	}
}
