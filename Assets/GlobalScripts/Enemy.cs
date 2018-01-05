using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Defines an AI controller enemy to fight in battle scenes</summary>
[System.Serializable]
public class Enemy : Character {

	/// <summary>Defines how many experience points the player should gain from defeating this enemy.
	/// Calculated by <see cref="level"/>  * 100 in constructor</summary>
	private int expGiven;

	public Enemy (string name, int level, int health, int attack, int defence, int maximumMagic,
		int magic, int luck, int speed, SpecialMove special1, SpecialMove special2, Texture2D image = null)
		   : base(name, level, health, attack, defence, maximumMagic, magic, luck, speed, special1, special2, image)
	{
		this.expGiven = level * 100;
	}

	public int ExpGiven {
		get {
			return this.expGiven;
		}
		set {
			expGiven = value;
		}
	}
}
