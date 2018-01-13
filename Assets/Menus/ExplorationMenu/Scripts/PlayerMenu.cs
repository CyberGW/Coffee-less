using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script for controlling the menu which allows users to switch the order of their team members 
/// </summary>
public class PlayerMenu : MonoBehaviour {

	Player[] players;
	GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
		player.SetActive (false);
		players = PlayerData.instance.data.Players;
		GameObject cell;
		GameObject container;
		GameObject stats;
		Texture2D image;

		//Loop through all players
		for (int i = 0; i < 6; i++) {
			cell = GameObject.Find ("Player" + (i + 1));
			container = cell.transform.Find ("Container").gameObject;

			//If player exists
			if (players [i] != null) {
				image = players [i].Image;
				container.transform.Find("Image").GetComponent<Image>().sprite = 
					Sprite.Create (image, new Rect (0.0f, 0.0f, image.width, image.height), new Vector2 (0.5f, 0.5f));
				container.transform.Find ("Name").GetComponent<Text> ().text = players [i].Name;
				stats = container.transform.Find ("Stats").gameObject;
				stats.transform.Find ("Level").GetComponent<Text> ().text = "Level: " + players [i].Level.ToString ();
				stats.transform.Find ("Health").GetComponent<Text> ().text = "Health: " + players [i].Health.ToString () + " / 100";
				stats.transform.Find ("Attack").GetComponent<Text> ().text = "Attack: " + players [i].Attack.ToString ();
				stats.transform.Find ("Defence").GetComponent<Text> ().text = "Defence: " + players [i].Defence.ToString ();
				stats.transform.Find ("Magic").GetComponent<Text> ().text = "Magic: " + players [i].Magic.ToString () + " / "
					+ players[i].MaximumMagic.ToString();
				stats.transform.Find ("Luck").GetComponent<Text> ().text = "Luck: " + players [i].Luck.ToString ();
				stats.transform.Find ("Speed").GetComponent<Text> ().text = "Speed: " + players [i].Speed.ToString ();
				stats.transform.Find ("Exp").GetComponent<Text> ().text = "Exp: " + players [i].Exp.ToString () + " / "
					+ players[i].ExpToNextLevel.ToString();

			//if player doesn't exits
			} else {
				//Destroy all children
				var children = new List<GameObject>();
				foreach (Transform child in container.transform) children.Add(child.gameObject);
				children.ForEach(child => Destroy(child));
				container.GetComponent<Image> ().color = Color.grey;
				Destroy (cell.GetComponent<DragAndDropCell> ());
			}
		}

	}

	/// <summary>
	/// On the OnItemPlace event called by DragAndDropScript, call <see cref="DataManager.swapPlayers"/> to
	/// swap the two players in the array 
	/// </summary>
	/// <param name="desc">Desc.</param>
	public void OnItemPlace(DragAndDropCell.DropDescriptor desc) {
		ContainerData source = desc.sourceCell.gameObject.GetComponent<ContainerData> ();
		ContainerData dest = desc.destinationCell.gameObject.GetComponent<ContainerData> ();
		PlayerData.instance.data.swapPlayers (source.index, dest.index);
		Debug.Log ("Source: " + source.Type);
		Debug.Log ("Destination: " + dest.Type);
	}

	/// <summary>
	/// When the back button is pressed, show player again and reload the previous scene, ensuring that the
	/// exploration menu is shown
	/// </summary>
	public void back() {
		player.SetActive (true);
		SceneChanger.instance.menuOpen = true;
		SceneChanger.instance.loadLevel (SceneChanger.instance.menuScene);
	}
}
