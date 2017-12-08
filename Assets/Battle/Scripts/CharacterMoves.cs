using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterMove {

	protected Character user;
	protected Character target;
	protected string text;

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

	public string Text {
		get {
			return this.text;
		}
	}

	abstract public void performMove (); 
}

public abstract class SpecialMove : CharacterMove {

	protected int magic;

	public int Magic {
		get {
			return this.magic;
		}
	}

	public abstract void setUp(BattleManager manager, Character user, Character target);

	public void decreaseMagic() {
		user.Magic -= magic;
	}
}

[System.Serializable]
public class StandardAttack : CharacterMove {

	private BattleManager manager;
	private int power;

	public StandardAttack (BattleManager manager, Character user, Character target, int power)
	{
		this.manager = manager;
		this.user = user;
		this.target = target;
		this.power = power;
		this.text = "attacked";
	}
		
	public override void performMove () {
		int damage = manager.damageCalculation (user, target, power);
		target.Health -= damage;
	}
}

[System.Serializable]
public class SwitchPlayers : CharacterMove {

	private BattleManager manager;

	public SwitchPlayers (Player currentPlayer, Player newPlayer, BattleManager manager)
	{
		this.user = currentPlayer;
		this.target = newPlayer;
		this.manager = manager;
		this.text = "switched with";
	}

	public override void performMove () {
		manager.switchPlayers ((Player) target);
	}
}

[System.Serializable]
public class HealingSpell : CharacterMove {

	private int healthRestore;

	public HealingSpell (Character user, Character target, int healthRestore)
	{
		this.user = user;
		this.target = target;
		this.healthRestore = healthRestore;
		this.text = "healed";
	}

	public override void performMove() {
		if ((100 - target.Health) > healthRestore) {
			target.Health += healthRestore;
		} else {
			target.Health = 100;
		}
	}
}

[System.Serializable]
public class Fireball : SpecialMove {

	private string desc;

	public Fireball (string text, string desc, int magic) {
		this.text = text;
		this.desc = desc;
		this.magic = magic;
	}

	public override void setUp(BattleManager manager, Character user, Character target) {
		this.target = target;
		this.user = user;
	}

	public override void performMove() {
		target.Health -= 40;
		decreaseMagic ();
	}

}
