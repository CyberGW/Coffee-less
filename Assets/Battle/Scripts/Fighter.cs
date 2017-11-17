using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Fighter {

	private string name;
	private int health;
	private int attack;
	private int defence;
	private int magic;
	private int luck;
	private int speed;

	public Fighter (string name, int health, int attack, int defence, int magic, int luck, int speed)
	{
		this.name = name;
		this.health = health;
		this.attack = attack;
		this.defence = defence;
		this.magic = magic;
		this.luck = luck;
		this.speed = speed;
	}


	public string Name {
		get {
			return this.name;
		}
		set {
			name = value;
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
}
