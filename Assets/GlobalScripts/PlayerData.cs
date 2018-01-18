using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A monobehaviour object to house an instance of <see cref="DataManager"/> so it can be called by other objects
/// </summary>
public class PlayerData : MonoBehaviour {

	public DataManager data;
	public static PlayerData instance = null;

	/// <summary>
	/// Creates <see cref="DataManager"/> object and adds initial player 
	/// </summary>
	void Awake() {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

}

/// <summary>
/// An object to store all player data including players, items and money and provide useful functions
/// </summary>
public class DataManager {

	private Player[] players;
	private Item[] items;
	private int money;

	public DataManager(Player initialPlayer) {
		players = new Player[6];
		players [0] = initialPlayer;
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

	/// <summary>
	/// Gets the first player.
	/// </summary>
	/// <returns>The first player in the array, <see cref="players"/>[0] </returns>
	public Player getFirstPlayer() {
		return players [0];
	}

	/// <summary>
	/// Returns the number of players in <see cref="players"/> that are not null and have health above zero 
	/// </summary>
	/// <returns>The number of players alive</returns>
	public int playersAlive() {
		int alive = 0;
		foreach (Player player in players) {
			if (player != null) {
				if (player.Health > 0) {
					alive += 1;
				}
			}
		}
		return alive;
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
		Player temp = players [index1];
		players [index1] = players [index2];
		players [index2] = temp;
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
