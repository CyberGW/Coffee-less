using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMenu : MonoBehaviour {

	Player[] players;
	GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
		player.SetActive (false);
		players = PlayerData.instance.data.Players;
		GameObject container;
		GameObject stats;
		for (int i = 0; i < 6; i++) {
			container = GameObject.Find ("Player" + (i + 1) + "/Container");
			if (players [i] != null) {
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
			} else {
				Destroy (container);
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void back() {
		player.SetActive (true);
		SceneChanger.instance.menuOpen = true;
		SceneChanger.instance.loadLevel (SceneChanger.instance.menuScene);
	}
}
