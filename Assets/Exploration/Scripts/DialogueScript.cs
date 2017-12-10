using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueScript : MonoBehaviour {

	public bool dialogueActive;
	public bool pseudoKeyPress;
	private string[] dialogueLines;
	private int currentLineIndex;
	private GameObject dialogueBox;
	private Text dialogueText;
	private PlayerMovement movementScript;

	// Use this for initialization
	void Start () {
		movementScript = FindObjectOfType<PlayerMovement> ();
		dialogueBox = gameObject.transform.Find ("DialogueBox").gameObject;
		dialogueText = dialogueBox.transform.Find ("DialogueText").GetComponent<Text> ();
		setInactive ();
	}
	
	// Update is called once per frame
	void Update () {
		if (dialogueActive && keyPress()) {
			if (currentLineIndex + 1 < dialogueLines.Length) {
				currentLineIndex += 1;
				showLine (dialogueLines [currentLineIndex]);
				SoundManager.instance.playSFX ("interact");
			} else {
				StartCoroutine (delaySetInactive ());
			}
		}
	}

	private bool keyPress() {
		bool val = Input.GetKeyDown (KeyCode.Space) || pseudoKeyPress;
		pseudoKeyPress = false;
		return val;
	}

	public void showDialogue(string[] dialogue) {
		dialogueBox.SetActive (true);
		dialogueLines = dialogue;
		currentLineIndex = 0;
		showLine (dialogueLines [currentLineIndex]);
		StartCoroutine (delaySetActive ());
	}

	private void showLine(string line) {
		dialogueText.text = line;
	}

	private void setInactive() {
		dialogueBox.SetActive (false);
		movementScript.setCanMove (true);
		dialogueActive = false;
	}

	private IEnumerator delaySetInactive() {
		yield return new WaitForEndOfFrame ();
		setInactive ();
	}

	private IEnumerator delaySetActive() {
		yield return new WaitForEndOfFrame ();
		dialogueActive = true;
	}

}
