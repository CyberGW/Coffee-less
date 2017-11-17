using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueScript : MonoBehaviour {

	public GameObject dialogueBox;
	public UnityEngine.UI.Text dialogueText;
	public bool dialogueActive;
	private PlayerMovement movementScript;

	// Use this for initialization
	void Start () {
		movementScript = FindObjectOfType<PlayerMovement> ();
		setInactive ();
	}
	
	// Update is called once per frame
	void Update () {
		if (dialogueActive && Input.GetKeyDown (KeyCode.Return)) {
			setInactive ();
		}
	}

	public void showDialogue(string dialogue) {
		dialogueActive = true;
		dialogueBox.SetActive (true);
		dialogueText.text = dialogue;
	}

	public void setInactive() {
		dialogueActive = false;
		dialogueBox.SetActive (false);
		movementScript.setCanMove (true);
	}

}
