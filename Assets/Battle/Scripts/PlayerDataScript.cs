using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataScript : MonoBehaviour {

	public Player[] playerArray;
	public int alive;
	private static bool dataExists = false;

	// Use this for initialization
	void Start () {
		playerArray = new Player[6];
		for (int i = 0; i < 6; i++) {
			playerArray [i] = new Player("Test",1,100,30,5,5,5,5,0, null,null,null);
		}
		playerArray [0].Item = new Hammer (playerArray [0]);
		alive = 6;

		if (!dataExists) {
			dataExists = true;
			DontDestroyOnLoad (transform.gameObject);
		} else {
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
