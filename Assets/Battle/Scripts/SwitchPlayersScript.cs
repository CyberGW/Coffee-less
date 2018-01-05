using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SwitchPlayersScript : MonoBehaviour {

	Player[] players;
	MainBattle mainBattle;

	// Use this for initialization
	void Start () {
		players = PlayerData.instance.data.Players;
		mainBattle = GameObject.Find ("BattleCode").GetComponent<MainBattle> ();
		GameObject cell;
		GameObject container;
		GameObject stats;
		Texture2D image;
		for (int i = 0; i < 6; i++) {
			cell = GameObject.Find ("Player" + (i + 1));
			container = cell.transform.Find ("Container").gameObject;
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
				if (players [i].Health == 0) {
					container.GetComponent<Image> ().color = Color.grey;
					Destroy (cell.GetComponent<Button> ());
				}
			} else {
				//Destroy all children
				var children = new List<GameObject>();
				foreach (Transform child in container.transform) children.Add(child.gameObject);
				children.ForEach(child => Destroy(child));
				container.GetComponent<Image> ().color = Color.grey;
				Destroy (cell.GetComponent<Button> ());
			}
		}
		//Disable attack button if opened after player has died
		if (!GlobalFunctions.instance.playerDied) {
			GameObject.Find ("BackButton").GetComponent<Button> ().interactable = false;
		}

	}

	public void switchPlayers(int player) {
		mainBattle.switchPlayers (0);
		SceneManager.UnloadSceneAsync (SceneManager.GetSceneByName ("SwitchPlayer").buildIndex);
	}
		

	// Update is called once per frame
	void Update () {
		
	}

	public void back() {
		SceneManager.UnloadSceneAsync (SceneManager.GetSceneByName ("SwitchPlayer").buildIndex);
	}
}
