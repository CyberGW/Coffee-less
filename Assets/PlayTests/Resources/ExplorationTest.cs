using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

[TestFixture]
public class ExplorationTest {

	GameObject player;
	GameObject camera;

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[SetUp]
	public void Setup() {
		player = MonoBehaviour.Instantiate (Resources.Load<GameObject> ("Player"));
		player.name = "Player";
		camera = MonoBehaviour.Instantiate (Resources.Load<GameObject> ("PlayerCamera"));
	}

	[UnityTest]
	public IEnumerator CameraMovement() {
		//Move Player
		player.transform.position = new Vector2 (50, 50);
		//Check Player has been moved
		Assert.AreEqual (50, player.transform.position.x);
		yield return WaitForFrames(2); //Wait for 2 frames for camera to update
		//Check x-component
		Assert.AreEqual (player.transform.position.x, camera.transform.position.x);
		//Check y-component
		Assert.AreEqual (player.transform.position.y, camera.transform.position.y);
		yield return new WaitForSeconds(3);
	}
		
	public IEnumerator WaitForFrames(int frames) {
		for (int i=0; i < frames; i++) {
			yield return null;
		}
	}
}
