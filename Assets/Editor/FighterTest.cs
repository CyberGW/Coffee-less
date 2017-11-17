using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class FighterTest {

	[Test]
	public void Constructor() {
		Fighter test = new Fighter ("George", 100, 20, 10, 25, 5, 30);
		Assert.AreEqual (test.Name, "George");
		Assert.AreEqual (test.Health, 100);
		Assert.AreEqual (test.Attack, 20);
		Assert.AreEqual (test.Defence, 10);
		Assert.AreEqual (test.Magic, 25);
		Assert.AreEqual (test.Luck, 5);
		Assert.AreEqual (test.Speed, 30);
	}

	[Test]
	public void Setters() {
		Fighter test = new Fighter ("George", 100, 20, 10, 25, 5, 30);
		test.Attack = test.Attack + 2;
		test.Defence = test.Defence / 2;
		Assert.AreEqual (test.Attack, 22);
		Assert.AreEqual (test.Defence, 5);
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
