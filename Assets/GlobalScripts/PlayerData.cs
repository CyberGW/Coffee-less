using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A monobehaviour object to house an instance of <see cref="DataManager"/> so it can be called by other objects
/// </summary>
public class PlayerData : MonoBehaviour {

	public DataManager data;
	public static PlayerData instance = null;

	// Use this for initialization
	void Awake() {
		
		data = new DataManager (new Player ("George", 1, 100, 30, 5, 5, 5, 5, 5, 0, null,
			new MagicAttack("hi-jump kicked", "Kick with power 15", 3, 15),
			new RaiseDefence("buffed up against", "Increase your defence by 10%", 2, 0.1f),
			(Texture2D) Resources.Load("Character1", typeof(Texture2D))));

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

/// <summary>
/// An object to store all player data including players, items and money and provide useful functions
/// </summary>
public class DataManager {

	private Player[] players;
	private int alive;
	private Item[] items;
	private int money;

	public DataManager(Player initialPlayer) {
		players = new Player[6];
		players [0] = initialPlayer;
		alive = 1;
		items = new Item[6];
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

	public Item[] Items {
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

	/// <summary>
	/// Adds a new player to <see cref="players"/> if not-full, otherwise throwing an <c> InvalidOperationException</c>
	/// </summary>
	/// <param name="player">The player to add to the array</param>
	public void addPlayer(Player player) {
		bool added = false;
		for (int i = 0; i < players.Length; i++) {
			if (players[i] == null) {
				players[i] = player;
				alive += 1;
				added = true;
				break;
			}
		}
		if (!added) {
			throw new System.InvalidOperationException("Player Array is full");
		}
	}

	/// <summary>Swap two player's positions in <see cref="players"/></summary>
	/// <param name="index1">The index of one player to swap</param>
	/// <param name="index2">The index of the other player to swap</param>
	public void swapPlayers(int index1, int index2) {
		Debug.Log ("Original Index1: " + players [index1].Name);
		Player temp = players [index1];
		players [index1] = players [index2];
		players [index2] = temp;
		Debug.Log ("New Index1: " + players [index1].Name);
	}

	/// <summary>
	/// Add an item to <see cref="items"/> 
	/// </summary>
	/// <param name="item">The item to add</param>
	public void addItem(Item item) {
		for (int i = 0; i < items.Length; i++) {
			if (items [i] == null) {
				items [i] = item;
				break;
			}
		}
	}

	/// <summary>
	/// Counts all items in <see cref="items"/> which are not null
	/// </summary>
	/// <returns>The number of non-null elements in the array</returns>
	public int countItems() {
		int count = 0;
		for (int i = 0; i < items.Length; i++) {
			if (items [i] != null) {
				count += 1;
			}
		}
		return count;
	}

}
