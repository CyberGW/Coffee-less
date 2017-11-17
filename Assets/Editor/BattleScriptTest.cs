using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class BattleScriptTest {

	[TestFixture]
	public class Tests {

		Fighter player;
		Fighter enemy;
		BattleManager manager;
		
		[OneTimeSetUp]
		public void Init() {
			this.player = new Fighter ("George", 100, 20, 10, 25, 5, 30);
			this.enemy = new Fighter ("Ben", 100, 5, 15, 15, 10, 20);
			this.manager = new BattleManager (player, enemy);
		}

		[Test]
		public void Constructor() {
			Assert.AreEqual (manager.Player, player);
			Assert.AreEqual (manager.Enemy, enemy);
		}

		[Test]
		public void TurnOrder() {
			//Initially
			Assert.AreEqual (manager.TurnOrder, new Fighter[] { player, enemy } );
			//Change
			enemy.Speed = enemy.Speed * 2;
			manager.Enemy = enemy;
			Assert.AreEqual (manager.TurnOrder, new Fighter[] { enemy, player } );
		}

	}
	


//	// A UnityTest behaves like a coroutine in PlayMode
//	// and allows you to yield null to skip a frame in EditMode
//	[UnityTest]
//	public IEnumerator BattleScriptTestWithEnumeratorPasses() {
//		// Use the Assert class to test conditions.
//		// yield to skip a frame
//		yield return null;
//	}
}
