using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

[TestFixture]
public class BattleScriptTest {

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
		this.playerObject = new Player ("Player", 10, 100, 10, 10, 10, 10, 10, 10, 2000, null, new MagicAttack("fireballed", "Fireball", 5, 30), null);
		//Enemy(Name, Level, Health, Attack, Defence, Magic, Luck, Speed)
		this.enemyObject = new Enemy ("Enemy", 10, 100, 5, 5, 5, 5, 5, 5, new MagicAttack("fireballed", "Fireball", 5, 30), new MagicAttack("fireballed", "Fireball", 30, 10));
		this.manager = new BattleManager (playerObject, enemyObject, 0);
		this.player = manager.Player;
		this.enemy = manager.Enemy;
		manager.forceCriticalHits = "None";
	}

	[Test]
	public void Constructor() {
		Assert.AreEqual (player, playerObject);
		Assert.AreEqual (enemy, enemyObject);
	}

	[Test]
	public void TurnOrder() {
		//Initially
		Assert.True (manager.playerFirst());
		//Change
		enemy.Speed = enemy.Speed + 10;
		Assert.False (manager.playerFirst());
	}

	[Test]
	public void Items() {
		player.Item = new Hammer ();
		//manager.applyItem ();
		//Check Attack has increased accordingly
		Assert.AreEqual (15, player.Attack);
		//Check in Attack calculations
		playerMove = new StandardAttack (manager, player, enemy); //Should now do 30 damage
		playerMove.performMove();
		Assert.AreEqual(70, enemy.Health);
	}


	[Test]
	public void StandardAttack () {
		//Damage Calculations
		playerMove = new StandardAttack (manager,player, enemy); //Should do 20 damage
		playerMove.performMove ();
		Assert.AreEqual (80, enemy.Health);
		enemyMove = new StandardAttack (manager,enemy, player); //Should do 5 damage
		enemyMove.performMove ();
		Assert.AreEqual (95, player.Health);

		//Only Integer Healths
		player.Defence = 3;
		enemyMove = new StandardAttack (manager, enemy, player); //Should do 16.66 = 17 damage
		enemyMove.performMove ();
		Assert.AreEqual (78, player.Health);

		//No Negative Health
		enemy.Health = 1;
		playerMove = new StandardAttack (manager,player, enemy); //Would lower enemy health below zero
		playerMove.performMove ();
		Assert.AreEqual(0, enemy.Health);
	}

	[Test]
	public void ChangePlayer() {
		Player newPlayer = new Player ("Second Player", 1, 1, 1, 1, 1, 1, 1, 1, 1, null, null, null);
		playerMove = new SwitchPlayers (manager, player, newPlayer);
		playerMove.performMove ();
		Assert.AreEqual (newPlayer, manager.Player); //Check player has been reassigned correctly
		Assert.False (manager.playerFirst()); //Check PlayerFirst has been updated
	}

	[Test]
	public void PlayerSpecialMove() {
		player.Special1.setUp (manager, player, enemy);
		playerMove = player.Special1;
		playerMove.performMove ();
		//Should do 30 * 10 / 5 = 60 damage
		Assert.AreEqual (40, enemy.Health);
	}

	[Test]
	[Ignore("Manual Probability Test")]
	public void CriticalHitGeneration() {
		//Player has 6% chance of generating a critical hit
		int n = 0;
		for (int i=0; i<1000; i++) {
			if (manager.isCriticalHit (player.Luck)) {
				n += 1;
			}
		}
		Debug.Log (n);
	}

	[Test]
	public void CriticalHitCalculation() {
		manager.forceCriticalHits = "All";
		playerMove = new StandardAttack (manager,player, enemy); //Should do 20 * 1.75 = 35 damage
		playerMove.performMove ();
		Assert.AreEqual (65, enemy.Health);
		enemyMove = new StandardAttack (manager,enemy, player); //Should do 5 * 1.75 = 8.75 = 9 damage
		enemyMove.performMove ();
		Assert.AreEqual (91, player.Health);
	}

	[Test]
	public void EnemySpecialMoveAI() {
		//try special 1
		Assert.AreEqual (enemy.Special1, manager.enemySpecialMove (0));
		//try special 2 but fail as not enough magic
		Assert.IsInstanceOf(typeof(StandardAttack), manager.enemySpecialMove(1));
	}

	[Test]
	public void BattleWon() {
		//Initially
		Assert.False (manager.battleWon ());
		//Standard Test above 0
		enemy.Health = 40; //Should do 60 damage, so not won
		Assert.False (manager.battleWon ());
		//Boundary Test at 1
		enemy.Health = 1;
		Assert.False (manager.battleWon ());
		//Boundary Test at 0
		enemy.Health = 0;
		Assert.True (manager.battleWon ());
		//Standard Test below 0
		enemy.Health = 10;
		playerMove = new StandardAttack (manager, player, enemy); //Should deal 20 damage, taking health to -10 = 0
		playerMove.performMove ();
		Assert.True (manager.battleWon ());
	}

	[Test]
	public void CharacterFainted() {
		//Initially
		Assert.False (manager.playerFainted ());
		player.Health = 1;
		//Check still alive at 1
		Assert.False (manager.playerFainted ());
		enemyMove = new StandardAttack (manager, enemy, player);
		enemyMove.performMove ();
		//Check that character fainting has been recognised
		Assert.True (manager.playerFainted ());
	}

}