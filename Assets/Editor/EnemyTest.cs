using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class FighterTest {

	[TestFixture]
	public class Tests {

		Enemy enemy;

		[OneTimeSetUp]
		public void Init() {
			this.enemy = new Enemy ("George", 10, 100, 20, 10, 25, 5, 30);
		}

		[Test]
		public void Constructor() {
			Assert.AreEqual ("George", enemy.Name);
			Assert.AreEqual (100, enemy.Health);
			Assert.AreEqual (20, enemy.Attack);
			Assert.AreEqual (10, enemy.Defence);
			Assert.AreEqual (25, enemy.Magic);
			Assert.AreEqual (5, enemy.Luck);
			Assert.AreEqual (30, enemy.Speed);
		}

		[Test]
		public void Setters() {
			enemy.Attack = enemy.Attack + 2;
			enemy.Defence = enemy.Defence / 2;
			Assert.AreEqual (22, enemy.Attack);
			Assert.AreEqual (5, enemy.Defence);
		}
	}

//	// A UnityTest behaves like a coroutine in PlayMode
//	// and allows you to yield null to skip a frame in EditMode
//	[UnityTest]
//	public IEnumerator FighterTestWithEnumeratorPasses() {
//		// Use the Assert class to test conditions.
//		// yield to skip a frame
//		yield return null;
//	}
}
