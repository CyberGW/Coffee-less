using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

[TestFixture]
public class ExplorationTest {

	GameObject player;
	PlayerMovement playerScript;
	GameObject camera;

	public IEnumerator Setup() {
		SceneManager.LoadScene ("ExplorationTestScene", LoadSceneMode.Single);
		yield return null; //Wait for scene to load
		player = GameObject.Find ("Player");
		playerScript = player.GetComponent<PlayerMovement> ();
		camera = GameObject.Find ("PlayerCamera");
		player.transform.position = new Vector2 (0, 0);
	}

	[UnityTest]
	public IEnumerator CameraMovement() {
		yield return Setup ();
		//Move Player
		player.transform.position = new Vector2 (5, 5);
		//Check Player has been moved
		Assert.AreEqual (5, player.transform.position.x);
		yield return WaitForFrames(2); //Wait for 2 frames for camera to update
		//Check x-component
		Assert.AreEqual (player.transform.position.x, camera.transform.position.x);
		//Check y-component
		Assert.AreEqual (player.transform.position.y, camera.transform.position.y);
	}

	[UnityTest]
	public IEnumerator PlayerMovement() {
		//Reset
		yield return Setup ();
		//Up
		yield return moveForFrames(60, "Up");
		Assert.AreEqual (0.1 * 60, Math.Round(player.transform.position.y, 2));
		//Down - Return to starting position
		yield return moveForFrames(60, "Down");
		Assert.AreEqual (0, Math.Round(player.transform.position.y, 2));
		//Left
		yield return moveForFrames(60, "Right");
		Assert.AreEqual (0.1 * 60, Math.Round(player.transform.position.x, 2));
		//Right - Return to starting position
		yield return moveForFrames(60, "Left");
		Assert.AreEqual (0, Math.Round(player.transform.position.y, 2));
	}

	[UnityTest]
	public IEnumerator ObjectCollision() {
		//Reset
		yield return Setup ();
		//Should move 0.1 * 120 = 12 units but square at 9 units
		yield return moveForFrames(120, "Right");
		//Therefore should not go past 9 units
		Assert.Less(player.transform.position.x, 9);
	}

	[UnityTest]
	public IEnumerator DialogueBox() {
		//Setup
		yield return Setup ();
		ObjectDialogue objectScript = GameObject.Find ("Triangle").GetComponentInChildren<ObjectDialogue> ();
		DialogueScript dialogueScript = GameObject.Find ("Dialogue Manager").GetComponent<DialogueScript> ();

		//Check dialogue box can't be triggered when out of range
		objectScript.pseudoKeyPress = true;
		yield return WaitForFrames(2); //Wait a frame
		Assert.IsNull (GameObject.Find ("DialogueBox")); //Check DialogueBox cannot be found (is inactive)

		//Check it can be triggered within range
		player.transform.position = new Vector2(3, 3);
		objectScript.pseudoKeyPress = true;
		yield return WaitForFrames (2); //Wait 2 frames for player to move then trigger to be detected
		Assert.IsNotNull (GameObject.Find ("DialogueBox")); //Check DialogueBox can be found (is active)

		UnityEngine.UI.Text dialogueText = GameObject.Find ("DialogueBox").GetComponentInChildren<UnityEngine.UI.Text> ();
		string firstText = dialogueText.text;

		//Check multi-line dialogue advances
		dialogueScript.pseudoKeyPress = true;
		yield return WaitForFrames (2);
		Assert.IsNotNull (GameObject.Find ("DialogueBox")); //Check DialogueBox is still active
		Assert.AreNotEqual(firstText, dialogueText.text); //Check text has changed

		//Check DialogueBox closes again
		dialogueScript.pseudoKeyPress = true;
		yield return WaitForFrames (2);
		Assert.IsNull (GameObject.Find ("DialogueBox"));
	}

	public IEnumerator moveForFrames(int frames, string direction) {
		for (int i = 0; i < frames; i++) {
			playerScript.move (direction);
			yield return new WaitForFixedUpdate();
		}
	}

		
	public IEnumerator WaitForFrames(int frames) {
		for (int i=0; i < frames; i++) {
			yield return null;
		}
	}
}
