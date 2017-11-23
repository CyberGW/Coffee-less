using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueScript : MonoBehaviour {

	public bool dialogueActive;
	private GameObject dialogueBox;
	private UnityEngine.UI.Text dialogueText;
	private PlayerMovement movementScript;

	// Use this for initialization
	void Start () {
		movementScript = FindObjectOfType<PlayerMovement> ();
		dialogueBox = gameObject.transform.Find ("DialogueBox").gameObject;
		dialogueText = dialogueBox.transform.Find ("DialogueText").GetComponent<UnityEngine.UI.Text> ();
		setInactive ();
	}
	
	// Update is called once per frame
	void Update () {
		if (dialogueActive && Input.GetKeyDown (KeyCode.Space)) {
			StartCoroutine(delaySetInactive ());
		}
	}

	public void showDialogue(string dialogue) {
		dialogueBox.SetActive (true);
		dialogueText.text = dialogue;
		StartCoroutine (delaySetActive ());
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
