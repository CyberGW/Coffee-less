using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplorationMenuScript : MonoBehaviour {

	public bool menuActive;
	//public bool pseudoKeyPress;
	private GameObject menuBox;
	private PlayerMovement movementScript;

//	public Button inventButton;
//	public Button partyButton;
//	public Button saveButton;
//	public Button optionButton;
//	public Button exitButton;

	// Use this for initialization
	void Start () {

//		inventButton = inventButton.GetComponent<Button> ();
//		partyButton = partyButton.GetComponent<Button> ();
//		saveButton = saveButton.GetComponent<Button> ();
//		optionButton = optionButton.GetComponent<Button> ();
//		exitButton = exitButton.GetComponent<Button> ();

		movementScript = FindObjectOfType<PlayerMovement> ();
		//menuBox = gameObject.transform.Find ("MenuScript").gameObject;
		SceneChanger.instance.menuOpen = true;
		//setInactive ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
//			SceneChanger.instance.menuOpen = false;
//			movementScript.setCanMove (true);
//			UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(20);
		}
	}

	private void setInactive() {
		menuBox.SetActive (false);
		movementScript.setCanMove (true);
		menuActive = false;
	}

	public void inventPressed() {
		SceneChanger.instance.menuOpen = false;
		SceneChanger.instance.loadLevel ("ItemMenu");
	}

	public void partyPressed() {
		SceneChanger.instance.menuOpen = false;
		SceneChanger.instance.loadLevel ("Party");
	}

	public void savePressed() {
		//UnityEngine.SceneManagement.SceneManager.LoadScene ();
	}

	public void optionPressed() {
		//UnityEngine.SceneManagement.SceneManager.LoadScene ();
	}

	public void exitPressed() {
		SceneChanger.instance.menuOpen = false;
		SceneChanger.instance.loadLevel ("mainmenu1");
	}

	/*
	private bool keyPress() {
		bool val = Input.GetKeyDown (KeyCode.Escape) || pseudoKeyPress;
		pseudoKeyPress = false;
		return val;
	}

	public void showDialogue(string[] dialogue) {
		menuBox.SetActive (true);
		StartCoroutine (delaySetActive ());
	}

	private IEnumerator delaySetInactive() {
		yield return new WaitForEndOfFrame ();
		setInactive ();
	}

	private IEnumerator delaySetActive() {
		yield return new WaitForEndOfFrame ();
		menuActive = true;
	}
	*/
}
