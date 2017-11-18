using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : Character {

	private string name;
	private int level;
	private int health;
	private int attack;
	private int defence;
	private int magic;
	private int luck;
	private int speed;
	private int exp;
	private string item;

	public Player (string name, int level, int health, int attack, int defence, int magic, int luck, int speed, int exp, string item)
	{
		this.name = name;
		this.level = level;
		this.health = health;
		this.attack = attack;
		this.defence = defence;
		this.magic = magic;
		this.luck = luck;
		this.speed = speed;
		this.exp = exp;
		this.item = item;
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

	public int Exp {
		get {
			return this.exp;
		}
		set {
			exp = value;
		}
	}

	public string Item {
		get {
			return this.item;
		}
		set {
			item = value;
		}
	}
}
