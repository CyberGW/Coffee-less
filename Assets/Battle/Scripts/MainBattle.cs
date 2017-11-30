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
	//Battle Manager References
	private Player player;
	private Enemy enemy;
	private int moneyReward;
	private Item itemReward;
	//Local Variables
	private bool battleWon;
	private bool battleLost;
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
	private StatsScript playerHealthBar;
	private StatsScript enemyHealthBar;
	private StatsScript playerMagicBar;
	private StatsScript enemyMagicBar;
	private int playerPreviousHealth;
	private int enemyPreviousHealth;
	//Scene Management
	private GameObject playerCamera;
	//Music
	private AudioClip BGM;


	// Use this for initialization
	void Start () {
		
		//Find Objects
		playerStats = GameObject.Find("PlayerStats");
		enemyStats = GameObject.Find ("EnemyStats");
		playerHealthBar = playerStats.transform.Find("Health").GetComponent<StatsScript> ();
		enemyHealthBar = enemyStats.transform.Find("Health").GetComponent<StatsScript> ();
		playerMagicBar = playerStats.transform.Find ("Magic").GetComponent<StatsScript> ();
		attackButton = GameObject.Find ("AttackButton").GetComponent<Button> ();

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
		enemyMove = new StandardAttack (manager, enemy, player, 10);
		playerHealthBar.setUpDisplay (player.Health, 100);
		enemyHealthBar.setUpDisplay (enemy.Health, 100);
		playerMagicBar.setUpDisplay (player.Magic, player.MaximumMagic);

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
		yield return StartCoroutine (playersTurn (playerMove));
		if (!battleWon) {
			yield return StartCoroutine (enemysTurn (enemyMove));
		}
		attackButton.interactable = true;
	}

	private IEnumerator enemyThenPlayer() {
		yield return StartCoroutine (enemysTurn (enemyMove));
		if (!battleLost) {
			yield return StartCoroutine (playersTurn (playerMove));
		}
		attackButton.interactable = true;
	}

	private IEnumerator playersTurn(CharacterMove playerMove) {
		enemyPreviousHealth = enemy.Health;
		playerMove.performMove ();
		yield return StartCoroutine ( enemyHealthBar.updateDisplay (enemyPreviousHealth, enemy.Health) );
		Debug.Log ("Enemy Health: " + enemy.Health);
		checkIfPlayerWon ();
	}

	private IEnumerator enemysTurn(CharacterMove enemyMove) {
		playerPreviousHealth = player.Health;
		enemyMove.performMove ();
		yield return StartCoroutine ( playerHealthBar.updateDisplay (playerPreviousHealth, player.Health) );
		Debug.Log ("Player Health: " + player.Health);
		checkIfPlayerLost ();
	}

	private void checkIfPlayerWon() {
		if (enemy.Health <= 0) {
			battleWon = true;
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
		moveChosen = true;
		attackButton.interactable = false;
	}

}
