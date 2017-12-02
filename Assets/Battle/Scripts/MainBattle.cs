using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainBattle : MonoBehaviour {

	//Objects
	private GameObject globalData;
	private BattleManager manager;
	private Player[] playerArray;
	private Button attackButton;
	private Text textBox;
	//Battle Manager References
	private Player player;
	private Enemy enemy;
	private int moneyReward;
	private Item itemReward;
	//Local Variables
	private bool battleWon;
	private bool battleLost;
	private string text;
	//Test Enemy
	private Enemy enemyObject;
	private bool playerFirst;
	//Moves
	private CharacterMove playerMove;
	private CharacterMove enemyMove;
	private bool moveChosen;
	//UI
	private GameObject playerStats;
	private GameObject enemyStats;
	private IDictionary<Character, StatsScript> healthBar;
	private IDictionary<Character, StatsScript> magicBar;
	private StatsScript playerHealthBar;
	private StatsScript enemyHealthBar;
	private StatsScript playerMagicBar;
	private StatsScript enemyMagicBar;
	private int playerPreviousHealth;
	private int enemyPreviousHealth;
	private int playerPreviousMagic;
	private int enemyPreviousMagic;
	//Generics
	private int previousHealth;
	private int previousMagic;
	//Scene Management
	private GameObject playerCamera;
	//Music
	private AudioClip BGM;


	// Use this for initialization
	void Start () {
		
		//Find Objects
		playerStats = GameObject.Find("PlayerStats");
		enemyStats = GameObject.Find ("EnemyStats");
//		playerHealthBar = playerStats.transform.Find("Health").GetComponent<StatsScript> ();
//		enemyHealthBar = enemyStats.transform.Find("Health").GetComponent<StatsScript> ();
//		playerMagicBar = playerStats.transform.Find ("Magic").GetComponent<StatsScript> ();
//		enemyMagicBar = enemyStats.transform.Find ("Magic").GetComponent<StatsScript> ();
		attackButton = GameObject.Find ("AttackButton").GetComponent<Button> ();
		textBox = GameObject.Find ("TextBox").transform.Find ("Text").GetComponent<Text> ();


		//Setup Object references
		playerArray = PlayerData.instance.playerArray;
		player = playerArray [0];
		enemyObject = GlobalFunctions.instance.getEnemy ();
		moneyReward = GlobalFunctions.instance.getMoney ();
		itemReward = GlobalFunctions.instance.getItem ();
		manager = new BattleManager (playerArray[0], enemyObject);
		player = manager.Player;
		enemy = manager.Enemy;
		playerFirst = manager.PlayerFirst;
		//enemyMove = new StandardAttack (manager, enemy, player, 10);

		//Bars
		healthBar = new Dictionary<Character, StatsScript>();
		healthBar[player] = playerStats.transform.Find("Health").GetComponent<StatsScript> ();
		healthBar[enemy] = enemyStats.transform.Find("Health").GetComponent<StatsScript> ();
		magicBar = new Dictionary<Character, StatsScript>();
		magicBar[player] = playerStats.transform.Find ("Magic").GetComponent<StatsScript> ();
		magicBar[enemy] = enemyStats.transform.Find ("Magic").GetComponent<StatsScript> ();

		healthBar[player].setUpDisplay (player.Health, 100);
		healthBar[enemy].setUpDisplay (enemy.Health, 100);
		magicBar[player].setUpDisplay (player.Magic, player.MaximumMagic);
		magicBar[enemy].setUpDisplay (enemy.Magic, enemy.MaximumMagic);

		//Setup local variables
		moveChosen = false;
		battleWon = false;
		battleLost = false;

		//Change Music
		BGM = Resources.Load("Audio/battle", typeof(AudioClip)) as AudioClip;
		SoundManager.instance.playBGM(BGM);

	}
	
	// Update is called once per frame
	void Update () {
		if (moveChosen) {
			if (playerFirst) {
				StartCoroutine (playerThenEnemy ());
			} else {
				StartCoroutine (enemyThenPlayer ());
			}
			moveChosen = false;
		}
	}		

	private IEnumerator playerThenEnemy () {
		yield return StartCoroutine (performTurn(playerMove));
		if (!battleWon) {
			yield return StartCoroutine (performTurn(enemyMove));
		}
		attackButton.interactable = true;
	}

	private IEnumerator enemyThenPlayer() {
		yield return StartCoroutine (performTurn(enemyMove));
		if (!battleLost) {
			yield return StartCoroutine (performTurn(playerMove));
		}
		attackButton.interactable = true;
	}

	private IEnumerator performTurn(CharacterMove move) {
		previousHealth = move.Target.Health;
		previousMagic = move.User.Magic;
		move.performMove ();
		textBox.text = move.User.Name + " " + move.Text + " " + move.Target.Name;
		if (manager.WasCriticalHit) {
			textBox.text += "\nCritical Hit!";
		}
		StartCoroutine (healthBar [move.Target].updateDisplay (previousHealth, move.Target.Health));
		yield return StartCoroutine (magicBar [move.User].updateDisplay (previousMagic, move.User.Magic));
		if (move.Target is Enemy) {
			checkIfPlayerWon ();
		} else {
			checkIfPlayerLost ();
		}
	}

	private void checkIfPlayerWon() {
		if (enemy.Health <= 0) {
			battleWon = true;
			player.gainExp (enemy.ExpGiven);
			playerArray [0] = player;
			PlayerData.instance.playerArray = playerArray;
			PlayerData.instance.money += moneyReward;
			Debug.Log ("Money: " + PlayerData.instance.money);
			GlobalFunctions.instance.endBattle ();
		}
	}

	private bool checkIfPlayerLost() {
		if (player.Health <= 0) {
			Debug.Log ("Lost!");
			return true;
		} else {
			return false;
		}
	}

	public void standardAttack() {
		playerMove = new StandardAttack (manager, player, enemy, 10);
		enemyMove = manager.enemyMove (enemy, player);
		moveChosen = true;
		attackButton.interactable = false;
	}

}
