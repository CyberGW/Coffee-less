using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Script to handle the menu allowing users to switch players in and out of battle
/// </summary>
public class SwitchPlayersScript : MonoBehaviour {

	Player[] players;
	MainBattle mainBattle;

	/// <summary>
	/// On start, show all existing players and their stats. Also disable back button if entered
	/// after the current player has just died
	/// </summary>
	void Start () {
		players = PlayerData.instance.data.Players;
		mainBattle = GameObject.Find ("BattleCode").GetComponent<MainBattle> ();
		GameObject cell;
		GameObject container;
		GameObject stats;
		Texture2D image;

		//Loop through all players
		for (int i = 0; i < 6; i++) {
			cell = GameObject.Find ("Player" + (i + 1));
			container = cell.transform.Find ("Container").gameObject;

			//If player exits
			if (players [i] != null) {

				//Setup all sprites and player stats display
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

			//If player doesn't exists
			} else {
				//Destroy all children, leaving the container empty
				var children = new List<GameObject>();
				foreach (Transform child in container.transform) children.Add(child.gameObject);
				children.ForEach(child => Destroy(child));
				container.GetComponent<Image> ().color = Color.grey;
				Destroy (cell.GetComponent<Button> ()); //Remove button component so can't be pressed
			}
		}
			
		//Disable attack button if opened after player has died
		if (mainBattle.playerDied) {
			GameObject.Find ("BackButton").GetComponent<Button> ().interactable = false;
		}

	}

	/// <summary>
	/// Once a player has been selected, call <see cref="MainBattle.switchPlayers"/> and unload this menu scene  
	/// </summary>
	/// <param name="player">Player.</param>
	public void switchPlayers(int player) {
		mainBattle.switchPlayers (player);
		SceneManager.UnloadSceneAsync (SceneManager.GetSceneByName ("SwitchPlayer").buildIndex);
	}

	/// <summary>
	/// When back button is pressed, unload this menu scene
	/// </summary>
	public void back() {
		SceneManager.UnloadSceneAsync (SceneManager.GetSceneByName ("SwitchPlayer").buildIndex);
	}
}
