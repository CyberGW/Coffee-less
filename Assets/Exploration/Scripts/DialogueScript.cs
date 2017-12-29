using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script to show dialogue, as well as step through multiple lines and close afterwards
/// </summary>
public class DialogueScript : MonoBehaviour {

	public bool dialogueActive;
	public bool pseudoKeyPress;
	private string[] dialogueLines;
	private int currentLineIndex;
	private GameObject dialogueBox;
	private Text dialogueText;
	private PlayerMovement movementScript;
	private ObjectInteraction caller;

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

	/// <summary>
	/// Determine whether an appropiate key was pressed
	/// </summary>
	/// <returns><c>true</c>, if the key was pressed, <c>false</c> otherwise.</returns>
	private bool keyPress() {
		bool val = Input.GetKeyDown (KeyCode.Space) || pseudoKeyPress;
		pseudoKeyPress = false;
		return val;
	}

	/// <summary>
	/// Shows the dialogue box and the first line of text by calling <see cref="showLine"/> 
	/// </summary>
	/// <param name="dialogue">A string array containing all the lines of dialogue</param>
	public void showDialogue(string[] dialogue, ObjectInteraction caller) {
		this.caller = caller;
		dialogueBox.SetActive (true);
		dialogueLines = dialogue;
		currentLineIndex = 0;
		showLine (dialogueLines [currentLineIndex]);
		StartCoroutine (delaySetActive ());
	}

	/// <summary>
	/// Shows a line of text
	/// </summary>
	/// <param name="line">The string of text to display</param>
	private void showLine(string line) {
		dialogueText.text = line;
	}

	/// <summary>
	/// Closes the dialogue box, whilst renabling player movement
	/// Will also call <see cref="ObjectInteraction.endOfDialogue"/> afterwards to add an item or start a battle as appropiate 
	/// </summary>
	private void setInactive() {
		dialogueBox.SetActive (false);
		movementScript.setCanMove (true);
		dialogueActive = false;
		if (caller != null) {
			caller.endOfDialogue ();
		}

	}

	/// <summary>
	/// Waits a frame after key pressed before closing dialogue box, so the same key press isn't picked up to reopen a new
	/// dialogue box
	/// </summary>
	/// <returns>Coroutine to wait for end of frame</returns>
	private IEnumerator delaySetInactive() {
		yield return new WaitForEndOfFrame ();
		setInactive ();
	}

	/// <summary>
	/// Waits a frame after key pressed before opening dialogue box, so the same key press isn't picked up to the next line of text
	/// or close the dialogue box
	/// </summary>
	/// <returns>Coroutine to wait for end of frame</returns>
	private IEnumerator delaySetActive() {
		yield return new WaitForEndOfFrame ();
		dialogueActive = true;
	}

}
