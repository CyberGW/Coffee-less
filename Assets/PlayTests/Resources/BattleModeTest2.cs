using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[TestFixture]
public class BattleModeTest2 {

	bool sceneLoaded = false;
	GameObject playableCharacter;
	PlayerMovement movementScript;
	MainBattle mainBattle;
	BattleManager battleManager;
	DialogueScript dialogueScript;
	SwitchPlayersScript partyScript;
	BattleButtons buttonManager;
	Player player;
	Enemy enemy;

	public IEnumerator Setup() {
		if (!sceneLoaded) {
			SceneManager.LoadScene ("BattleTestInitialScene", LoadSceneMode.Single);
			yield return null; //Wait for scene to load
			playableCharacter = GameObject.Find ("Player");
			movementScript = playableCharacter.GetComponent<PlayerMovement> ();
			sceneLoaded = true;
		}
		playableCharacter.transform.position = new Vector2 (0, 0);
		yield return null;
	}

	public void SetupReferences() {
		mainBattle = GameObject.Find ("BattleCode").GetComponent<MainBattle> ();
		battleManager = mainBattle.manager;
		battleManager.forceCriticalHits = "None";
		player = battleManager.Player;
		enemy = battleManager.Enemy;
		buttonManager = GameObject.Find ("ButtonHandler").GetComponent<BattleButtons> ();
	}

	[UnityTest]
	public IEnumerator B6StartNextBattle() {
		yield return Setup ();
		//Add new player for testing
		DataManager data = PlayerData.instance.data;
		data.addPlayer (new Player ("Hannah", 5, 100, 5, 5, 5, 5, 5, 5, 0, null,
			new IncreaseMoney ("stole money from", "Increase money returns by 50%", 2, 0.5f),
			new MagicAttack ("threw wine battles at", "Thorw wine bottles with damage 15", 2, 15),
			(Texture2D)Resources.Load ("Character2", typeof(Texture2D))));

		//Move to next fight
		yield return moveForFrames(20, "Left");

		ObjectInteraction objectScript = GameObject.Find ("Character").GetComponentInChildren<ObjectInteraction> ();
		dialogueScript = GameObject.Find ("Dialogue Manager").GetComponent<DialogueScript> ();
		objectScript.pseudoKeyPress = true;
		yield return WaitForFrames (3);
		dialogueScript.pseudoKeyPress = true;
		yield return new WaitForSeconds(2); //Wait 3 frames for player to move then trigger to be detected and input processed
		Assert.AreEqual("Battle", SceneManager.GetActiveScene().name); //Check Battle scene has loaded

		//Check Run Button is enabled
		Assert.True(GameObject.Find("RunButton").GetComponent<Button>().interactable);

		//Perform setup
		SetupReferences();
	}

	[UnityTest]
	public IEnumerator B7ChangePlayerMenu() {
		buttonManager.showPlayerMenu ();
		yield return WaitForFrames (20);

		//Check back button works
		Assert.True (GameObject.Find ("SwitchCanvas/BackButton").GetComponent<Button> ().interactable);
		partyScript = GameObject.Find ("SwitchCanvas").GetComponent<SwitchPlayersScript> ();

		//Switch to Hannah first, but have here killed instantly by Enemy
		enemy.Special1.setUp (battleManager, enemy, player);
		battleManager.forceEnemyMove = "Special1";
		partyScript.switchPlayers (1);
		yield return new WaitForSeconds (1);
		Assert.AreEqual ("Hannah", battleManager.Player.Name);
		yield return new WaitForSeconds (5);
	}

	[UnityTest]
	public IEnumerator B8ChangePlayerOnceFainted() {
		//Check Button for Hannah is disabled
		Assert.Null (GameObject.Find ("SwitchCanvas/Margin/Player1").GetComponent<Button> ());
		//Put George back in
		partyScript.switchPlayers (1);
		yield return new WaitForSeconds (3);
		Assert.AreEqual ("George", battleManager.Player.Name);
		yield return new WaitForSeconds (1);
	}

	[UnityTest]
	public IEnumerator B9CheckWhenGameOver() {
		enemy.Special1.setUp (battleManager, enemy, player);
		battleManager.forceEnemyMove = "Special1";
		mainBattle.standardAttack ();
		yield return new WaitForSeconds (6);
		Assert.AreEqual ("mainmenu1", SceneManager.GetActiveScene ().name);
	}

	public IEnumerator WaitForFrames(int frames) {
		for (int i=0; i < frames; i++) {
			yield return null;
		}
	}

	public IEnumerator moveForFrames(int frames, string direction) {
		for (int i = 0; i < frames; i++) {
			movementScript.move (direction);
			yield return new WaitForFixedUpdate();
		}
	}

}
