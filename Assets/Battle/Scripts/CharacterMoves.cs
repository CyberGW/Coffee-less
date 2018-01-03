using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Astract base class for character moves
/// </summary>
public abstract class CharacterMove {

	protected BattleManager manager;
	protected Character user;
	protected Character target;
	/// <summary>The text to display when the move is executed</summary>
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

	/// <summary>
	/// Performs the move.
	/// </summary>
	abstract public void performMove (); 
}

/// <summary>
/// Special move abstract base class that expands upon CharacterMove, adding magic properties
/// </summary>
public abstract class SpecialMove : CharacterMove {

	protected string desc;
	protected int magic;

	protected SpecialMove(string text, string desc, int magic) {
		this.text = text;
		this.desc = desc;
		this.magic = magic;
	}

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
	 /// <summary>
	 /// Sets up the move, referencing the current instance of BattleManager, user and target
	 /// </summary>
	/// <param name="manager">The battle manager</param>
	 /// <param name="user">The user.</param>
	 /// <param name="target">The target</param>
	public void setUp (BattleManager manager, Character user, Character target) {
		this.manager = manager;
		this.user = user;
		this.target = target;
	}

	/// <summary>
	/// Decreases the user's magic after <see cref="CharacterMove.performMove"/> has been called
	/// </summary>
	public void decreaseMagic() {
		user.Magic -= magic;
	}
}

/// <summary>
/// Basic standard attack
/// </summary>
public class StandardAttack : CharacterMove {

	private int power;

	public StandardAttack(BattleManager manager, Character user, Character target) {
		this.manager = manager;
		this.user = user;
		this.target = target;
		this.power = 10;
		this.text = "attacked";
	}

	/// <summary>
	/// Calculate damage by <see cref="BattleManager.damageCalculation"/> and subtract it from target's health 
	/// </summary>
	public override void performMove () {
		int damage = manager.damageCalculation (user, target, power);
		target.Health -= damage;
	}
}

/// <summary>
/// Simple move to swap out the current player
/// </summary>
public class SwitchPlayers : CharacterMove {

	public SwitchPlayers (BattleManager manager, Character user, Character target)
	{
		this.manager = manager;
		this.user = user;
		this.target = target;
		this.text = "switched with";
	}

	/// <summary>
	/// Calls <see cref="BattleManager.switchPlayers"/> to switch in the target 
	/// </summary>
	public override void performMove () {
		manager.switchPlayers ((Player) target);
	}
}

/// <summary>
/// An attack move that uses magic
/// </summary>
public class MagicAttack : SpecialMove {

	/// <summary>Defines the power of the attack</summary>
	private int power;

	public MagicAttack (string text, string desc, int magic, int power) : base(text, desc, magic) {
		this.power = power;
	}

	/// <summary>Calls <see cref="BattleManager.damageCalculation"/> and subtracts this from target's health</summary>
	public override void performMove() {
		int damage = manager.damageCalculation (user, target, power);
		target.Health -= damage;
		decreaseMagic ();
	}

}

/// <summary>
/// Lower's the target's defence
/// </summary>
public class LowerDefence : SpecialMove {

	/// <summary>The ratio to decrease the defence stat by</summary>
	private float decrease;

	public LowerDefence (string text, string desc, int magic, float decrease) : base(text, desc, magic) {
		this.decrease = decrease;
	}

	/// <summary>Lowers the target's defence by the <see cref="decrease"/> ratio and rounds to an integer</summary>
	public override void performMove() {
		target.Defence = Mathf.RoundToInt (target.Defence * (1 - decrease));
		decreaseMagic ();
	}

}

/// <summary>
/// Lower's the target's speed
/// </summary>
public class LowerSpeed : SpecialMove {

	/// <summary>
	/// The ratio to decrease the target's speed by
	/// </summary>
	private float decrease;

	public LowerSpeed (string text, string desc, int magic, float decrease) : base(text, desc, magic) {
		this.decrease = decrease;
	}

	/// <summary>Lower's the target's speed by the <see cref="decrease"/> ratio and rounds to an integer</summary>
	public override void performMove() {
		target.Speed = Mathf.RoundToInt (target.Speed * (1 - decrease));
		decreaseMagic ();
	}

}

/// <summary>Raise the target's attack stat</summary>
public class RaiseAttack : SpecialMove {

	/// <summary>The ratio to increase the attack by</summary>
	private float increase;

	public RaiseAttack (string text, string desc, int magic, float increase) : base(text, desc, magic) {
		this.increase = increase;
	}

	/// <summary>Increases the target's attack by the <see cref="increase"/> ratio and rounds to an integer</summary>
	public override void performMove() {
		user.Attack = Mathf.RoundToInt (user.Attack * (1 + increase));
		decreaseMagic();
	}

}

/// <summary>Raises the target's defence stat</summary>
public class RaiseDefence : SpecialMove { 

	/// <summary>The ratio to increase the defence by</summary>
	private float increase;

	public RaiseDefence (string text, string desc, int magic, float increase) : base(text, desc, magic) {
		this.increase = increase;
	}

	/// <summary>Increases the target's defence by the <see cref="increase"/> ratio and rounds to an integer</summary>
	public override void performMove() {
		user.Defence = Mathf.RoundToInt (user.Defence * (1 + increase));
		decreaseMagic();
	}

}

/// <summary>Increases the money reward of a battle</summary>
public class IncreaseMoney : SpecialMove {

	/// <summary>The ratio to increase the money reward by </summary>
	private float increase;

	public IncreaseMoney(string text, string desc, int magic, float increase) : base(text, desc, magic) {
		this.increase = increase;
	}

	/// <summary>Increases the <see cref="BattleManager.money"/> value by the <see cref="increase"/> ratio</summary>
	public override void performMove() {
		manager.money = Mathf.RoundToInt (manager.money * (1 + increase));
		decreaseMagic ();
	}

}

/// <summary>Heal a target by a set amount of health points</summary>
public class HealingSpell : SpecialMove {

	/// <summary>The amount of health points to restore</summary>
	private int increase;

	public HealingSpell(string text, string desc, int magic, int increase) : base(text, desc, magic) {
		this.increase = increase;
	}

	/// <summary>Increases the target's health by <see cref="increase"/> ensuring it does not go beyond 100</summary>
	public override void performMove() {
		if (target.Health + increase >= 100) {
			target.Health = 100;
		} else {
			target.Health += increase;
		}
		decreaseMagic ();
	}

}
