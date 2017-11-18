using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class BattleScriptTest {

	[TestFixture]
	public class Tests {

		Player playerObject;
		Enemy enemyObject;
		Player player;
		Enemy enemy;
		BattleManager manager;

		[OneTimeSetUp]
		public void Init() {
			//Player(Name, Level, Health, Attack, Defence, Magic, Luck, Speed, Exp, Item)
			this.playerObject = new Player ("Player", 10, 100, 10, 10, 10, 10, 10, 2000, "Hammer");
			//Enemy(Name, Level, Health, Attack, Defence, Magic, Luck, Speed)
			this.enemyObject = new Enemy ("Enemy", 10, 100, 5, 5, 5, 5, 5);
			this.manager = new BattleManager (playerObject, enemyObject);
			this.player = manager.Player;
			this.enemy = manager.Enemy;
		}

		[Test]
		public void Constructor() {
			Assert.AreEqual (player, playerObject);
			Assert.AreEqual (enemy, enemyObject);
		}

		[Test]
		public void TurnOrder() {
			//Initially
			Assert.True (manager.PlayerFirst);
			//Change
			enemy.Speed = enemy.Speed + 10;
			manager.calculatePlayerFirst ();
			Assert.False (manager.PlayerFirst);
		}

		[Test]
		public void BasicAttack() {
			//Damage Calculations
			manager.attack (10, player, enemy); //Should do 20 damage
			Assert.AreEqual (80, enemy.Health);
			manager.attack (10, enemy, player); //Should do 5 damage
			Assert.AreEqual (95, player.Health);
			//Only Integer Healths
			manager.attack (3, enemy, player); //Should do 1.5 = 2 damage
			Assert.AreEqual (93, player.Health);
			//No Negative Health
			manager.attack(200, player, enemy); //Should do 400 damage
			Assert.AreEqual(0, enemy.Health);
		}
	}

	


	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator BattleScriptTestWithEnumeratorPasses() {
		// Use the Assert class to test conditions.
		// yield to skip a frame
		yield return null;
	}
}
