using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

[TestFixture]
public class ItemTest {

	Item item;
	Player player;

	[SetUp]
	public void Init() {
		this.item = new Hammer ();
		this.player = new Player ("Player", 10, 10, 10, 10, 10, 10, 10, 10, 10, this.item, null, null); 
	}

	[Test]
	public void Attributes() {
		Assert.AreEqual ("Hammer", item.Name);
		Assert.AreEqual ("Increases user's attack power by 5", item.Desc);
	}

	[Test]
	public void ApplyBuffs() {
		item.applyBuffs (player);
		Assert.AreEqual (15, player.Attack);
	}

}
