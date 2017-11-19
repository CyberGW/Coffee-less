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
		CharacterMove playerMove;
		CharacterMove enemyMove;

		[SetUp]
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
		public void StandardAttack() {
			//Damage Calculations
			playerMove = new StandardAttack(player, enemy, 10); //Should do 20 damage
			playerMove.performMove ();
			Assert.AreEqual (80, enemy.Health);
			enemyMove = new StandardAttack (enemy, player, 10); //Should do 5 damage
			enemyMove.performMove ();
			Assert.AreEqual (95, player.Health);

			//Only Integer Healths
			enemyMove = new StandardAttack(enemy, player, 3); //Should do 1.5 = 2 damage
			enemyMove.performMove ();
			Assert.AreEqual (93, player.Health);

			//No Negative Health
			playerMove = new StandardAttack(player, enemy, 200); //Would lower enemy health below zero
			playerMove.performMove ();
			Assert.AreEqual(0, enemy.Health);
		}

		[Test]
		public void ChangePlayer() {
			Player newPlayer = new Player ("Second Player", 1, 1, 1, 1, 1, 1, 1, 1, "None");
			manager.Player = newPlayer;
			Assert.AreEqual (newPlayer, manager.Player);
		}

		[Test]
		public void HealthRestore() {
			//Damage Enemy so we have a target to heal
			playerMove = new StandardAttack (player, enemy, 10); //Should do 20 damage
			playerMove.performMove ();
			playerMove = new HealingSpell (player, enemy, 15); //Should restore back up to 95
			playerMove.performMove ();
			Assert.AreEqual (95, enemy.Health);

			//Check Health doesn't go beyond 100
			playerMove.performMove (); //Should restore health to 105 = 100
			Assert.AreEqual (100, enemy.Health);
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
