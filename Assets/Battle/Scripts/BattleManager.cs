using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleManager {

	private Fighter player;
	private Fighter enemy;
	private Fighter[] turnOrder;

	public BattleManager (Fighter player, Fighter enemy)
	{
		this.player = player;
		this.enemy = enemy;
		calculateTurnOrder ();
	}

	public Fighter Player {
		get {
			return this.player;
		}
		set {
			player = value;
			calculateTurnOrder();
		}
	}

	public Fighter Enemy {
		get {
			return this.enemy;
		}
		set {
			enemy = value;
			calculateTurnOrder();
		}
	}

	public Fighter[] TurnOrder {
		get {
			return this.turnOrder;
		}
		set {
			turnOrder = value;
		}
	}

	public void calculateTurnOrder() {
		int playerSpeed = player.Speed;
		int enemySpeed = enemy.Speed;
		Fighter[] turnOrder;
		if (playerSpeed >= enemySpeed) {
			this.turnOrder = new Fighter[] {player, enemy};
		} else {
			this.turnOrder = new Fighter[] {enemy, player};
		}
	}

}
