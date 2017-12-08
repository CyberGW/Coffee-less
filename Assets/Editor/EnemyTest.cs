using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

[TestFixture]
public class EnemyTest {

	Enemy enemy;

	[OneTimeSetUp]
	public void Init() {
		this.enemy = new Enemy ("Enemy1", 10, 100, 20, 10, 30, 25, 5, 30, null, null);
	}

	[Test]
	public void Attributes() {
		Assert.AreEqual ("Enemy1", enemy.Name);
		Assert.AreEqual (10, enemy.Level);
		Assert.AreEqual (100, enemy.Health);
		Assert.AreEqual (20, enemy.Attack);
		Assert.AreEqual (10, enemy.Defence);
		Assert.AreEqual (30, enemy.MaximumMagic);
		Assert.AreEqual (25, enemy.Magic);
		Assert.AreEqual (5, enemy.Luck);
		Assert.AreEqual (30, enemy.Speed);
		Assert.AreEqual (null, enemy.Special1);
		Assert.AreEqual (null, enemy.Special2);
		Assert.AreEqual (1000, enemy.ExpGiven);
	}

	[Test]
	public void Setters() {
		enemy.Attack = enemy.Attack + 2;
		enemy.Defence = enemy.Defence / 2;
		Assert.AreEqual (22, enemy.Attack);
		Assert.AreEqual (5, enemy.Defence);
	}
}