using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterMove {

	protected BattleManager manager;
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

	protected string desc;
	protected int magic;

	public int Magic {
		get {
			return this.magic;
		}
	}

	public string Desc {
		get {
			return this.desc;
		}
	}

	public void setUp (BattleManager manager, Character user, Character target) {
		this.manager = manager;
		this.user = user;
		this.target = target;
	}

	public void decreaseMagic() {
		user.Magic -= magic;
	}
}

[System.Serializable]
public class StandardAttack : CharacterMove {

	private int power;

	public StandardAttack(BattleManager manager, Character user, Character target) {
		this.manager = manager;
		this.user = user;
		this.target = target;
		this.power = 10;
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

	public SwitchPlayers (BattleManager manager, Character user, Character target)
	{
		this.manager = manager;
		this.user = user;
		this.target = target;
		this.text = "switched with";
	}

	public override void performMove () {
		manager.switchPlayers ((Player) target);
	}
}

[System.Serializable]
public class HealingSpell : CharacterMove {

	private int healthRestore;

	public HealingSpell (BattleManager manager, Character user, Character target, int healthRestore)
	{
		this.manager = manager;
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
public class MagicAttack : SpecialMove {

	private int power;

	public MagicAttack (string text, string desc, int power, int magic) {
		this.text = text;
		this.desc = desc;
		this.power = power;
		this.magic = magic;
	}

	public override void performMove() {
		int damage = manager.damageCalculation (user, target, power);
		target.Health -= damage;
		decreaseMagic ();
	}

}

[System.Serializable]
public class LowerDefence : SpecialMove {

	private float decrease;

	public LowerDefence (string text, string desc, float decrease, int magic) {
		this.text = text;
		this.desc = desc;
		this.decrease = decrease;
		this.magic = magic;
	}

	public override void performMove() {
		target.Defence = Mathf.RoundToInt (target.Defence * (1 - decrease));
		decreaseMagic ();
	}

}
