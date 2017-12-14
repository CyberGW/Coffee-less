using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDialogue : MonoBehaviour {

	public string[] dialogue;
	public static AudioClip SFX;
	//For testing
	public bool pseudoKeyPress;
	public string treasure;
	public Item item;
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
			if (treasure != "") {
				Dictionary<string, Item> items = GameObject.Find ("GlobalData").GetComponent<ItemObjects> ().items;
				DataManager data= PlayerData.instance.data;
				data.addItem (items [treasure]);
				Destroy (gameObject); //Remove trigger to stop player obtaining item again
			}
		}
	}

	public bool keyPressed() {
		bool val = Input.GetKeyDown (KeyCode.Space) || pseudoKeyPress;
		pseudoKeyPress = false;
		return val;
	}
		
}
