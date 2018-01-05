using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataComponent : MonoBehaviour {

	private Player player;

	public Player Player {
		get {
			return this.player;
		}
		set {
			player = value;
		}
	}
}
