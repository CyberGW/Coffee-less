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
	private GameObject[] itemObjects;

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
		//Add two hammers for testing purposes
		data.addItem (new Hammer());
		data.addItem (new Hammer());
		itemObjects = new GameObject[6];
		GameObject container;
		GameObject stats;
		//Find all cells
		for (int i = 0; i < 6; i++) {
			//Find and store the item and player container
			itemContainers [i] = GameObject.Find ("Item" + i).GetComponent<DragAndDropCell>();
			playerContainers [i] = GameObject.Find ("Player" + i);
			Debug.Log (playerContainers [i]);
			//If there is an item in the item inventory
			if (items[i] != null) {
				//Load an item object in this position to drag and drop
				itemObjects [i] = Instantiate (Resources.Load ("Item", typeof(GameObject))) as GameObject;
				itemObjects [i].GetComponent<ItemData> ().Item = data.Items [i];
				itemObjects [i].transform.Find ("Text").GetComponent<Text> ().text = data.Items [i].Name + " - " + data.Items [i].Desc;
				itemContainers [i].PlaceItem (itemObjects [i]);
			}
			//If there's not a player at this current position
			if (players [i] == null) {
				Destroy (playerContainers [i]);
			} else {
				container = playerContainers [i].transform.Find ("Container").gameObject;
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
					GameObject item = Instantiate (Resources.Load ("Item", typeof(GameObject))) as GameObject;
					item.GetComponent<ItemData> ().Item = players [i].Item;
					item.transform.Find ("Text").GetComponent<Text> ().text = data.Items [i].Name + " - " + data.Items [i].Desc;
					itemCell.PlaceItem (item);
					//Disabled the container from having items dragged into it and set to grey to indicate this
					//playerContainers [i].enabled = false;
					//playerContainers [i].GetComponent<Image> ().color = Color.grey;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// On the event an item is placed, swap the values in the appropiate arrays
	/// </summary>
	/// <param name="desc">The description of the event, containing source and destination cells as well
	/// as item details</param>
	void OnItemPlace(DragAndDropCell.DropDescriptor desc) {
		ContainerData source = desc.sourceCell.gameObject.GetComponent<ContainerData> ();
		ContainerData dest = desc.destinationCell.gameObject.GetComponent<ContainerData> ();
		//If moving to a new item slot
		if (dest.Type == "Item") {
			items [dest.Index] = items [source.Index]; //Assign item to new slot
		}
		//If equipping to a player
		if (dest.Type == "Player") {
			PlayerData.instance.data.Players [dest.Index].Item = items [source.Index]; //Change that player's item accordingly
		}
		//If coming from an item slot
		if (source.Type == "Item") {
			items [source.Index] = null; //Remove from it's original location
		}
		if (source.Type == "Player") {
			PlayerData.instance.data.Players [source.Index].Item = null;
		}
		Debug.Log ("Source: " + source.Type);
		Debug.Log ("Destination: " + dest.Type);
		Debug.Log ("Item Name: " + desc.item.GetComponent<ItemData> ().Item.Name);
	}

	public void back() {
		player.SetActive (true);
		SceneChanger.instance.menuOpen = true;
		SceneChanger.instance.loadLevel (SceneChanger.instance.menuScene);
	}
}
