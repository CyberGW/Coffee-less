using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Script to handle the main menu
/// </summary>
public class MenuScript : MonoBehaviour {

	/// <summary>
	/// Separate canvas asking if user is sure they want to quit
	/// </summary>
	public Canvas quitMenu;
	/// <summary>
	/// Exit button
	/// </summary>
	public Button exitText;
	/// <summary>
	/// Start Game button
	/// </summary>
	public Button startText;
	GameObject player;
	private Text audioText;
	/// <summary>
	/// Reference for current sound situation
	/// </summary>
	private bool soundOn;

	/// <summary>
	/// Sets us variable references
	/// </summary>
	void Start () {		
		quitMenu = quitMenu.GetComponent<Canvas> ();
		startText = startText.GetComponent<Button> ();
		exitText = exitText.GetComponent<Button> ();
		audioText = GameObject.Find ("Audio").GetComponent<Text> ();
		quitMenu.enabled = false;
		player = GameObject.Find ("Player");
		player.SetActive (false);
		soundOn = SoundManager.instance.BGMSource.mute;
	}

	/// <summary>
	/// Disables "start" and "exit" buttons on mainMenu and activates the quitMenu when "Exit" is selected
	/// </summary>
	public void ExitPress(){		
		quitMenu.enabled = true;
		exitText.enabled = false;
		startText.enabled = false;

	}
	/// <summary>
	/// When "no" is selected on quitMenu disable quitMenu and reenable start and exit buttons on mainMenu
	/// </summary>
	public void NoPress(){
		quitMenu.enabled = false;
		exitText.enabled = true;
		startText.enabled = true;
	}

	/// <summary>
	/// Start the game from the initial level
	/// </summary>
	public void StartLevel(){
		SoundManager.instance.playSFX ("interact");
		player.SetActive (true);
		SceneChanger.instance.loadLevel ("CS-Jail", new Vector2 (0, 0));
	}

	/// <summary>
	/// Closes application
	/// </summary>
	public void ExitGame() {
		Application.Quit ();
	}

	/// <summary>
	/// If <see cref="soundOn"/> is <c>true</c>, then turn sound off and update variables and text
	/// If <c>false</c>, then turn sound on and update variables and text similarly 
	/// </summary>
	public void sound() {
		if (soundOn) {
			SoundManager.instance.soundOn (false);
			soundOn = false;
			audioText.text = "Sound: Off";
		} else {
			SoundManager.instance.soundOn (true);
			soundOn = true;
			audioText.text = "Sound: On";
		}
	}
			
}
