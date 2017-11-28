using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {

	public Player[] playerArray;
	public int money;
	public int alive;
	public static PlayerData instance = null;

	// Use this for initialization
	void Awake() {
		playerArray = new Player[6];
		for (int i = 0; i < 6; i++) {
			playerArray [i] = new Player("Test",1,100,30,5,5,5,5,0, null,null,null);
		}
		playerArray [0].Item = new Hammer (playerArray [0]);
		alive = 6;
		money = 0;

		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Player[] getData() {
		return playerArray;
	}

}
