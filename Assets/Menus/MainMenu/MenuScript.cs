using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Script to handle the main menu
/// </summary>
public class MenuScript : MonoBehaviour {

	public Canvas quitMenu;
	public Button exitText;
	public Button startText;

	// Use this for initialization
	void Start () {
		
		quitMenu = quitMenu.GetComponent<Canvas> ();
		startText = startText.GetComponent<Button> ();
		exitText = exitText.GetComponent<Button> ();
		quitMenu.enabled = false;
	}

	public void ExitPress(){
		
		quitMenu.enabled = true;
		exitText.enabled = false;
		startText.enabled = false;

	}

	public void NoPress(){
		quitMenu.enabled = false;
		exitText.enabled = true;
		startText.enabled = true;
	}

	public void StartLevel(string newGameLevel){
		SoundManager.instance.playSFX ("interact");
		Initiate.Fade ("CS-Jail", Color.black, 1f);
	}

	public void ExitGame(){
		
		Application.Quit ();

	}

		
}
