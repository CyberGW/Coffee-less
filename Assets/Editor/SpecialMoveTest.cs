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
		this.manager = new BattleManager (playerObject, enemyObject, 50);
		this.player = manager.Player;
		this.enemy = manager.Enemy;
		manager.forceCriticalHits = "None";
	}

	[Test]
	public void MagicAttack() {
		player.Special1 = new MagicAttack ("fireballed", "Sends a fireball with 30 power", 5, 30);
		player.Special1.setUp (manager, player, enemy);
		player.Special1.performMove ();
		Assert.AreEqual (70, enemy.Health); //Check correct damage is dealt
		Assert.AreEqual (5, player.Magic); //Check player's magic is lowered
	}

	[Test]
	public void LowerDefence() {
		player.Special1 = new LowerDefence ("weakened", "Lowers enemy's defence", 3, 0.1f);
		player.Special1.setUp (manager, player, enemy);
		player.Special1.performMove ();
		Assert.AreEqual(9, enemy.Defence);
		Assert.AreEqual (7, player.Magic);
		//Check rounding
		player.Special1 = new LowerDefence ("weakened", "Lowers enemy's defence", 3, 0.15f);
		player.Special1.setUp (manager, player, enemy);
		player.Special1.performMove ();
		Assert.AreEqual (8, enemy.Defence); //Should decrease to 7.5 = 8
		Assert.AreEqual (4, player.Magic);
	}

	[Test]
	public void LowerSpeed() {
		player.Special1 = new LowerSpeed ("slowed down", "Lowers enemy's speed", 3, 0.2f);
		player.Special1.setUp (manager, player, enemy);
		player.Special1.performMove ();
		Assert.AreEqual(8, enemy.Speed);
		Assert.AreEqual (7, player.Magic);
		//Check rounding
		player.Special1 = new LowerSpeed ("slowed down", "Lowers enemy's speed", 3, 0.3f);
		player.Special1.setUp (manager, player, enemy);
		player.Special1.performMove ();
		Assert.AreEqual(6, enemy.Speed); //Should decrease to 5.6 = 6
		Assert.AreEqual (4, player.Magic);
	}

	[Test]
	public void RaiseAttack() {
		player.Special1 = new RaiseAttack ("strengthened against", "Raises your attack", 3, 0.1f);
		player.Special1.setUp (manager, player, player);
		player.Special1.performMove ();
		Assert.AreEqual (11, player.Attack);
		Assert.AreEqual (7, player.Magic);
		//Check rounding
		player.Special1 = new RaiseAttack ("strengthened against", "Raises your attack", 3, 0.15f);
		player.Special1.setUp (manager, player, player);
		player.Special1.performMove ();
		Assert.AreEqual (13, player.Attack); //Should increase to 11 * 1.15 = 12.65 = 13
		Assert.AreEqual (4, player.Magic);
	}

	[Test]
	public void RaiseDefence() {
		player.Special1 = new RaiseDefence ("strengthened against", "Raises your defence", 3, 0.1f);
		player.Special1.setUp (manager, player, player);
		player.Special1.performMove ();
		Assert.AreEqual (11, player.Defence);
		Assert.AreEqual (7, player.Magic);
		//Check rounding
		player.Special1 = new RaiseDefence ("strengthened against", "Raises your defence", 3, 0.15f);
		player.Special1.setUp (manager, player, player);
		player.Special1.performMove ();
		Assert.AreEqual (13, player.Defence); //Should increase to 11 * 1.15 = 12.65 = 13
		Assert.AreEqual (4, player.Magic);
	}

	[Test]
	public void MoneyIncrease() {
		player.Special1 = new IncreaseMoney ("took money off", "Increase money reward", 2, 0.5f);
		player.Special1.setUp (manager, player, enemy);
		player.Special1.performMove ();
		Assert.AreEqual (75, manager.money);
		Assert.AreEqual (8, player.Magic);
		//Check rounding
		player.Special1 = new IncreaseMoney ("took money off", "Increase money reward", 2, 0.3f);
		player.Special1.setUp (manager, player, enemy);
		player.Special1.performMove ();
		Assert.AreEqual (98, manager.money); //Should increase to 75 * 1.3 = 97.5 = 98
		Assert.AreEqual (6, player.Magic);
	}

	[Ignore ("Healing Spells may be removed")]
	public void HealingSpell() {
		player.Health = 40; //Lower player health for test
		player.Special1 = new HealingSpell ("healed", "Heal an ally", 5, 50);
		player.Special1.setUp (manager, player, player);
		player.Special1.performMove ();
		Assert.AreEqual (90, player.Health);
		Assert.AreEqual (5, player.Magic);
		//Check does not go beyond 100
		player.Special1.performMove ();
		Assert.AreEqual (100, player.Health);
		Assert.AreEqual (0, player.Magic);
	}

}
