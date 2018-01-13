using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

[TestFixture]
public class MenuTest {

	bool sceneLoaded = false;
	GameObject playableCharacter;
	PlayerMovement movementScript;
	ExplorationMenuScript menuScript;
	ItemsMenuScript itemScript;
	PlayerMenu partyScript;
	GameObject itemsPanel;

	public IEnumerator Setup() {
		if (!sceneLoaded) {
			SceneManager.LoadScene ("ExplorationTestScene", LoadSceneMode.Single);
			yield return null; //Wait for scene to load
			playableCharacter = GameObject.Find ("Player");
			movementScript = playableCharacter.GetComponent<PlayerMovement> ();
			sceneLoaded = true;

			DataManager data = PlayerData.instance.data;
			data.addPlayer (new Player ("Hannah", 5, 100, 5, 5, 5, 5, 5, 5, 0, null,
				new IncreaseMoney ("stole money from", "Increase money returns by 50%", 2, 0.5f),
				new MagicAttack ("threw wine battles at", "Thorw wine bottles with damage 15", 2, 15),
				(Texture2D)Resources.Load ("Character2", typeof(Texture2D))));
			data.addItem (new Hammer ());
			data.addItem (new RabbitFoot ());
		}
		playableCharacter.transform.position = new Vector2 (0, 0);
		yield return null;
	}

	[UnityTest]
	public IEnumerator M0OpenAndCloseMenu() {
		yield return Setup ();

		//Open
		movementScript.pseudoEscapeKeyPress = true;
		yield return WaitForFrames (3);
		Assert.True (SceneManager.GetSceneByName("GameMenu").isLoaded);
		//Check can't move
		yield return moveForFrames(30, "Down");
		Assert.AreEqual (0, playableCharacter.transform.position.y);

		movementScript.pseudoEscapeKeyPress = true;
		yield return WaitForFrames (3);
		Assert.False (SceneManager.GetSceneByName("GameMenu").isLoaded);
		//Check can move
		yield return moveForFrames(30, "Down");
		Assert.AreNotEqual (0, playableCharacter.transform.position.y);
	}

	[UnityTest]
	public IEnumerator M1OpenItemMenu() {
		movementScript.pseudoEscapeKeyPress = true;
		yield return WaitForFrames (3);
		menuScript = GameObject.Find ("MenuCanvas").GetComponent<ExplorationMenuScript> ();
		menuScript.inventPressed ();
		yield return new WaitForSeconds (1);
		itemScript = GameObject.Find ("SwapManager").GetComponent<ItemsMenuScript> ();

		//Should have two items in inventory
		itemsPanel = GameObject.Find("Padding");
		Assert.NotNull (itemsPanel.transform.Find ("Item0/Item"));
		Assert.NotNull (itemsPanel.transform.Find ("Item1/Item"));
		//Rest should be empty
		for (int i = 2; i < 6; i++) {
			Assert.Null (itemsPanel.transform.Find ("Item" + i + "/Item"));
		}

		//Should have two players
		Assert.NotNull(GameObject.Find("Player0"));
		Assert.NotNull(GameObject.Find("Player1"));
		//Rest should not exist
		for (int i = 2; i < 6; i++) {
			Assert.Null (GameObject.Find("Player" + i));
		}
	}

	[Test]
	public void M2SwapItems() {
		Item[] items = PlayerData.instance.data.Items;
		Player[] players = PlayerData.instance.data.Players;

		//Item --> Item (also includes swap check)
		swapItems("Item0", "Item1");
		//Check data updated
		Assert.IsInstanceOf (typeof(RabbitFoot), items [0]);
		Assert.IsInstanceOf (typeof(Hammer), items [1]);

		//Item --> Player
		swapItems("Item1", "Player0/Container/Stats/Item");
		Assert.IsInstanceOf (typeof(Hammer), players[0].Item);
		Assert.Null (items [1]);

		//Player --> Player
		swapItems("Player0/Container/Stats/Item", "Player1/Container/Stats/Item");
		Assert.IsInstanceOf (typeof(Hammer), players[1].Item);
		Assert.Null (players[0].Item);

		//Player --> Item
		swapItems("Player1/Container/Stats/Item", "Item2");
		Assert.IsInstanceOf (typeof(Hammer), items[2]);
		Assert.Null (players [1].Item);
	}

	private void swapItems(string sourceCell, string destCell) {
		DragAndDropCell.DropDescriptor desc = new DragAndDropCell.DropDescriptor ();
		desc.sourceCell = GameObject.Find (sourceCell).GetComponent<DragAndDropCell> ();
		desc.destinationCell = GameObject.Find (destCell).GetComponent<DragAndDropCell> ();
		desc.item = GameObject.Find(sourceCell + "/" + "Item").GetComponent<DragAndDropItem> ();
		itemScript.OnItemPlace (desc);
		desc.sourceCell.SwapItems(desc.destinationCell, desc.sourceCell);
	}

	[UnityTest]
	public IEnumerator M3ItemBackButton() {
		itemScript.back ();
		yield return new WaitForSeconds (1);
		//Check back in previous scene
		Assert.AreEqual ("ExplorationTestScene", SceneManager.GetActiveScene ().name);
		//Check menu is still open
		Assert.True (SceneManager.GetSceneByName ("GameMenu").isLoaded);
	}

	[UnityTest]
	public IEnumerator M4OpenPlayerMenu() {
		menuScript.partyPressed ();
		yield return new WaitForSeconds (1);
		partyScript = GameObject.Find ("PartyCanvas").GetComponent<PlayerMenu> ();
		Debug.Log (partyScript);

		//Should show two players
		Assert.NotNull(GameObject.Find("Player1").GetComponent<DragAndDropCell>());
		Assert.NotNull(GameObject.Find("Player2").GetComponent<DragAndDropCell>());
		//Rest should not be drag and drop cells
		for (int i=3; i < 7; i++) {
			Assert.Null(GameObject.Find("Player" + i).GetComponent<DragAndDropCell>());
		}
	}

	[Test]
	public void M5SwitchPlayers() {
		Player[] players = PlayerData.instance.data.Players;

		//Swap
		swapPlayers ("Player1", "Player2");
		Assert.AreEqual ("Hannah", players [0].Name);
		Assert.AreEqual ("George", players [1].Name);

		//Swap back
		swapPlayers ("Player2", "Player1");
		Assert.AreEqual ("George", players [0].Name);
		Assert.AreEqual ("Hannah", players [1].Name);
	}

	private void swapPlayers(string sourceCell, string destCell) {
		DragAndDropCell.DropDescriptor desc = new DragAndDropCell.DropDescriptor ();
		desc.sourceCell = GameObject.Find (sourceCell).GetComponent<DragAndDropCell> ();
		desc.destinationCell = GameObject.Find (destCell).GetComponent<DragAndDropCell> ();
		desc.item = GameObject.Find(sourceCell + "/" + "Container").GetComponent<DragAndDropItem> ();
		partyScript.OnItemPlace (desc);
		desc.sourceCell.SwapItems(desc.destinationCell, desc.sourceCell);
	}

	[UnityTest]
	public IEnumerator M6PartyBackButton() {
		partyScript.back ();
		yield return new WaitForSeconds (1);
		//Check back in previous scene
		Assert.AreEqual ("ExplorationTestScene", SceneManager.GetActiveScene ().name);
		//Check menu is still open
		Assert.True (SceneManager.GetSceneByName ("GameMenu").isLoaded);
		//Close
		movementScript.pseudoEscapeKeyPress = true;
	}

	public IEnumerator WaitForFrames(int frames) {
		for (int i=0; i < frames; i++) {
			yield return null;
		}
	}

	public IEnumerator moveForFrames(int frames, string direction) {
		for (int i = 0; i < frames; i++) {
			movementScript.move (direction);
			yield return new WaitForFixedUpdate();
		}
	}

}
