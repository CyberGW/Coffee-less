using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

[TestFixture]
public class PlayerDataTest {

	Player playerObject;
	DataManager data;

	[SetUp]
	public void Setup() {
		playerObject = new Player ("George", 1, 100, 30, 5, 5, 5, 5, 5, 0, null, null, null);
		data = new DataManager (playerObject);
	}

	[Test]
	public void Constructor() {
		Assert.AreEqual (playerObject, data.Players [0]);
		Assert.AreEqual (0, data.countItems());
		Assert.AreEqual (0, data.Money);
	}

	[Test]
	public void SwapPlayers() {
		data.swapPlayers (0, 3);
		Assert.Null (data.Players [0]);
		Assert.AreEqual (playerObject, data.Players [3]);
	}

	[Test]
	public void AddPlayer() {
		Player newPlayer = new Player ("Player2", 1, 1, 1, 1, 1, 1, 1, 1, 1, null, null, null);
		data.addPlayer (newPlayer);
		Assert.AreEqual (newPlayer, data.Players [1]);
		//Fill array by adding 4 more
		for (int i = 0; i < 4; i++) {
			data.addPlayer (newPlayer);
		}
		//Check error is thrown when trying to add 7th player
		Assert.Throws<System.InvalidOperationException>( () => data.addPlayer (newPlayer));
	}

	[Test]
	public void GetFirstPlayer() {
		//Initially
		Assert.AreEqual (playerObject, data.getFirstPlayer ());
		//Swap players
		data.swapPlayers (0, 1);
		Assert.Null (data.getFirstPlayer ());
	}

	[Test]
	public void AddItem() {
		Item itemObject = new Hammer ();
		data.addItem (itemObject);
		Assert.AreEqual (1, data.countItems());
		Assert.AreEqual (itemObject, data.Items [0]);
	}

	[Test]
	public void PlayersAlive() { 
		//Add one more player
		Player newPlayer = new Player ("Player2", 1, 1, 1, 1, 1, 1, 1, 1, 1, null, null, null);
		data.addPlayer (newPlayer);
		//Check initially 2
		Assert.AreEqual (2, data.playersAlive ());
		//Kill one player
		data.Players [1].Health = 0;
		Assert.AreEqual (1, data.playersAlive ());
		//Kill the other
		data.Players [0].Health = 0;
		Assert.AreEqual (0, data.playersAlive ());
	}

	[Test]
	public void CountItems() {
		//Initially
		Assert.AreEqual (0, data.countItems ());
		//Add Item
		Item itemObject = new Hammer ();
		data.addItem (itemObject);
		Assert.AreEqual (1, data.countItems ());
	}

}
