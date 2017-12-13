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
	private string text;
	//Test Enemy
	private Enemy enemyObject;
	//Moves
	private CharacterMove playerMove;
	private CharacterMove enemyMove;
	private bool moveChosen;
	//UI
	private GameObject playerStats;
	private GameObject enemyStats;
	private IDictionary<Character, StatsScript> healthBar;
	private IDictionary<Character, StatsScript> magicBar;
	private StatsScript expBar;
	//Scene Management
	private GameObject playerCamera;
	//Music
	private AudioClip BGM;
	private AudioClip victory;


	// Use this for initialization
	void Start () {
		
		//Find Objects
		playerStats = GameObject.Find("PlayerStats");
		enemyStats = GameObject.Find ("EnemyStats");
		attackButton = GameObject.Find ("AttackButton").GetComponent<Button> ();
		textBox = GameObject.Find ("TextBox").transform.Find ("Text").GetComponent<Text> ();


		//Setup Object references
		playerArray = PlayerData.instance.data.Players;
		player = playerArray [0];
		enemyObject = GlobalFunctions.instance.getEnemy ();
		moneyReward = GlobalFunctions.instance.getMoney ();
		itemReward = GlobalFunctions.instance.getItem ();
		manager = new BattleManager (playerArray[0], enemyObject);
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
	
	// Update is called once per frame
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

	private IEnumerator playerThenEnemy () {
		yield return StartCoroutine (performTurn(playerMove));
		if (!manager.battleWon()) {
			yield return StartCoroutine (performTurn(enemyMove));
		}
		attackButton.interactable = true;
	}

	private IEnumerator enemyThenPlayer() {
		yield return StartCoroutine (performTurn(enemyMove));
		if (!manager.playerFainted()) {
			yield return StartCoroutine (performTurn(playerMove));
		}
		attackButton.interactable = true;
	}

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

	private IEnumerator checkIfPlayerWon() {
		if (manager.battleWon()) {
			SoundManager.instance.playBGM(victory);
			Debug.Log(enemy.ExpGiven);
			yield return StartCoroutine (updateExp(enemy.ExpGiven));
			playerArray [0] = player;
			PlayerData.instance.data.Players = playerArray;
			PlayerData.instance.data.Money += moneyReward;
			Debug.Log ("Money: " + PlayerData.instance.data.Money);
			GlobalFunctions.instance.endBattle ();
		}
	}

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

	private IEnumerator updateExpHelper(int gainedExp, bool levelledUp) {
		yield return StartCoroutine (expBar.updateDisplay (player.Exp, player.Exp + gainedExp));
		player.gainExp (gainedExp);
		if (levelledUp) {
			textBox.text = player.Name + " grew to level " + player.Level + "!";
			SoundManager.instance.playSFX ("interact");
		}
		yield return new WaitForSeconds (1.5f);
	}

	private bool checkIfPlayerLost() {
		if (manager.playerFainted()) {
			Debug.Log ("Lost!");
			return true;
		} else {
			return false;
		}
	}

	public void standardAttack() {
		playerMove = new StandardAttack (manager, player, enemy);
		enemyMove = manager.enemyMove (enemy, player);
		moveChosen = true;
		attackButton.interactable = false;
	}

    public void runAway()
    {
        if (manager.ranAway(player.Speed,enemy.Speed))
        {
            GlobalFunctions.instance.endBattle();
            Debug.Log("success!!");
        }
        else
        {
            Debug.Log("run attempt failed!!");
        }
        }
    }