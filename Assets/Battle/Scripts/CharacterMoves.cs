using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CharacterMove {
	void performMove (); 
}

[System.Serializable]
public class StandardAttack : CharacterMove {

	private Character user;
	private Character target;
	private int power;

	public StandardAttack (Character user, Character target, int power)
	{
		this.user = user;
		this.target = target;
		this.power = power;
	}
		
	public void performMove () {
		int damage = (int) Mathf.Round( (float) user.Attack * power / target.Defence);
		if (damage < target.Health) {
			target.Health = target.Health - damage;
		} else {
			target.Health = 0;
		}
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