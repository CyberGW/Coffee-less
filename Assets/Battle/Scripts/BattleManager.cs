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

}
