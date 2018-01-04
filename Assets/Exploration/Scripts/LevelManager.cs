using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A script used to check and handle when a level is beat by defeating the final boss
///  </summary>
public class LevelManager : MonoBehaviour {

	/// <summary>
	/// The name of the object that should be beat in order to progress
	/// </summary>
	[SerializeField]
	private string bossObjectName = "Boss";
	/// <summary>
	/// The position that the player should spawn at on the world map once beat
	/// </summary>
	private Player newPlayer;
	private string[] playerDesc;
	[SerializeField]
	private Vector2 worldMapExitPosition;

	/// <summary>
	/// When the scene is loaded (either at the start or after exiting the boss battle scene, check if boss has been
	/// defeated and if so increase <see cref="GlobalFunctions.currentLevel"/> and send the player back to the world map.
	/// A new player will also be added to the team with a brief description shown
	/// </summary>
	void Start () {
		switch (GlobalFunctions.instance.currentLevel) {
		case (1):
			newPlayer = new Player ("Hannah", 5, 100, 5, 5, 5, 5, 5, 5, 0, null,
				new IncreaseMoney ("stole money from", "Increase money returns by 50%", 2, 0.5f),
				new MagicAttack ("threw wine battles at", "Thorw wine bottles with damage 15", 2, 15));
			playerDesc = new string[] { "You got a new team member, Hannah! She's from Constantine and has very high luck " +
				"but low defense. Her specials can increase your money gain or attack by throwing wine bottles." };
			break;
		default: 
			break;
		}

		IDictionary<string,bool> objectsActive = GlobalFunctions.instance.objectsActive;
		string key = SceneManager.GetActiveScene ().name + bossObjectName;
		if (objectsActive.ContainsKey(key)) {
			if (!GlobalFunctions.instance.objectsActive [key]) {
				StartCoroutine (WaitThenLoad ());
			}
		}
	}

	/// <summary>
	/// Function called by <see cref="Start"/> once boss has been beaten
	/// Applies all functions along with waiting 5 seconds for user to read dialogue about new player
	/// </summary>
	/// <returns>The then load.</returns>
	private IEnumerator WaitThenLoad() {
		PlayerData.instance.data.addPlayer (newPlayer);
		DialogueScript dManager = FindObjectOfType<DialogueScript> ();
		dManager.showDialogue (playerDesc);
		GlobalFunctions.instance.currentLevel += 1;
		Debug.Log ("Beat the level!");
		yield return new WaitForSeconds (5);
		dManager.pseudoKeyPress = true; //Close the dialogue box
		SceneChanger.instance.loadLevel ("WorldMap", worldMapExitPosition);
	}

}
