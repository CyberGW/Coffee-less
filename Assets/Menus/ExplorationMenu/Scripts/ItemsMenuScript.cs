using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls all initialisation of menu and updates arrays when an item is moved
/// </summary>
public class ItemsMenuScript : MonoBehaviour {

	private GameObject player;
	/// <summary>Refer to all the containers from the item inventory</summary>
	private DragAndDropCell[] itemContainers;
	/// <summary>Refer to all the containers representing the players</summary>
	private GameObject[] playerContainers;
	private DataManager data;
	private Item[] items;
	private Player[] players;

	/// <summary>
	/// Initialises variables and creates item objects as required
	/// </summary>
	void Start () {
		player = GameObject.Find ("Player");
		player.SetActive (false);
		itemContainers = new DragAndDropCell[6];
		playerContainers = new GameObject[6];
		data = PlayerData.instance.data;
		items = data.Items;
		players = data.Players;
		GameObject container;
		GameObject stats;
		Texture2D image;
		//Find all cells
		for (int i = 0; i < 6; i++) {
			//Find and store the item and player container
			itemContainers [i] = GameObject.Find ("Item" + i).GetComponent<DragAndDropCell>();
			playerContainers [i] = GameObject.Find ("Player" + i);
			//If there is an item in the item inventory
			if (items[i] != null) {
				//Load an item object in this position to drag and drop
				createItemCell(itemContainers[i], data.Items[i]);
			}
			//If there's not a player at this current position
			if (players [i] == null) {
				Destroy (playerContainers [i]);
			} else {
				container = playerContainers [i].transform.Find ("Container").gameObject;
				image = players [i].Image;
				container.transform.Find("Image").GetComponent<Image>().sprite = 
					Sprite.Create (image, new Rect (0.0f, 0.0f, image.width, image.height), new Vector2 (0.5f, 0.5f));
				container.transform.Find ("Name").GetComponent<Text> ().text = players [i].Name;
				stats = container.transform.Find ("Stats").gameObject;
				stats.transform.Find ("Attack").GetComponent<Text> ().text = "Attack: " + players [i].Attack.ToString ();
				stats.transform.Find ("Defence").GetComponent<Text> ().text = "Defence: " + players [i].Defence.ToString ();
				stats.transform.Find ("Magic").GetComponent<Text> ().text = "Magic: " + players [i].Magic.ToString () + " / "
					+ players[i].MaximumMagic.ToString();
				stats.transform.Find ("Luck").GetComponent<Text> ().text = "Luck: " + players [i].Luck.ToString ();
				stats.transform.Find ("Speed").GetComponent<Text> ().text = "Speed: " + players [i].Speed.ToString ();
				if (players [i].Item != null) {
					DragAndDropCell itemCell = playerContainers [i].transform.Find ("Container/Stats/Item").GetComponent<DragAndDropCell> ();
					createItemCell (itemCell, players [i].Item);
					//Disabled the container from having items dragged into it and set to grey to indicate this
					//playerContainers [i].enabled = false;
					//playerContainers [i].GetComponent<Image> ().color = Color.grey;
				}
			}
		}
	}

	/// <summary>
	/// Creates an item cell, showing the name and description
	/// </summary>
	/// <param name="cell">Cell.</param>
	/// <param name="itemObject">Item object.</param>
	private void createItemCell(DragAndDropCell cell, Item itemObject) {
		GameObject item = Instantiate (Resources.Load ("Item", typeof(GameObject))) as GameObject;
		item.name = "Item";
		item.transform.Find ("Text").GetComponent<Text> ().text = itemObject.Name + " - " + itemObject.Desc;
		cell.PlaceItem (item);
	}

	/// <summary>
	/// On the event an item is placed, swap the values in the appropiate arrays
	/// </summary>
	/// <param name="desc">The description of the event, containing source and destination cells as well
	/// as item details</param>
	public void OnItemPlace(DragAndDropCell.DropDescriptor desc) {
		ContainerData source = desc.sourceCell.gameObject.GetComponent<ContainerData> ();
		ContainerData dest = desc.destinationCell.gameObject.GetComponent<ContainerData> ();
		Player[] players = data.Players;
		Item temp;
		if (dest.type == "Item") {
			if (source.type == "Item") {
				temp = items [source.Index];
				items [source.Index] = items [dest.Index];
				items [dest.Index] = temp;
			} else { //if source.type == "Player"
				temp = players [source.Index].Item;
				players [source.Index].Item = items [dest.Index];
				items [dest.Index] = temp;
			}
		} else { //if dest.type == "Player"
			if (source.type == "Item") {
				temp = items [source.Index];
				items [source.Index] = players [dest.Index].Item;
				players [dest.Index].Item = temp;
			} else { //if source.type == "Player"
				temp = players [source.Index].Item;
				players [source.Index].Item = players [dest.Index].Item;
				players [dest.Index].Item = temp;
			}
		}
		Debug.Log ("Source: " + source.Type);
		Debug.Log ("Destination: " + dest.Type);
		//Debug.Log ("Item Name: " + desc.item.GetComponent<ItemData> ().Item.Name);
	}

	/// <summary>
	/// When the back button is pressed, reshow player and go back to the scene where the menu was called from,
	/// ensuring the exploration menu is shown
	/// </summary>
	public void back() {
		player.SetActive (true);
		SceneChanger.instance.menuOpen = true;
		SceneChanger.instance.loadLevel (SceneChanger.instance.menuScene);
	}
}
