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
				playersTurn (playerMove);
				enemysTurn (enemyMove);
			} else {
				enemysTurn (enemyMove);
				playersTurn (playerMove);
			}
			moveChosen = false;
		}
	}

	void initialSetup() {
		data = GameObject.Find ("PlayerData").GetComponent<PlayerDataScript> ();
		playerArray = data.playerArray;
		player = playerArray [0];
		enemyObject = new Enemy ("Test", 5, 100, 15, 5, 5, 5, 5);
		manager = new BattleManager (playerArray[0], enemyObject);
		player = manager.Player;
		enemy = manager.Enemy;
		playerFirst = manager.PlayerFirst;
		enemyMove = new StandardAttack (manager, enemy, player, 10);
	}

	public void playersTurn(CharacterMove playerMove) {
		playerMove.performMove ();
		Debug.Log ("Enemy Health: " + enemy.Health);
		checkIfPlayerWon ();
	}

	public void enemysTurn(CharacterMove enemyMove) {
		enemyMove.performMove ();
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
