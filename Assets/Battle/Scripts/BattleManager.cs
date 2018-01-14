using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds current <see cref="Player"/> and <see cref="Enemy"/> objects in battle and provides functions for
/// <see cref="MainBattle"/> and other objects to utilise   
/// </summary>
public class BattleManager {

	private Player player;
	private Enemy enemy;
	/// <summary>
	/// Can be set to "All" or "None" to make every attack or not attack a critical hit, for testing purposes
	/// Any other value will enable them as per usual</summary>
	public string forceCriticalHits = ""; //Used for testing
	/// <summary>
	/// Can be set to StandardAttack, Special1 or Special2 to choose the enemy's move for testing purposes
	/// Any other value will use the AI as per usual</summary>
	public string forceEnemyMove = ""; //Used for testing
	private bool wasCriticalHit;
	public int money;

	public BattleManager (Player player, Enemy enemy, int money)
	{
		this.player = player;
		this.enemy = enemy;
		this.money = money;
	}

	public Player Player {
		get {
			return this.player;
		}
		set {
			player = value;
		}
	}

	public Enemy Enemy {
		get {
			return this.enemy;
		}
		set {
			enemy = value;
		}
	}

	public bool WasCriticalHit {
		get {
			return this.wasCriticalHit;
		}
	}

	/// <summary>
	/// Determines whether the player should go first or not by comparing their speed stat to the enemy's
	/// </summary>
	/// <returns><c>true</c>, if player should go first, <c>false</c> otherwise.</returns>
	public bool playerFirst() {
		if (player.Speed >= enemy.Speed) {
			return true;
		} else {
			return false;
		}
	}

	/// <summary>
	/// Switches two players, called by a <see cref="CharacterMove"/> 
	/// </summary>
	/// <param name="newPlayer">The new player the current one should be switched with</param>
	public void switchPlayers(Player newPlayer) {
		Debug.Log (newPlayer.Name);
		this.player = newPlayer;
		Debug.Log (this.player.Name);
	}

	/// <summary>
	/// Determines if a move is a critical hit, based upon luck stat and random element.
	/// Can be forced by setting <see cref="forceCriticalHits"/> to All or None 
	/// </summary>
	/// <returns><c>true</c>, if critical hit, <c>false</c> otherwise.</returns>
	/// <param name="luck">Luck.</param>
	public bool isCriticalHit(int luck) {
		//Conditions for testing
		switch (forceCriticalHits)
		{
		case "All":
			return true;
		case "None":
			return false;
		default:
			//Usual code block
			float chance = 0.05f + (float) luck / 200;
			float random = Random.value;
			if (random < chance) {
				return true;
			} else {
				return false;
			}
		}
	}

	/// <summary>
	/// Generate whether the player has run away or not, dependent upon speed stats and random element
	/// </summary>
	/// <returns><c>true</c>, if away is to run away, <c>false</c> otherwise.</returns>
	/// <param name="playerSpeed">Player speed stat.</param>
	/// <param name="enemySpeed">Enemy speed stat.</param>
	public bool ranAway(int playerSpeed, int enemySpeed) {
		float chance = 0.5f + (playerSpeed - enemySpeed) * 2;
		if (Random.value < chance) {
			return true;
		} else {
			return false;
		}
	}

	/// <summary>
	/// Calculates the amount of damage and attack should inflict, ensuring not to take health to negative values
	/// </summary>
	/// <returns>The amount of damage inflicted</returns>
	/// <param name="user">The user of the attack</param>
	/// <param name="target">The target of the attack</param>
	/// <param name="power">The power of the attack.</param>
	public int damageCalculation(Character user, Character target, int power) {
		float fDamage = (float) user.Attack * power / target.Defence;
		if (isCriticalHit (user.Luck)) {
			fDamage *= 1.75f; //If critical hit increase damage by 75%
			wasCriticalHit = true;
		} else {
			wasCriticalHit = false;
		}
		int damage = Mathf.RoundToInt (fDamage);
		if (damage < target.Health) { //If not going to kill target
			return damage;
		} else {
			return target.Health; //If going to kill, return health left so it doesn't go below zero
		}
	}

	/// <summary>
	/// Decides upon whether to pick a special or standard attack move for the enemy
	/// Can be forced for testing by setting <see cref="forceEnemyMove"/> 
	/// </summary>
	/// <returns>The move for the enemy to perform</returns>
	/// <param name="enemy">The enemy object to generate a move for</param>
	/// <param name="player">The player object who is target of the move</param>
	public CharacterMove enemyMove(Enemy enemy, Player player) {
		switch (forceEnemyMove) {
		case "StandardAttack":
			return new StandardAttack (this, enemy, player);
		case "Special1":
			enemy.Special1.setUp (this, enemy, player);
			return enemy.Special1;
		case "Special2":
			enemy.Special2.setUp (this, enemy, player);
			return enemy.Special2;
		default: 
			double chance = 0.7 - 0.7 * (enemy.MaximumMagic - enemy.Magic) / (double)enemy.MaximumMagic;
			if (Random.value < chance) { //try magic spell
				Debug.Log ("Special Move");
				int random = Random.Range (0, 2);
				return enemySpecialMove (random);
			}
			//if special move not picked
			return new StandardAttack (this, enemy, player);
		}
	}

	/// <summary>
	/// Called by <see cref="enemyMove"/> to determine which special move to pick, resorting back to a
	/// standard attack if not enough magic 
	/// </summary>
	/// <returns>The special move to use</returns>
	/// <param name="random">Random number 0 or 1 to choose <see cref="Enemy.Special1"/> or <see cref="Enemy.Special2"/> </param>  
	public CharacterMove enemySpecialMove(int random) {
		//Setup moves
		enemy.Special1.setUp (this, enemy, player);
		enemy.Special2.setUp (this, enemy, player);
		if (random == 0) { //if special 1 has been randomly picked
			if (enemy.Special1.Magic <= enemy.Magic) {
				return enemy.Special1;
			}
		}
		//if special 2 randomly picked OR not enough magic for special 1
		if (enemy.Special2.Magic <= enemy.Magic) {
			return enemy.Special2;
		} else { //resort back to standard attack if not enough damage for either
			return new StandardAttack (this, enemy, player);
		}
	}

	/// <summary>
	/// Characters the fainted.
	/// </summary>
	/// <returns><c>true</c>, if fainted was charactered, <c>false</c> otherwise.</returns>
	/// <param name="character">Character.</param>
	private bool characterFainted(Character character) {
		if (character.Health <= 0) {
			return true;
		} else {
			return false;
		}
	}

	/// <summary>
	/// Checks if battle has been won, dependent upon <see cref="characterFainted"/> 
	/// </summary>
	/// <returns><c>true</c>, if battle has been won, <c>false</c> otherwise.</returns>
	public bool battleWon() {
		return characterFainted (enemy);
	}

	/// <summary>
	/// Checks if player current player fainted, dependent upon <see cref="characterFainted"/> 
	/// </summary>
	/// <returns><c>true</c>, if current player has fainted, <c>false</c> otherwise.</returns>
	public bool playerFainted() {
		return characterFainted (player);
	}

}
