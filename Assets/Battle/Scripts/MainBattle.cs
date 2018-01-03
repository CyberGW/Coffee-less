using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script for handing a battle scene. Uses <see cref="BattleManager"/>, but handles turn order and execution as well
/// as updating the display appropriately </summary>
public class MainBattle : MonoBehaviour {

	//Objects
	private GameObject globalData;
	public BattleManager manager;
	private Player[] playerArray;
	private Button attackButton;
	private Button runButton;
	private Text textBox;
	//Battle Manager References
	public Player player;
	public Enemy enemy;
	private int moneyReward;
	private Item itemReward;
	//Local Variables
	private string text;
	//Test Enemy
	private Enemy enemyObject;
	//Moves
	private CharacterMove playerMove;
	private CharacterMove enemyMove;
	private bool moveChosen;
	//UI
	private GameObject attacksPanel;
	private GameObject playerStats;
	private GameObject enemyStats;
	private IDictionary<Character, StatsScript> healthBar;
	private IDictionary<Character, StatsScript> magicBar;
	private StatsScript expBar;
	private Image enemySprite;
	//Scene Management
	private GameObject playerCamera;
	//Music
	private AudioClip BGM;
	private AudioClip victory;


	/// <summary>
	/// Start this instance.
	/// Includes finding game objects, setting references and changing background music
	/// </summary>
	void Start () {
		
		//Find Objects
		attacksPanel = GameObject.Find("BattleCanvas").transform.Find("AttacksPanel").gameObject;
		playerStats = GameObject.Find("PlayerStats");
		enemyStats = GameObject.Find ("EnemyStats");
		attackButton = GameObject.Find ("AttackButton").GetComponent<Button> ();
		runButton = GameObject.Find ("RunButton").GetComponent<Button> ();
		runButton.interactable = GlobalFunctions.instance.canRunAway;
		textBox = GameObject.Find ("TextBox").transform.Find ("Text").GetComponent<Text> ();
		enemySprite = GameObject.Find ("EnemyImage").GetComponent<Image> ();
		Texture2D image = GlobalFunctions.instance.sprite;
		enemySprite.sprite = Sprite.Create (image, new Rect (0.0f, 0.0f, image.width, image.height), new Vector2 (0.5f, 0.5f));


		//Setup Object references
		playerArray = PlayerData.instance.data.Players;
		player = playerArray [0];
		enemyObject = GlobalFunctions.instance.getEnemy ();
		moneyReward = GlobalFunctions.instance.getMoney ();
		itemReward = GlobalFunctions.instance.getItem ();
		manager = new BattleManager (playerArray[0], enemyObject, moneyReward);
		player = manager.Player;
		enemy = manager.Enemy;

		//Bars
		healthBar = new Dictionary<Character, StatsScript>();
		healthBar[player] = playerStats.transform.Find("Health").GetComponent<StatsScript> ();
		healthBar[enemy] = enemyStats.transform.Find("Health").GetComponent<StatsScript> ();
		magicBar = new Dictionary<Character, StatsScript>();
		magicBar[player] = playerStats.transform.Find ("Magic").GetComponent<StatsScript> ();
		magicBar[enemy] = enemyStats.transform.Find ("Magic").GetComponent<StatsScript> ();
		expBar = playerStats.transform.Find ("Exp").GetComponent<StatsScript> ();

		healthBar[player].setUpDisplay (player.Health, 100);
		healthBar[enemy].setUpDisplay (enemy.Health, 100);
		magicBar[player].setUpDisplay (player.Magic, player.MaximumMagic);
		magicBar[enemy].setUpDisplay (enemy.Magic, enemy.MaximumMagic);
		expBar.setUpDisplay (player.Exp, player.ExpToNextLevel);

		//Setup local variables
		moveChosen = false;

		//Change Music
		BGM = Resources.Load("Audio/battle", typeof(AudioClip)) as AudioClip;
		SoundManager.instance.playBGM(BGM);
		victory = Resources.Load ("Audio/victory", typeof(AudioClip)) as AudioClip;

	}
	
	/// <summary>
	/// Update this instance.
	/// If a move is chosen, then call <see cref="playerThenEnemy"/> or <see cref="enemyThenPlayer"/> dependent upon
	/// <see cref="BattleManager.playerFirst()"/> 
	/// </summary>
	void Update () {
		if (moveChosen) {
			if (manager.playerFirst()) {
				StartCoroutine (playerThenEnemy ());
			} else {
				StartCoroutine (enemyThenPlayer ());
			}
			moveChosen = false;
		}
	}		

	/// <summary>
	/// Performs the player's turn, then the enemy's\n
	/// Re-enables attack button afterwards
	/// </summary>
	/// <returns>Coroutine functions to perform the turns</returns>
	private IEnumerator playerThenEnemy () {
		yield return StartCoroutine (performTurn(playerMove));
		if (!manager.battleWon()) {
			yield return StartCoroutine (performTurn(enemyMove));
			attackButton.interactable = true;
		}
	}

	/// <summary>
	/// Performs the enemy's turn, then the player's\n
	/// Re-enables attack button afterwards
	/// </summary>
	/// <returns>Coroutine functions to perform the turns</returns>
	private IEnumerator enemyThenPlayer() {
		yield return StartCoroutine (performTurn(enemyMove));
		if (!manager.playerFainted()) {
			yield return StartCoroutine (performTurn(playerMove));
			attackButton.interactable = true;
		}
	}

	/// <summary>
	/// Performs the turn, updating text display and health and magic bars.
	/// Afterwards, checks to see whether the player has won or lost
	/// </summary>
	/// <returns>Coroutine functions</returns>
	/// <param name="move">The move to  be performed</param>
	private IEnumerator performTurn(CharacterMove move) {
		int previousHealth = move.Target.Health;
		int previousMagic = move.User.Magic;
		move.performMove ();
		textBox.text = move.User.Name + " " + move.Text + " " + move.Target.Name;
		if (manager.WasCriticalHit) {
			textBox.text += "\nCritical Hit!";
		}
		StartCoroutine (healthBar [move.Target].updateDisplay (previousHealth, move.Target.Health));
		yield return StartCoroutine (magicBar [move.User].updateDisplay (previousMagic, move.User.Magic));
		if (move.Target is Enemy) {
			StartCoroutine( checkIfPlayerWon ());
		} else {
			checkIfPlayerLost ();
		}
	}

	/// <summary>
	/// Checks if the player has won\n
	/// If they have, exp is given and shown on screen, before saving player data, adding money and ending the battle
	/// </summary>
	/// <returns>Coroutine function to update exp bar</returns>
	private IEnumerator checkIfPlayerWon() {
		if (manager.battleWon()) {
			SoundManager.instance.playBGM(victory);
			enemySprite.gameObject.SetActive (false);
			attackButton.interactable = false;
			Debug.Log(enemy.ExpGiven);
			yield return StartCoroutine (updateExp(enemy.ExpGiven));
			playerArray [0] = player;
			PlayerData.instance.data.Players = playerArray;
			PlayerData.instance.data.Money += manager.money;
			Debug.Log ("Money: " + PlayerData.instance.data.Money);
			GlobalFunctions.instance.endBattle ();
		}
	}

	/// <summary>
	/// Updates the saved and displayed exp
	/// </summary>
	/// <returns>Coroutine function to update exp bar</returns>
	/// <param name="totalExp">The total exp the player has gained</param>
	private IEnumerator updateExp(int totalExp) {
		yield return new WaitForSeconds (1f);
		int gainedExp;
		int remainingExp = totalExp;
		while (player.Exp + remainingExp >= player.ExpToNextLevel) {
			gainedExp = player.ExpToNextLevel - player.Exp;
			remainingExp -= gainedExp;
			yield return StartCoroutine (updateExpHelper (gainedExp, true));
			expBar.setUpDisplay (0, player.ExpToNextLevel);
		}
		if (remainingExp > 0) {
			gainedExp = player.Exp + remainingExp;
			yield return StartCoroutine (updateExpHelper (gainedExp, false));
		}
	}

	/// <summary>
	/// A helper function for <see cref="updateExp"/>  that performs an exp increase within a single level interval.
	/// Updates text display if player levels up
	/// </summary>
	/// <returns>Coroutines to update exp display and add a delay before scene closes</returns>
	/// <param name="gainedExp">Exp gained within this level interval</param>
	/// <param name="levelledUp">If set to <c>true</c> indicates the player is about to level up.</param>
	private IEnumerator updateExpHelper(int gainedExp, bool levelledUp) {
		yield return StartCoroutine (expBar.updateDisplay (player.Exp, player.Exp + gainedExp));
		player.gainExp (gainedExp);
		if (levelledUp) {
			textBox.text = player.Name + " grew to level " + player.Level + "!";
			SoundManager.instance.playSFX ("interact");
		}
		yield return new WaitForSeconds (1.5f);
	}

	/// <summary>
	/// Checks if player lost.
	/// </summary>
	/// <returns><c>true</c>, if player has fainted, <c>false</c> otherwise.</returns>
	private bool checkIfPlayerLost() {
		if (manager.playerFainted()) {
			Debug.Log ("Lost!");
			return true;
		} else {
			return false;
		}
	}

	/// <summary>
	/// Setup a standard attack move
	/// </summary>
	public void standardAttack() {
		playerMove = new StandardAttack (manager, player, enemy);
		prepareTurn ();
	}

	public void special1() {
		player.Special1.setUp (manager, player, enemy);
		playerMove = player.Special1;
		prepareTurn ();
	}

	public void special2() {
		player.Special2.setUp (manager, player, enemy);
		playerMove = player.Special2;
		prepareTurn ();
	}

	private void prepareTurn() {
		enemyMove = manager.enemyMove (enemy, player);
		moveChosen = true;
		attacksPanel.SetActive (false);
		attackButton.interactable = false;
	}


    /// <summary>
	/// Run away from battle if <see cref="BattleManager.ranAway"/> returns true
    /// </summary>
	public void ranAway() {
        if (manager.ranAway(player.Speed,enemy.Speed)) {
			textBox.text = "You ran from the battle";
			attackButton.interactable = false;
			StartCoroutine (runAway ());
        } else {
			textBox.text = "You failed to run away";
			runButton.interactable = false;
        }
    }

	private IEnumerator runAway() {
		yield return new WaitForSeconds (2);
		GlobalFunctions.instance.endBattle ();
	}		

}
