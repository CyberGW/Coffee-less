using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

[TestFixture]
public class WorldMapTest {

	GameObject player;
	PlayerMovement playerScript;

	public IEnumerator Setup() {
		SceneManager.LoadScene ("WorldMap");
		yield return null;

		//Change current level then reload
		GlobalFunctions.instance.currentLevel = 1;
		SceneManager.LoadScene ("WorldMap");
		yield return null;
		player = GameObject.Find ("Player");
		playerScript = player.GetComponent<PlayerMovement> ();

	}

	[UnityTest]
	public IEnumerator W0DepartmentColouring() {
		//Setup
		yield return Setup ();

		//Check completed regions are coloured red
		MeshRenderer CS = GameObject.FindGameObjectWithTag ("CS").transform.Find ("CS").GetComponent<MeshRenderer> ();
		Assert.AreEqual (Color.red, CS.material.color);
		//Check next region is coloured green
		MeshRenderer TFTV = GameObject.FindGameObjectWithTag ("TFTV").transform.Find ("TFTV").GetComponent<MeshRenderer> ();
		Assert.AreEqual (Color.green, TFTV.material.color);
		//Check future departments are uncoloured
		MeshRenderer RCH = GameObject.FindGameObjectWithTag ("RCH").transform.Find ("RCH").GetComponent<MeshRenderer> ();
		Assert.AreEqual (Color.grey, RCH.material.color);
	}

	[UnityTest]
	public IEnumerator W1CantEnterPreviousDepartments() {
		yield return moveForFrames (20, "Left"); //Walk to beaten Computer Science building
		Assert.AreEqual("WorldMap", SceneManager.GetActiveScene().name); //Check active scene is still WorldMap
		// e.g. not entered CS
	}

	[UnityTest]
	public IEnumerator W2CantEnterFutureDepartments() {
		player.transform.position = new Vector2 (1,2); //Line up with RCH building
		yield return null;
		yield return moveForFrames (20, "Right");
		Assert.AreEqual("WorldMap", SceneManager.GetActiveScene().name); //Check active scene is still WorldMap
	}

	[UnityTest]
	public IEnumerator W3BusStop() {
		player.transform.position = new Vector2 (-13,-8); //Line up with bus stop
		yield return moveForFrames(20, "Down");
		yield return new WaitForSeconds (1); //Wait for transition
		//Check on Hes West site now
		Assert.AreEqual(-46, (int) player.transform.position.x);
		Assert.AreEqual(-3, (int) player.transform.position.y);
		yield return moveForFrames(20, "Down");
		yield return new WaitForSeconds (1); //Wait for transition
		//Check back on Hes East site now
		Assert.AreEqual(-13, Mathf.RoundToInt(player.transform.position.x) );
		Assert.AreEqual(-7, Mathf.RoundToInt(player.transform.position.y) );
	}

	[UnityTest]
	public IEnumerator W4CanEnterAndReturnFromCurrentLevel() {
		player.transform.position = new Vector2 (-10,-1); //Line up with TFTV
		yield return moveForFrames(20, "Left");
		yield return new WaitForSeconds (1); //Wait for transition
		Assert.AreEqual("TFTV-Ground", SceneManager.GetActiveScene().name); //Check in location now
		yield return moveForFrames(20, "Down");
		yield return new WaitForSeconds (1); //Wait for transition
		Assert.AreEqual("WorldMap", SceneManager.GetActiveScene().name); //Check back on World Map
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
