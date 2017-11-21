using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Item {

	string Name {
		get;
		set;
	}

	string Desc {
		get;
		set;
	}

	void applyBuffs();
}

[System.Serializable]
public class Hammer : Item {

	private Player player;
	private string name;
	private string desc;

	public Hammer (Player player)
	{
		this.player = player;
		this.name = "Hammer";
		this.desc = "Increases user's attack power by 5";
	}

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

	//Increase user's attack by 5
	public void applyBuffs() {
		player.Attack += 5;
	}

}
