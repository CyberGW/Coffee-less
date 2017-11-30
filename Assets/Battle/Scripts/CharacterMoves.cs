using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CharacterMove {
	void performMove (); 
}

public interface SpecialMove : CharacterMove {
	int Magic {
		get;
	}
	void setUp(BattleManager manager, Character user, Character target);
	void decreaseMagic();
}



[System.Serializable]
public class StandardAttack : CharacterMove {

	private BattleManager manager;
	private Character user;
	private Character target;
	private int power;

	public StandardAttack (BattleManager manager, Character user, Character target, int power)
	{
		this.manager = manager;
		this.user = user;
		this.target = target;
		this.power = power;
	}
		
	public void performMove () {
		int damage = manager.damageCalculation (user, target, power);
		target.Health -= damage;
	}
}

[System.Serializable]
public class SwitchPlayers : CharacterMove {

	private Player currentPlayer;
	private Player newPlayer;
	private BattleManager manager;

	public SwitchPlayers (Player currentPlayer, Player newPlayer, BattleManager manager)
	{
		this.currentPlayer = currentPlayer;
		this.newPlayer = newPlayer;
		this.manager = manager;
	}

	public void performMove () {
		manager.switchPlayers (newPlayer);
	}
}

[System.Serializable]
public class HealingSpell : CharacterMove {

	private Character user;
	private Character target;
	private int healthRestore;

	public HealingSpell (Character user, Character target, int healthRestore)
	{
		this.user = user;
		this.target = target;
		this.healthRestore = healthRestore;
	}

	public void performMove() {
		if ((100 - target.Health) > healthRestore) {
			target.Health += healthRestore;
		} else {
			target.Health = 100;
		}
	}
}

[System.Serializable]
public class Fireball : SpecialMove {

	private string name;
	private string desc;
	private int magic;
	private Character user;
	private Character target;

	public Fireball (string name, string desc, int magic) {
		this.name = name;
		this.desc = desc;
		this.magic = magic;
	}

	public int Magic {
		get {
			return this.magic;
		}
	}

	public void setUp(BattleManager manager, Character user, Character target) {
		this.target = target;
		this.user = user;
	}

	public void performMove() {
		target.Health -= 40;
		decreaseMagic ();
	}

	public void decreaseMagic() {
		user.Magic -= magic;
	}

}