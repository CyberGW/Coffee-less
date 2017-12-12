using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {

	public DataManager data;
	public static PlayerData instance = null;

	// Use this for initialization
	void Awake() {
		
		data = new DataManager (new Player ("George", 1, 100, 30, 5, 5, 5, 5, 5, 0, null, null, null));

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

}

public class DataManager {

	private Player[] players;
	private int alive;
	private List<Item> items;
	private int money;

	public DataManager(Player initialPlayer) {
		players = new Player[6];
		players [0] = initialPlayer;
		alive = 1;
		items = new List<Item> ();
		money = 0;
	}

	public Player[] Players {
		get {
			return this.players;
		}
		set {
			players = value;
		}
	}

	public int Alive {
		get {
			return this.alive;
		}
		set {
			alive = value;
		}
	}

	public List<Item> Items {
		get {
			return this.items;
		}
	}

	public int Money {
		get {
			return this.money;
		}
		set {
			money = value;
		}
	}

	public Player getFirstPlayer() {
		return players [0];
	}

	public void addPlayer(Player player) {
		bool added = false;
		for (int i = 0; i < players.Length; i++) {
			if (players[i] == null) {
				players[i] = player;
				added = true;
				break;
			}
		}
		if (!added) {
			throw new System.InvalidOperationException("Player Array is full");
		}
	}

	public void swapPlayers(int index1, int index2) {
		Player temp = players [index1];
		players [index1] = players [index2];
		players [index2] = temp;
	}

	public void addItem(Item item) {
		items.Add (item);
	}

}
