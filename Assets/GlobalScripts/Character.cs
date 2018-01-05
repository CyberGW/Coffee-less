using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base abstract class for Enemy and Player to inherit from.
/// Defines all shared variables, their constructor and their setters and getters
/// </summary>
public abstract class Character {

	protected string name;
	protected int level;
	protected int health;
	protected int attack;
	protected int defence;
	protected int maximumMagic;
	protected int magic;
	protected int luck;
	protected int speed;
	protected SpecialMove special1;
	protected SpecialMove special2;
	protected Texture2D image;

	protected Character (string name, int level, int health, int attack, int defence, int maximumMagic,
		int magic, int luck, int speed, SpecialMove special1, SpecialMove special2, Texture2D image = null)
	{
		this.name = name;
		this.level = level;
		this.health = health;
		this.attack = attack;
		this.defence = defence;
		this.maximumMagic = maximumMagic;
		this.magic = magic;
		this.luck = luck;
		this.speed = speed;
		this.special1 = special1;
		this.special2 = special2;
		this.image = image;
	}		

	public string Name {
		get {
			return this.name;
		}
		set {
			name = value;
		}
	}

	public int Level {
		get {
			return this.level;
		}
		set {
			level = value;
		}
	}

	public int Health {
		get {
			return this.health;
		}
		set {
			health = value;
		}
	}

	public int Attack {
		get {
			return this.attack;
		}
		set {
			attack = value;
		}
	}

	public int Defence {
		get {
			return this.defence;
		}
		set {
			defence = value;
		}
	}

	public int MaximumMagic {
		get {
			return this.maximumMagic;
		}
		set {
			maximumMagic = value;
		}
	}

	public int Magic {
		get {
			return this.magic;
		}
		set {
			magic = value;
		}
	}

	public int Luck {
		get {
			return this.luck;
		}
		set {
			luck = value;
		}
	}

	public int Speed {
		get {
			return this.speed;
		}
		set {
			speed = value;
		}
	}

	public SpecialMove Special1 {
		get {
			return this.special1;
		}
		set {
			special1 = value;
		}
	}

	public SpecialMove Special2 {
		get {
			return this.special2;
		}
		set {
			special2 = value;
		}
	}

	public Texture2D Image {
		get {
			return this.image;
		}
	}

}
