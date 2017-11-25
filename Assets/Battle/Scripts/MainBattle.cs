using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBattle : MonoBehaviour {

	//Objects
	private GameObject globalData;
	private PlayerDataScript playerData;
	private GlobalVariables globalVariables;
	private Player[] playerArray;
	private BattleManager manager;
	private Player player;
	private Enemy enemy;
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
	private StatsScript playerHealthBar;
	private StatsScript enemyHealthBar;
	private int playerPreviousHealth;
	private int enemyPreviousHealth;
	//Scene Management
	private SceneChanger sceneChanger;
	private GameObject moveablePlayer;
	private GameObject playerCamera;
	//Music
	private AudioClip BGM;


	// Use this for initialization
	void Start () {
		//Deactive Player
		moveablePlayer = GameObject.Find ("Player").gameObject;
		moveablePlayer.SetActive (false);

		//Find Objects
		globalData = GameObject.Find ("GlobalData");
		playerData = globalData.GetComponent<PlayerDataScript> ();
		globalVariables = globalData.GetComponent<GlobalVariables> ();
		playerHealthBar = GameObject.Find ("PlayerStats").GetComponent<StatsScript> ();
		enemyHealthBar = GameObject.Find ("EnemyStats").GetComponent<StatsScript> ();
		sceneChanger = GameObject.Find("SceneChanger").GetComponent<SceneChanger> ();

		//Setup Object references
		playerArray = playerData.playerArray;
		player = playerArray [0];
		enemyObject = globalVariables.battleEnemy;
		manager = new BattleManager (playerArray[0], enemyObject);
		player = manager.Player;
		enemy = manager.Enemy;
		playerFirst = manager.PlayerFirst;
		enemyMove = new StandardAttack (manager, enemy, player, 10);
		playerHealthBar.setUpDisplay (player.Health);
		enemyHealthBar.setUpDisplay (enemy.Health);

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
	}

	private IEnumerator enemyThenPlayer() {
		yield return StartCoroutine (enemysTurn (enemyMove));
		if (!battleLost) {
			yield return StartCoroutine (playersTurn (playerMove));
		}
	}

	private IEnumerator playersTurn(CharacterMove playerMove) {
		enemyPreviousHealth = enemy.Health;
		playerMove.performMove ();
		yield return StartCoroutine ( enemyHealthBar.updatePlayerHealth (enemyPreviousHealth, enemy.Health) );
		Debug.Log ("Enemy Health: " + enemy.Health);
		checkIfPlayerWon ();
	}

	private IEnumerator enemysTurn(CharacterMove enemyMove) {
		playerPreviousHealth = player.Health;
		enemyMove.performMove ();
		yield return StartCoroutine ( playerHealthBar.updatePlayerHealth (playerPreviousHealth, player.Health) );
		Debug.Log ("Player Health: " + player.Health);
		checkIfPlayerLost ();
	}

	private void checkIfPlayerWon() {
		if (enemy.Health <= 0) {
			battleWon = true;
			playerArray [0] = player;
			playerData.playerArray = playerArray;
			moveablePlayer.SetActive (true);
			SoundManager.instance.playBGM (globalVariables.battleMusicToReturnTo);
			sceneChanger.loadLevel (globalVariables.battleSceneToReturnTo);
			Initiate.Fade ("Main", Color.black, 3f);
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

	private void attackButton() {
		playerMove = new StandardAttack (manager, player, enemy, 10);
		moveChosen = true;
	}

}
