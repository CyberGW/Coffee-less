using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[TestFixture]
public class BattleModeTest1 {

	bool sceneLoaded = false;
	GameObject playableCharacter;
	MainBattle mainBattle;
	BattleManager battleManager;
	DialogueScript dialogueScript;
	Player player;
	Enemy enemy;
	GameObject enemyStats;
	GameObject playerStats;
	Text enemyHealthBar;
	Text playerHealthBar;
	Text enemyMagicBar;
	Text playerMagicBar;

	public IEnumerator Setup() {
		if (!sceneLoaded) {
			SceneManager.LoadScene ("BattleTestInitialScene", LoadSceneMode.Single);
			yield return null; //Wait for scene to load
			playableCharacter = GameObject.Find ("Player");
			sceneLoaded = true;
		}
		PlayerData.instance.data.Players [0] = new Player ("George", 1, 100, 30, 5, 5, 5, 5, 5, 0, null,
			new MagicAttack ("hi-jump kicked", "Kick with power 15", 3, 15),
			new RaiseDefence ("buffed up against", "Increase your defence by 10%", 2, 0.1f),
			(Texture2D)Resources.Load ("Character1", typeof(Texture2D)));
		playableCharacter.transform.position = new Vector2 (0, 0);
		yield return null;
	}

	public void SetupReferences() {
		mainBattle = GameObject.Find ("BattleCode").GetComponent<MainBattle> ();
		battleManager = mainBattle.manager;
		battleManager.forceCriticalHits = "None";
		player = battleManager.Player;
		enemy = battleManager.Enemy;
		enemyStats = GameObject.Find ("EnemyStats");
		playerStats = GameObject.Find ("PlayerStats");
		enemyHealthBar = enemyStats.transform.Find ("Health/Text").GetComponent<Text>();
		playerHealthBar = playerStats.transform.Find ("Health/Text").GetComponent<Text>();
		enemyMagicBar = enemyStats.transform.Find ("Magic/Text").GetComponent<Text>();
		playerMagicBar = playerStats.transform.Find ("Magic/Text").GetComponent<Text>();
	}

	[UnityTest]
	public IEnumerator B0StartBattle() {
		//Setup
		yield return Setup ();
		ObjectInteraction objectScript = GameObject.Find ("Triangle").GetComponentInChildren<ObjectInteraction> ();
		dialogueScript = GameObject.Find ("Dialogue Manager").GetComponent<DialogueScript> ();

		objectScript.pseudoKeyPress = true;
		yield return WaitForFrames (3); //Wait 3 frames for player to move then trigger to be detected and input processed
		Assert.IsNotNull (GameObject.Find ("DialogueBox")); //Check DialogueBox can be found (is active)
		dialogueScript.pseudoKeyPress = true;
		yield return new WaitForSeconds(1); //Wait 3 frames for player to move then trigger to be detected and input processed
		Assert.AreEqual("Battle", SceneManager.GetActiveScene().name); //Check Battle scene has loaded

		//Check Run Button not enabled
		Assert.False(GameObject.Find("RunButton").GetComponent<Button>().interactable);

		//Perform setup
		SetupReferences();
	}

	[UnityTest]
	public IEnumerator B1AttackButton() {
		BattleButtons attackButton = GameObject.Find ("ButtonHandler").GetComponent<BattleButtons> ();
		attackButton.setPanelActive (); //Click attack button
		yield return null;
		Assert.NotNull (GameObject.Find ("AttacksPanel")); //Check panel shows
		attackButton.setPanelActive (); //Click again
		yield return null;
		Assert.Null (GameObject.Find ("AttacksPanel")); //Check panel hides
	}

	[UnityTest]
	public IEnumerator B2AttackPanelDisplay() {
		BattleButtons attackButton = GameObject.Find ("ButtonHandler").GetComponent<BattleButtons> ();
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

	[UnityTest]
	public IEnumerator B3AttackMove() {
		battleManager.forceEnemyMove = "StandardAttack"; //Make sure enemy performs standard attack
		mainBattle.standardAttack (); //Should do 30 * 10 / 5 = 60 damage
		yield return new WaitForSeconds(3);
		//Enemy Health Bar
		Assert.AreEqual ("Health: 40 / 100", enemyHealthBar.text);
		//Player Health Bar
		Assert.AreEqual ("Health: 90 / 100", playerHealthBar.text);
	}

	[UnityTest]
	public IEnumerator B4MagicMove() {
		enemy.Special1.setUp (battleManager, enemy, player);
		battleManager.forceEnemyMove = "Special1"; // Should do 15 * 5 / 6 = 12.5 = 12 damage (as defence is buffed first in turn)
		mainBattle.special2();
		yield return new WaitForSeconds(3);
		//Check Player stat buff has been applied
		Assert.AreEqual(6, player.Defence); //5 * 0.1 = 5.5 = 6
		//Player Health Bar
		Assert.AreEqual("Health: 78 / 100", playerHealthBar.text); 
		//Enemy Magic Bar
		Assert.AreEqual("Magic: 2 / 5", enemyMagicBar.text);
		//Enemy Health Bar
		Assert.AreEqual ("Health: 40 / 100", enemyHealthBar.text); //Stayed the same
		//Player Magic Bar
		Assert.AreEqual("Magic: 3 / 5", playerMagicBar.text);
	}

	[UnityTest]
	public IEnumerator B5WinBattle() {
		mainBattle.standardAttack();
		yield return new WaitForSeconds(4.2f);
		Assert.AreEqual ("Exp: 100 / 200", playerStats.transform.Find ("Exp/Text").GetComponent<Text> ().text);
		yield return new WaitForSeconds (2);
		Assert.AreEqual ("BattleTestInitialScene", SceneManager.GetActiveScene ().name);
		Assert.AreEqual (50, PlayerData.instance.data.Money);
		Assert.IsInstanceOf (typeof(Shield), PlayerData.instance.data.Items [0]);
	}

	public IEnumerator WaitForFrames(int frames) {
		for (int i=0; i < frames; i++) {
			yield return null;
		}
	}

}
