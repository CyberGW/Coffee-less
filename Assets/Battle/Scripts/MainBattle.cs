using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Script for handing a battle scene. Uses <see cref="BattleManager"/>, but handles turn order and execution as well
/// as updating the display appropriately </summary>
public class MainBattle : MonoBehaviour {

	//Objects
	private GameObject globalData;
	public BattleManager manager;
	private Player[] playerArray;
	private Button attackButton;
	private Button playerButton;
	private Button runButton;
	private Text textBox;
	//Battle Manager References
	/// <summary>
	/// Set to public
	/// </summary>
	public Player player;
	private Enemy enemy;
	private int moneyReward;
	private Item itemReward;
	//Local Variables
	private string text;
	public bool playerDied;
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
	//private IDictionary<Character, StatsScript> healthBar;
	//private IDictionary<Character, StatsScript> magicBar;
	private StatsScript playerHealthBar;
	private StatsScript playerMagicBar;
	private StatsScript enemyHealthBar;
	private StatsScript enemyMagicBar;
	private StatsScript expBar;
	private Image playerSprite;
	private Image enemySprite;
	//Scene Management
	private GameObject playerCamera;
	//Music
	private AudioClip BGM;
	private AudioClip victory;


	/// <summary>
	/// Includes finding game objects, setting references and changing background music
	/// </summary>
	void Start () {
		
		//Find Objects
		attacksPanel = GameObject.Find("BattleCanvas").transform.Find("AttacksPanel").gameObject;
		playerStats = GameObject.Find("PlayerStats");
		enemyStats = GameObject.Find ("EnemyStats");
		attackButton = GameObject.Find ("AttackButton").GetComponent<Button> ();
		playerButton = GameObject.Find ("PlayersButton").GetComponent<Button> ();
		runButton = GameObject.Find ("RunButton").GetComponent<Button> ();
		setButtonsInteractable (true);
		textBox = GameObject.Find ("TextBox").transform.Find ("Text").GetComponent<Text> ();
		playerSprite = GameObject.Find ("PlayerImage").GetComponent<Image> ();
		enemySprite = GameObject.Find ("EnemyImage").GetComponent<Image> ();


		//Setup Object references
		playerArray = PlayerData.instance.data.Players;
		enemyObject = GlobalFunctions.instance.getEnemy ();
		moneyReward = GlobalFunctions.instance.getMoney ();
		itemReward = GlobalFunctions.instance.getItem ();
		manager = new BattleManager (playerArray[0], enemyObject, moneyReward);
		player = manager.Player;
		enemy = manager.Enemy;
		Texture2D image;
		image = enemy.Image;
		enemySprite.sprite = Sprite.Create (image, new Rect (0.0f, 0.0f, image.width, image.height), new Vector2 (0.5f, 0.5f));
		image = player.Image;
		playerSprite.sprite = Sprite.Create (image, new Rect (0.0f, 0.0f, image.width, image.height), new Vector2 (0.5f, 0.5f));


		expBar = playerStats.transform.Find ("Exp").GetComponent<StatsScript> ();
		playerHealthBar = playerStats.transform.Find("Health").GetComponent<StatsScript> ();
		playerMagicBar = playerStats.transform.Find ("Magic").GetComponent<StatsScript> ();
		enemyHealthBar = enemyStats.transform.Find("Health").GetComponent<StatsScript> ();
		enemyMagicBar = enemyStats.transform.Find ("Magic").GetComponent<StatsScript> ();
		expBar.setUpDisplay (player.Exp, player.ExpToNextLevel);
		playerHealthBar.setUpDisplay (player.Health, 100);
		enemyHealthBar.setUpDisplay (enemy.Health, 100);
		playerMagicBar.setUpDisplay (player.Magic, player.MaximumMagic);
		enemyMagicBar.setUpDisplay (enemy.Magic, enemy.MaximumMagic);
		//Setup local variables
		moveChosen = false;
		playerDied = false;

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
			Debug.Log (player.Name);
			enemyMove = manager.enemyMove (enemy, player);
			yield return StartCoroutine (performTurn(enemyMove));
			setButtonsInteractable (true);
		}
	}

	/// <summary>
	/// Performs the enemy's turn, then the player's\n
	/// Re-enables attack button afterwards
	/// </summary>
	/// <returns>Coroutine functions to perform the turns</returns>
	private IEnumerator enemyThenPlayer() {
		enemyMove = manager.enemyMove (enemy, player);
		yield return StartCoroutine (performTurn(enemyMove));
		if (!manager.playerFainted()) {
			yield return StartCoroutine (performTurn(playerMove));
			setButtonsInteractable (true);
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
		if (manager.WasCriticalHit && (move is StandardAttack || move is MagicAttack)) {
			textBox.text += "\nCritical Hit!";
		}

		//Update bars and data
		if (move is SwitchPlayers) {
			updateToNewPlayer ();
		}
		yield return updateBars(move, previousHealth, previousMagic);
		if (move.Target is Enemy) {
			StartCoroutine( checkIfPlayerWon ());
		} else {
			StartCoroutine( checkIfPlayerLost ());
		}
	}

	private IEnumerator updateBars(CharacterMove move, int previousHealth, int previousMagic) {
		if (move.User is Player) {
			StartCoroutine(playerMagicBar.updateDisplay (previousMagic, move.User.Magic));
		} else { //if user is Enemy
			StartCoroutine(enemyMagicBar.updateDisplay (previousMagic, move.User.Magic));
		}
		if (move.Target is Player) {
			yield return playerHealthBar.updateDisplay (previousHealth, move.Target.Health);
		} else { //if target is Enemy
			yield return enemyHealthBar.updateDisplay( previousHealth, move.Target.Health);
		}
	}

	/// <summary>
	/// Checks if the player has won\n
	/// If they have, exp is given and shown on screen, before saving player data, adding money, adding the item and ending the battle
	/// </summary>
	/// <returns>Coroutine function to update exp bar</returns>
	private IEnumerator checkIfPlayerWon() {
		if (manager.battleWon()) {
			SoundManager.instance.playBGM(victory);
			enemySprite.gameObject.SetActive (false);
			//Wait a frame before changing button states
			yield return null;
			setButtonsInteractable (false);
			Debug.Log(enemy.ExpGiven);
			yield return StartCoroutine (updateExp(enemy.ExpGiven));
			playerArray [0] = player;
			PlayerData.instance.data.Players = playerArray;
			PlayerData.instance.data.Money += manager.money;
			if (itemReward != null) {
				PlayerData.instance.data.addItem (itemReward);
			}
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
		while ( (player.Exp + remainingExp) >= player.ExpToNextLevel) {
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
	/// Checks if player lost. If so and no player's left return to main menu, reactivating the player so the menu script can find it.
	/// If so and player's left, open switch player menu. Otherwise pass.
	/// </summary>
	private IEnumerator checkIfPlayerLost() {
		if (manager.playerFainted()) {
			yield return null;
			setButtonsInteractable (false);
			if (PlayerData.instance.data.playersAlive() == 0) {
				textBox.text = "All players have fainted! Game Over.";
				yield return new WaitForSeconds (2);
				GlobalFunctions.instance.player.SetActive (true); //Make player active so it can be found again in main menu
				SoundManager.instance.playBGM(GlobalFunctions.instance.previousBGM);
				SceneChanger.instance.loadLevel ("mainmenu1");
			} else {
				playerDied = true;
				textBox.text = player.Name + " fainted!";
				yield return new WaitForSeconds (3);
				SceneManager.LoadSceneAsync ("SwitchPlayer", LoadSceneMode.Additive);
			}
		}
	}

	private void setButtonsInteractable(bool val) {
		attackButton.interactable = val;
		playerButton.interactable = val;
		//So both val and canRunAway need to be true to activate run button
		runButton.interactable = val && GlobalFunctions.instance.canRunAway;
	}

	/// <summary>
	/// Setup a standard attack move for the player
	/// </summary>
	public void standardAttack() {
		playerMove = new StandardAttack (manager, player, enemy);
		prepareTurn ();
	}

	/// <summary>
	/// Setup the first special move for the player
	/// </summary>
	public void special1() {
		player.Special1.setUp (manager, player, enemy);
		playerMove = player.Special1;
		prepareTurn ();
	}

	/// <summary>
	/// Setup the second special move for the player
	/// </summary>
	public void special2() {
		player.Special2.setUp (manager, player, enemy);
		playerMove = player.Special2;
		prepareTurn ();
	}

	/// <summary>
	/// Switchs the players.
	/// </summary>
	/// <param name="playerIndex">Index of new player in <see cref="DataManager.players"/> array </param>
	public void switchPlayers(int playerIndex) {
		Player newPlayer = PlayerData.instance.data.Players [playerIndex];
		playerMove = new SwitchPlayers (manager, player, newPlayer);
		PlayerData.instance.data.swapPlayers (0, playerIndex);
		playerSprite.sprite = Sprite.Create (newPlayer.Image, new Rect (0.0f, 0.0f, newPlayer.Image.width, newPlayer.Image.height),
			new Vector2 (0.5f, 0.5f));

		prepareTurn();
	}

	/// <summary>
	/// Updates references and re-setup bars to this next player
	/// </summary>
	public void updateToNewPlayer() {
		this.player = manager.Player;
		//Update references
		playerHealthBar.setUpDisplay(player.Health, 100);
		playerMagicBar.setUpDisplay (player.Magic, player.MaximumMagic);
		expBar.setUpDisplay (player.Exp, player.ExpToNextLevel);
		
	}

	/// <summary>
	/// Called by any of <see cref="standardAttack"/>, <see cref="special1"/> or <see cref="special2"/>
	/// Uses <see cref="BattleManager.enemyMove"/> to decide on the enemy's move, and hides attack panel and makes
	/// attack button unclickable    
	/// </summary>
	private void prepareTurn() {
		if (!playerDied) {
			moveChosen = true;
			attacksPanel.SetActive (false);
			setButtonsInteractable (false);
		} else {
			//Perform the switch character move
			StartCoroutine(performTurn(playerMove));
			playerDied = false;
			setButtonsInteractable (true);
		}
	}


    /// <summary>
	/// Run away from battle if <see cref="BattleManager.ranAway"/> returns true.
	/// Otherwise display appropriate text and make sure run cannot be selected again that move
    /// </summary>
	public void ranAway() {
        if (manager.ranAway(player.Speed,enemy.Speed)) {
			StartCoroutine (runAway ());
        } else {
			textBox.text = "You failed to run away";
			runButton.interactable = false;
        }
    }

	/// <summary>
	/// If <see cref="ranAway"/> return true, then  display message, disable attack button and after 2 seconds
	/// call <see cref="GlobalFunctions.endBattle"/> 
	/// </summary>
	/// <returns>The away.</returns>
	private IEnumerator runAway() {
		textBox.text = "You ran from the battle";
		attackButton.interactable = false;
		yield return new WaitForSeconds (2);
		GlobalFunctions.instance.endBattle ();
	}		

}
