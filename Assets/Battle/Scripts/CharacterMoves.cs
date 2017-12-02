using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CharacterMove {

	Character User {
		get;
	}

	Character Target {
		get;
	}

	string Text {
		get;
	}

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
	private string text;

	public StandardAttack (BattleManager manager, Character user, Character target, int power)
	{
		this.manager = manager;
		this.user = user;
		this.target = target;
		this.power = power;
		this.text = "attacked";
	}

	public string Text {
		get {
			return this.text;
		}
	}

	public Character User {
		get {
			return this.user;
		}
	}

	public Character Target {
		get {
			return this.target;
		}
	}
		
	public void performMove () {
		int damage = manager.damageCalculation (user, target, power);
		target.Health -= damage;
	}
}

[System.Serializable]
public class SwitchPlayers : CharacterMove {

	private Player user;
	private Player target;
	private BattleManager manager;
	private string text;

	public SwitchPlayers (Player currentPlayer, Player newPlayer, BattleManager manager)
	{
		this.user = currentPlayer;
		this.target = newPlayer;
		this.manager = manager;
		this.text = "switched with";
	}

	public string Text {
		get {
			return this.text;
		}
	}

	public Character User {
		get {
			return this.user;
		}
	}

	public Character Target {
		get {
			return this.target;
		}
	}

	public void performMove () {
		manager.switchPlayers (target);
	}
}

[System.Serializable]
public class HealingSpell : CharacterMove {

	private Character user;
	private Character target;
	private int healthRestore;
	private string text;

	public HealingSpell (Character user, Character target, int healthRestore)
	{
		this.user = user;
		this.target = target;
		this.healthRestore = healthRestore;
		this.text = "healed";
	}

	public string Text {
		get {
			return this.text;
		}
	}

	public Character User {
		get {
			return this.user;
		}
	}

	public Character Target {
		get {
			return this.target;
		}
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

	private string text;
	private string desc;
	private int magic;
	private Character user;
	private Character target;

	public Fireball (string text, string desc, int magic) {
		this.text = text;
		this.desc = desc;
		this.magic = magic;
	}

	public int Magic {
		get {
			return this.magic;
		}
	}

	public string Text {
		get {
			return this.text;
		}
	}

	public Character User {
		get {
			return this.user;
		}
	}

	public Character Target {
		get {
			return this.target;
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