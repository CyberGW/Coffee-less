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
	GameObject itemsPanel;

	public IEnumerator Setup() {
		if (!sceneLoaded) {
			SceneManager.LoadScene ("MenuTestScene", LoadSceneMode.Single);
			yield return null; //Wait for scene to load
			playableCharacter = GameObject.Find ("Player");
			movementScript = playableCharacter.GetComponent<PlayerMovement> ();
			sceneLoaded = true;
		}
		playableCharacter.transform.position = new Vector2 (0, 0);
		yield return null;
	}

	[UnityTest]
	public IEnumerator A1OpenAndCloseMenu() {
		yield return Setup ();
		movementScript.pseudoEscapeKeyPress = true;
		yield return WaitForFrames (3);
		Assert.True (SceneManager.GetSceneByName("GameMenu").isLoaded);
		movementScript.pseudoEscapeKeyPress = true;
		yield return WaitForFrames (3);
		Assert.False (SceneManager.GetSceneByName("GameMenu").isLoaded);
	}

	[UnityTest]
	public IEnumerator A2OpenItemMenu() {
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
		//Should have two players
		Assert.NotNull(GameObject.Find("Player0"));
		Assert.NotNull(GameObject.Find("Player1"));
	}

	[Test]
	public void A3SwapItems() {
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
	public IEnumerator A4ItemBackButton() {
		itemScript.back ();
		yield return new WaitForSeconds (1);
		//Check back in previous scene
		Assert.AreEqual ("MenuTestScene", SceneManager.GetActiveScene ().name);
		//Check menu is still open
		Assert.True (SceneManager.GetSceneByName ("GameMenu").isLoaded);
	}

	public IEnumerator WaitForFrames(int frames) {
		for (int i=0; i < frames; i++) {
			yield return null;
		}
	}

}
