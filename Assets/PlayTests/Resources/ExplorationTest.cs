using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

[TestFixture]
public class ExplorationTest {

	GameObject player;
	GameObject camera;

	public IEnumerator Setup() {
		SceneManager.LoadScene ("ExplorationTest", LoadSceneMode.Single);
		yield return null; //Wait for scene to load
		player = GameObject.Find ("Player");
		camera = GameObject.Find ("PlayerCamera");
	}

	[UnityTest]
	public IEnumerator CameraMovement() {
		yield return Setup ();
		yield return new WaitForSeconds (3);
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
