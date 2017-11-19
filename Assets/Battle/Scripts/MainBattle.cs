using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBattle : MonoBehaviour {

	private PlayerDataScript data;
	private Player[] playerArray;
	private BattleManager manager;
	private Player currentPlayer;
	private Enemy enemy;
	//Test Enemy
	private Enemy enemyObject;
	private bool playerFirst;

	// Use this for initialization
	void Start () {
		playerArray = new Player[6];
		//Let PlayerDataScript setup first before reading
		Invoke ("initialSetup", 0.1f);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			if (playerFirst) {
				//manager.attack (10, currentPlayer, enemy);
				checkIfPlayerWon ();
				//manager.attack (10, enemy, currentPlayer);
				checkIfPlayerLost ();
			} else {
				//manager.attack (10, enemy, currentPlayer);
				checkIfPlayerLost ();
				//manager.attack (10, currentPlayer, enemy);
				checkIfPlayerWon ();
			}
		}
	}

	void initialSetup() {
		data = GameObject.Find ("PlayerData").GetComponent<PlayerDataScript> ();
		playerArray = data.playerArray;
		enemyObject = new Enemy ("Test", 5, 100, 5, 5, 5, 5, 5);
		manager = new BattleManager (playerArray[0], enemyObject);
		currentPlayer = manager.Player;
		enemy = manager.Enemy;
		Debug.Log ("Player Health: " + currentPlayer.Health);
		playerFirst = manager.PlayerFirst;
		//Debug.Log (playerFirst);
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
		if (currentPlayer.Health <= 0) {
			Debug.Log ("Lost!");
			return true;
		} else {
			return false;
		}
	}
}
