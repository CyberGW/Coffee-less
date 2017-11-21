using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataScript : MonoBehaviour {

	public Player[] playerArray;
	public int alive;

	// Use this for initialization
	void Start () {
		playerArray = new Player[6];
		for (int i = 0; i < 6; i++) {
			playerArray [i] = new Player("Test",1,100,1,1,1,1,1,0,null,null,null);
		}
		alive = 6;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Player[] getData() {
		return playerArray;
	}

}
