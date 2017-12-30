using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingStation : MonoBehaviour {

	private DialogueScript dManager;
	private PlayerMovement movementScript;
	private string[] text;
	private bool inRange = false;

	// Use this for initialization
	void Start () {
		dManager = FindObjectOfType<DialogueScript> ();
		movementScript = FindObjectOfType<PlayerMovement> ();
		text = new string[1];
		text [0] = "All teammates have been healed";
	}
	
	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.name == "Player" && !inRange && !dManager.dialogueActive) {
			inRange = true;
			SoundManager.instance.playSFX ("interact");
			dManager.showDialogue (text);
			movementScript.setCanMove (false);
			healPlayers ();
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.name == "Player") {
			inRange = false;
		}
	}

	public void healPlayers() {
		Player[] playerArray = PlayerData.instance.data.Players;
		foreach (Player player in playerArray) {
			if (player != null) {
				player.Health = 100;
			}
		}
	}
		
}
