using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalFunctions : MonoBehaviour {

	public static GlobalFunctions instance = null;
	private Enemy enemy;
	private int money;
	private Item item;
	private GameObject player;
	private string previousScene;
	private AudioClip previousBGM;

	void Awake() {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		} else if (instance != this) {
			Destroy (gameObject);
		}

		//Set References
		player = GameObject.Find ("Player").gameObject;
	}

	public void createBattle(Enemy enemy, int money, Item item) {
		this.enemy = enemy;
		this.money = money;
		this.item = item;
		previousScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
		previousBGM = SoundManager.instance.BGMSource.clip;
		player.SetActive (false);
		SceneChanger.instance.loadLevel("Battle");
	}

	public Enemy getEnemy() {
		return enemy;
	}

	public int getMoney() {
		return money;
	}

	public Item getItem() {
		return item;
	}		

	public void endBattle() {
		SoundManager.instance.playBGM (GlobalFunctions.instance.previousBGM);
		SceneChanger.instance.loadLevel (GlobalFunctions.instance.previousScene);
		player.SetActive (true);
	}
}
