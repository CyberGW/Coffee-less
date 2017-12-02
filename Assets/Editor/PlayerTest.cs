using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class PlayerTest {

	[TestFixture]
	public class Tests {
		
		Player player;

		[SetUp]
		public void Init() {
			this.player= new Player ("Player", 10, 100, 10, 10, 10, 10, 10, 10, 0, null, new Fireball("Fireball", "Instant Kill", 5), null);
		}

		[Test]
		public void expToNextLevel() {
			//Player is Level 10 = 10*200 = 2,000 Exp to Level 11
			Assert.AreEqual( 2000, player.ExpToNextLevel);
		}

		[Test]
		public void gainExp() {
			//Simple Increase
			player.gainExp (1000);
			Assert.AreEqual (1000, player.Exp);
			//Check Accumulative
			player.gainExp (500);
			Assert.AreEqual (1500, player.Exp);
			//Check resets to 0 when levelled up
			player.gainExp(500);
			Assert.AreEqual (0, player.Exp);
			//Check exp is still added after levelling up
			player.gainExp (2700); // As expToNextLevel = 2200, should go back round to 500
			Assert.AreEqual (500, player.Exp);
			//Check works with multiple level ups
			player.gainExp(4520); //1900 + 2600 + 20
			Assert.AreEqual (20, player.Exp);
		}

		[Test]
		public void levelUp() {
			player.levelUp ();
			//Check stat increases
			Assert.AreEqual (11, player.Level);
			Assert.AreEqual (12, player.Attack);
			Assert.AreEqual (12, player.Defence);
			Assert.AreEqual (12, player.MaximumMagic);
			Assert.AreEqual (12, player.Luck);
			Assert.AreEqual (12, player.Speed);
			//Check exp reset
			Assert.AreEqual (0, player.Exp);
			//Check recalculation of expToNextLevel
			Assert.AreEqual (2200, player.ExpToNextLevel);
		}


	}
			

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
//	[UnityTest]
//	public IEnumerator PlayerTestWithEnumeratorPasses() {
//		// Use the Assert class to test conditions.
//		// yield to skip a frame
//		yield return null;
//	}
}
