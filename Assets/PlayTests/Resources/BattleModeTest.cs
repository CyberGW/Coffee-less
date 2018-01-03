using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[TestFixture]
public class BattleModeTest {

	bool sceneLoaded = false;
	GameObject player;
	PlayerMovement playerScript;
	MainBattle mainBattle;
	BattleManager battleManager;

	public IEnumerator Setup() {
		if (!sceneLoaded) {
			SceneManager.LoadScene ("BattleTestInitialScene", LoadSceneMode.Single);
			yield return null; //Wait for scene to load
			player = GameObject.Find ("Player");
			playerScript = player.GetComponent<PlayerMovement> ();
			sceneLoaded = true;
		}
		player.transform.position = new Vector2 (0, 0);
		yield return null;
	}

	[UnityTest]
	public IEnumerator A1StartBattle() {
		//Setup
		yield return Setup ();
		ObjectInteraction objectScript = GameObject.Find ("Triangle").GetComponentInChildren<ObjectInteraction> ();
		DialogueScript dialogueScript = GameObject.Find ("Dialogue Manager").GetComponent<DialogueScript> ();

		objectScript.pseudoKeyPress = true;
		yield return WaitForFrames (3); //Wait 3 frames for player to move then trigger to be detected and input processed
		Assert.IsNotNull (GameObject.Find ("DialogueBox")); //Check DialogueBox can be found (is active)
		dialogueScript.pseudoKeyPress = true;
		yield return new WaitForSeconds(2); //Wait 3 frames for player to move then trigger to be detected and input processed
		Assert.AreEqual("Battle", SceneManager.GetActiveScene().name); //Check Battle scene has loaded
	}

	[UnityTest]
	public IEnumerator A2AttackButton() {
		mainBattle = GameObject.Find ("BattleCode").GetComponent<MainBattle> ();
		battleManager = mainBattle.manager;
		battleManager.forceCriticalHits = "None";
		AttackButton attackButton = GameObject.Find ("AttackButtonHandler").GetComponent<AttackButton> ();
		attackButton.setPanelActive (); //Click attack button
		yield return null;
		Assert.NotNull (GameObject.Find ("AttacksPanel")); //Check panel shows
		attackButton.setPanelActive (); //Click again
		yield return null;
		Assert.Null (GameObject.Find ("AttacksPanel")); //Check panel hides
	}

	[UnityTest]
	public IEnumerator A3AttackPanelDisplay() {
		AttackButton attackButton = GameObject.Find ("AttackButtonHandler").GetComponent<AttackButton> ();
		attackButton.setPanelActive (); //Click attack button
		yield return null;
		Player player = PlayerData.instance.data.Players [0];
		checkPanelMagicSpell (player.Special1, GameObject.Find ("MagicSpell1"));
		checkPanelMagicSpell (player.Special2, GameObject.Find ("MagicSpell2"));
	}

	private void checkPanelMagicSpell (SpecialMove specialMove, GameObject containerObject) {
		Text desc = containerObject.transform.Find ("Desc").GetComponent<Text> ();
		Assert.AreEqual(specialMove.Desc, desc.text);
		Text magic = containerObject.transform.Find ("Magic").GetComponent<Text> ();
		Assert.AreEqual ("Magic: " + specialMove.Magic, magic.text);
	}

	public IEnumerator WaitForFrames(int frames) {
		for (int i=0; i < frames; i++) {
			yield return null;
		}
	}

}
