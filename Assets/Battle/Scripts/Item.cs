using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item {

	protected string name;
	protected string desc;

	public string Name {
		get {
			return this.name;
		}
		set {
			name = value;
		}
	}

	public string Desc {
		get {
			return this.desc;
		}
		set {
			desc = value;
		}
	}

	abstract public void applyBuffs(Player player);
	abstract public void revertBuffs(Player player);
}

[System.Serializable]
public class Hammer : Item {

	public Hammer ()
	{
		this.name = "Hammer";
		this.desc = "Increases user's attack power by 5";
	}

	//Increase user's attack by 5
	public override void applyBuffs(Player player) {
		player.Attack += 5;
	}

	public override void revertBuffs(Player player) {
		player.Attack -= 5;
	}

}
