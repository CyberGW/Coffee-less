using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class SpecialMoveTest {

	private Player playerObject;
	private Enemy enemyObject;
	private BattleManager manager;
	private Player player;
	private Enemy enemy;

	[SetUp]
	public void Setup() {
		//Player(Name, Level, Health, Attack, Defence, Magic, Luck, Speed, Exp, Item)
		this.playerObject = new Player ("Player", 10, 100, 10, 10, 10, 10, 10, 10, 2000, null, null, null);
		//Enemy(Name, Level, Health, Attack, Defence, Magic, Luck, Speed)
		this.enemyObject = new Enemy ("Enemy", 10, 100, 10, 10, 10, 10, 10, 10, new MagicAttack("fireballed", "Fireball", 30, 5), new MagicAttack("fireballed", "Fireball", 30, 10));
		this.manager = new BattleManager (playerObject, enemyObject);
		this.player = manager.Player;
		this.enemy = manager.Enemy;
		manager.forceCriticalHits = "None";
	}

	[Test]
	public void MagicAttack() {
		player.Special1 = new MagicAttack ("fireballed", "Sends a fireball with 30 power", 30, 5);
		player.Special1.setUp (manager, player, enemy);
		player.Special1.performMove ();
		Assert.AreEqual (70, enemy.Health); //Check correct damage is dealt
		Assert.AreEqual (5, player.Magic); //Check player's magic is lowered
	}

	[Test]
	public void LowerDefence() {
		player.Special1 = new LowerDefence ("weakened", "Lowers enemy's defence", 0.1f, 3);
		player.Special1.setUp (manager, player, enemy);
		player.Special1.performMove ();
		Assert.AreEqual(9, enemy.Defence);
		Assert.AreEqual (7, player.Magic);
		//Check rounding
		player.Special1 = new LowerDefence ("weakened", "Lowers enemy's defence", 0.15f, 3);
		player.Special1.setUp (manager, player, enemy);
		player.Special1.performMove ();
		Assert.AreEqual (8, enemy.Defence); //Should decrease to 7.5 = 8
		Assert.AreEqual (4, player.Magic);
	}

}
