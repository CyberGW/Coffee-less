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
	/// Start the game from the initial level, setting up the initial global data
	/// </summary>
	public void StartLevel(){
		
		//Setup global data to initial values
		PlayerData.instance.data = new DataManager (
			new Player ("George", 1, 100, 5, 5, 5, 5, 5, 5, 0, null,
				new MagicAttack ("hi-jump kicked", "Kick with power 15", 3, 15),
				new RaiseDefence ("buffed up against", "Increase your defence by 10%", 2, 0.1f),
				(Texture2D)Resources.Load ("Character1", typeof(Texture2D))));
		PlayerData.instance.data.addPlayer (new Player ("Hannah", 1, 100, 5, 3, 5, 5, 15, 5, 0, null,
				new IncreaseMoney ("stole money from", "Increase money returns by 50%", 2, 0.5f),
				new MagicAttack ("threw wine battles at", "Throw wine bottles with damage 15", 2, 15),
				(Texture2D) Resources.Load("Character2", typeof(Texture2D))));
		GlobalFunctions.instance.currentLevel = 0;
		GlobalFunctions.instance.objectsActive = new Dictionary<string, bool> ();

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
