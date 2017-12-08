using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleManager {

	private Player player;
	private Enemy enemy;
	public string forceCriticalHits; //Used for testing
	private bool wasCriticalHit;

	public BattleManager (Player player, Enemy enemy)
	{
		this.player = player;
		this.enemy = enemy;
		applyItem ();
		forceCriticalHits = "";
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

	public bool playerFirst() {
		if (player.Speed >= enemy.Speed) {
			return true;
		} else {
			return false;
		}
	}

	public void applyItem() {
		if (player.Item != null) {
			player.Item.applyBuffs(player);
		}
	}

	public void switchPlayers(Player newPlayer) {
		player = newPlayer;
	}

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
			float chance = 0.05f + (float) luck / 500;
			float random = Random.value;
			if (random < chance) {
				return true;
			} else {
				return false;
			}
		}
	}

	public bool ranAway(int playerSpeed, int enemySpeed) {
		float chance = 0.5f + (playerSpeed - enemySpeed) * 2;
		if (Random.value < chance) {
			return true;
		} else {
			return false;
		}
	}

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

	public CharacterMove enemyMove(Enemy enemy, Player player) {
		double chance = 0.7 - 0.7 * (enemy.MaximumMagic - enemy.Magic) / (double) enemy.MaximumMagic;
		if (Random.value < chance) { //try magic spell
			Debug.Log("Special Move");
			int random = Random.Range(0, 2);
			return enemySpecialMove (random);
		}
		//if special move not picked
		return new StandardAttack (this, enemy, player, 10);
	}

	public CharacterMove enemySpecialMove(float random) {
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
			return new StandardAttack (this, enemy, player, 10);
		}
	}

	private bool characterFainted(Character character) {
		if (character.Health <= 0) {
			return true;
		} else {
			return false;
		}
	}

	public bool battleWon() {
		return characterFainted (enemy);
	}

	public bool playerFainted() {
		return characterFainted (player);
	}


}