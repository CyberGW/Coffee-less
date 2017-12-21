using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

[TestFixture]
public class ExplorationTest {

	bool sceneLoaded = false;
	GameObject player;
	PlayerMovement playerScript;
	GameObject camera;

	public IEnumerator Setup() {
		if (!sceneLoaded) {
			SceneManager.LoadScene ("ExplorationTestScene", LoadSceneMode.Single);
			yield return null; //Wait for scene to load
			player = GameObject.Find ("Player");
			playerScript = player.GetComponent<PlayerMovement> ();
			camera = GameObject.Find ("PlayerCamera");
			sceneLoaded = true;
		}
		player.transform.position = new Vector2 (0, 0);
		yield return null;
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
	public IEnumerator PlayerAnimation() {
		//Reset
		yield return Setup ();
		//Get Animator object
		Animator anim = player.GetComponent<Animator> ();

		//Up
		playerScript.move("Up");
		yield return null; //Wait one frame to update to walking
		Assert.AreEqual("UpWalk", getCurrentClipName(anim));
		yield return WaitForFrames(2); //Wait two frames to reset back to idle
		Assert.AreEqual("UpIdle", getCurrentClipName(anim));

		//Down
		playerScript.move("Down");
		yield return null;
		Assert.AreEqual("DownWalk", getCurrentClipName(anim));
		yield return WaitForFrames(2);
		Assert.AreEqual("DownIdle", getCurrentClipName(anim));

		//Right
		playerScript.move("Right");
		yield return null;
		Assert.AreEqual("RightWalk", getCurrentClipName(anim));
		yield return WaitForFrames(2);
		Assert.AreEqual("RightIdle", getCurrentClipName(anim));

		//Up
		playerScript.move("Up");
		yield return null;
		Assert.AreEqual("UpWalk", getCurrentClipName(anim));
		yield return WaitForFrames(2);
		Assert.AreEqual("UpIdle", getCurrentClipName(anim));
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
		player.transform.position = new Vector2(2.5f, 3);
		objectScript.pseudoKeyPress = true;
		yield return WaitForFrames (3); //Wait 3 frames for player to move then trigger to be detected and input processed
		Assert.IsNotNull (GameObject.Find ("DialogueBox")); //Check DialogueBox can be found (is active)
		//Get current text so we can check if it changes later
		UnityEngine.UI.Text dialogueText = GameObject.Find ("DialogueBox").GetComponentInChildren<UnityEngine.UI.Text> ();
		string firstText = dialogueText.text;

		//Check can't move when dialogue box open
		Vector3 originalPosition = player.transform.position;
		yield return moveForFrames(60, "Right");
		Assert.AreEqual (originalPosition, player.transform.position);

		//Check multi-line dialogue advances
		dialogueScript.pseudoKeyPress = true;
		yield return WaitForFrames (3);
		Assert.IsNotNull (GameObject.Find ("DialogueBox")); //Check DialogueBox is still active
		Assert.AreNotEqual(firstText, dialogueText.text); //Check text has changed

		//Check DialogueBox closes again
		dialogueScript.pseudoKeyPress = true;
		yield return WaitForFrames (3);
		Assert.IsNull (GameObject.Find ("DialogueBox"));
	}

	[UnityTest]
	public IEnumerator ItemObtain() {
		//Setup
		yield return Setup ();
		ObjectDialogue objectScript = GameObject.Find ("TriangleChest").GetComponentInChildren<ObjectDialogue> ();
		DialogueScript dialogueScript = GameObject.Find ("Dialogue Manager").GetComponent<DialogueScript> ();
		DataManager data = GameObject.Find ("GlobalData").GetComponent<PlayerData> ().data;
		Item[] items;

		//Initially no items
		items = data.Items;
		Assert.AreEqual (0, data.countItems());

		//Open dialogue
		player.transform.position = new Vector2 (-2.75f, 3);
		objectScript.pseudoKeyPress = true;
		yield return WaitForFrames (3); //Wait 3 frames for player to move then trigger to be detected and input processed
		Assert.IsNotNull (GameObject.Find ("DialogueBox")); //Check DialogueBox can be found (is active)

		//Close dialogue
		dialogueScript.pseudoKeyPress = true;
		yield return WaitForFrames (3);
		Assert.Null (GameObject.Find ("DialogueBox"));

		//Check item has been added
		items = data.Items;
		Assert.AreEqual (1, data.countItems()); //Check item has been added
		Assert.IsInstanceOf(typeof(Hammer), items[0]); //Check it's the correct hammer item

		//Check trigger has been destroyed
		Assert.Null(GameObject.Find("TriangleChest").transform.Find("dialogueBox"));
	}

	[UnityTest]
	public IEnumerator Portal() {
		//Reset
		yield return Setup ();
		//Get Initial Scene
		string originalScene = SceneManager.GetActiveScene().name;
		//Move 4 units right = 4 / 0.1 = 40 frames
		yield return moveForFrames(40, "Right");
		//Move 3 units down = 3 / 0.1 = 30 frames
		yield return moveForFrames(30, "Down");
		yield return new WaitForSeconds (1); //Wait for transition animation
		Assert.AreNotEqual (originalScene, SceneManager.GetActiveScene().name);
	}

	public IEnumerator moveForFrames(int frames, string direction) {
		for (int i = 0; i < frames; i++) {
			playerScript.move (direction);
			yield return new WaitForFixedUpdate();
		}
	}

	public string getCurrentClipName(Animator anim) {
		AnimatorClipInfo[] info = anim.GetCurrentAnimatorClipInfo (0);
		return info[0].clip.name;
	}

		
	public IEnumerator WaitForFrames(int frames) {
		for (int i=0; i < frames; i++) {
			yield return null;
		}
	}
}
