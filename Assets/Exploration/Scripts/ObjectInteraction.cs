using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A component to add to collider relating to an object that can interact with the player by pressing the space bar.
/// Must show at least one line of dialogue, and can give the player an item or start a battle after dialogue has finished</summary>
public class ObjectInteraction : MonoBehaviour {

	/// <summary>
	/// An array of dialogue, with each element being one line to display on screen at a time
	/// </summary>
	public string[] dialogue;
	public static AudioClip SFX;
	//For testing
	[HideInInspector]
	public bool pseudoKeyPress;
	//Setup
	/// <summary>
	/// An item that will be given to the player at the end of all the dialogue
	/// </summary>
	public GlobalFunctions.ItemTypes treasure = GlobalFunctions.ItemTypes.None;
	/// <summary>
	/// If <c>true</c>, create a battle at the end of all the dialogue, requiring a <see cref="BattleDescriptor"/> component 
	/// </summary>
	[Header("If true, requires a BattleDescriptor component")]
	public bool createBattle = false;

	//Script References
	private DialogueScript dManager;
	private PlayerMovement movementScript;

	/// <summary>
	/// An unique identifer of the object created by concatenating the scene name with the parent object name
	/// </summary>
	private string id;

	// Use this for initialization
	void Start () {
		dManager = FindObjectOfType<DialogueScript> ();
		movementScript = FindObjectOfType<PlayerMovement> ();
		id = SceneManager.GetActiveScene().name + gameObject.transform.parent.gameObject.name;
		if (createBattle) {
			IDictionary<string, bool> active = GlobalFunctions.instance.objectsActive;
			if (active.ContainsKey (id)) {
				gameObject.transform.parent.gameObject.SetActive (active [id]);
			} else {
				GlobalFunctions.instance.objectsActive.Add (id, true);
			}
		}
	}	

	/// <summary>
	/// When player is within collider, check if key is pressed and open dialogue if so
	/// </summary>
	/// <param name="other">The object that has been collided with, checked to see if "Player"</param>
	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.name == "Player" && keyPressed() && !dManager.dialogueActive) {
			SoundManager.instance.playSFX ("interact");
			dManager.showDialogue (dialogue, this);
			movementScript.setCanMove (false);
		}
	}

	/// <summary>
	/// A function to manage key presses, also allowing for false triggerings for testing through <see cref="pseudoKeyPress"/>  
	/// </summary>
	/// <returns><c>true</c>, if key was pressed, <c>false</c> otherwise.</returns>
	public bool keyPressed() {
		bool val = Input.GetKeyDown (KeyCode.Space) || pseudoKeyPress;
		pseudoKeyPress = false;
		return val;
	}

	/// <summary>
	/// Called by <see cref="DialogueScript"/> once all dialogue lines have been read, giving the user an item and/or
	/// starting a battle as appropiate
	/// </summary>
	public void endOfDialogue() {
		if (treasure != GlobalFunctions.ItemTypes.None) {
			DataManager data= PlayerData.instance.data;
			data.addItem ( GlobalFunctions.instance.createItem (treasure) );
			Destroy (gameObject); //Remove trigger to stop player obtaining item again
		}
		if (createBattle) {
			GlobalFunctions.instance.objectsActive [id] = false;
			gameObject.GetComponent<BattleDescriptor> ().createBattle ();
		}
	}
			
}
