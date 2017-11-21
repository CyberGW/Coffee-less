using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleManager {

	private Player[] playerArray;
	private Player player;
	private Enemy enemy;
	private bool playerFirst;

	public BattleManager (Player player, Enemy enemy)
	{
		this.player = player;
		this.enemy = enemy;
		this.playerArray = playerArray;
		applyItem ();
		calculatePlayerFirst ();
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

	public bool PlayerFirst {
		get {
			return this.playerFirst;
		}
	}

	public void calculatePlayerFirst() {
		if (player.Speed >= enemy.Speed) {
			playerFirst = true;
		} else {
			playerFirst = false;
		}
	}

	public void applyItem() {
		if (Player.Item != null) {
			Player.Item.applyBuffs();
		}
	}

	public void switchPlayers(Player newPlayer) {
		Player = newPlayer;
		calculatePlayerFirst ();
	}

	public bool isCriticalHit(Character user) {
		float chance = 0.05f + (float) user.Luck / 1000;
		float random = Random.value;
		if (random < chance) {
			return true;
		} else {
			return false;
		}
	}


}
