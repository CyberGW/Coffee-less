using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBattle : MonoBehaviour {

	private PlayerDataScript data;
	private Player[] playerArray;
	private BattleManager manager;
	private Player player;
	private Enemy enemy;
	//Test Enemy
	private Enemy enemyObject;
	private bool playerFirst;
	//Moves
	private CharacterMove playerMove;
	private CharacterMove enemyMove;
	private bool moveChosen;
	//UI
	private StatsScript playerHealthBar;
	private StatsScript enemyHealthBar;
	private int playerPreviousHealth;
	private int enemyPreviousHealth;


	// Use this for initialization
	void Start () {
		moveChosen = false;
		//Let PlayerDataScript setup first before reading
		Invoke ("initialSetup", 0.1f);
	}
	
	// Update is called once per frame
	void Update () {
		if (moveChosen) {
			if (playerFirst) {
				StartCoroutine (playerThenEnemy ());
			} else {
				enemysTurn (enemyMove);
				playersTurn (playerMove);
			}
			moveChosen = false;
		}
	}

	public IEnumerator playerThenEnemy () {
		yield return StartCoroutine (playersTurn (playerMove));
		yield return StartCoroutine (enemysTurn (enemyMove));
	}

	void initialSetup() {
		data = GameObject.Find ("PlayerData").GetComponent<PlayerDataScript> ();
		playerHealthBar = GameObject.Find ("PlayerStats").GetComponent<StatsScript> ();
		enemyHealthBar = GameObject.Find ("EnemyStats").GetComponent<StatsScript> ();
		playerArray = data.playerArray;
		player = playerArray [0];
		enemyObject = new Enemy ("Test", 5, 100, 15, 5, 5, 5, 5);
		manager = new BattleManager (playerArray[0], enemyObject);
		player = manager.Player;
		enemy = manager.Enemy;
		playerFirst = manager.PlayerFirst;
		enemyMove = new StandardAttack (manager, enemy, player, 10);
	}

	public IEnumerator playersTurn(CharacterMove playerMove) {
		enemyPreviousHealth = enemy.Health;
		playerMove.performMove ();
		yield return StartCoroutine ( enemyHealthBar.updatePlayerHealth (enemyPreviousHealth, enemy.Health) );
		Debug.Log ("Enemy Health: " + enemy.Health);
		checkIfPlayerWon ();
	}

	public IEnumerator enemysTurn(CharacterMove enemyMove) {
		playerPreviousHealth = player.Health;
		enemyMove.performMove ();
		yield return StartCoroutine ( playerHealthBar.updatePlayerHealth (playerPreviousHealth, player.Health) );
		Debug.Log ("Player Health: " + player.Health);
		checkIfPlayerLost ();
	}

	public bool checkIfPlayerWon() {
		if (enemy.Health <= 0) {
			Debug.Log ("Won!");
			return true;
		} else {
			return false;
		}
	}

	public bool checkIfPlayerLost() {
		if (player.Health <= 0) {
			Debug.Log ("Lost!");
			return true;
		} else {
			return false;
		}
	}

	public void attackButton() {
		playerMove = new StandardAttack (manager, player, enemy, 10);
		moveChosen = true;
	}

}
