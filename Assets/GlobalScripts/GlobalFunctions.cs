using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides functions and variables to be accessed by any game object to allow data to be passed between scenes
/// </summary>
public class GlobalFunctions : MonoBehaviour {

	/// <summary>
	/// References the current instance so variables and functions can be called by <c> GlobalFunctions.instance...</c>
	/// </summary>
	public static GlobalFunctions instance = null;
	/// <summary>Holds an enemy object for the battle scene to load from</summary>
	private Enemy enemy;
	/// <summary>Holds an amount of money for the battle scene to load from</summary>
	private int money;
	/// <summary>Holds an item for the battle scene to load from</summary>
	private Item item;
	/// <summary>Refers to the player object so it can be set active or inactive</summary>
	private GameObject player;
	/// <summary>Refers to the scene a battle was initiated from so it can be returned to afterwards</summary>
	private string previousScene;
	/// <summary>Refers to the music playing before the battle was initiated so it can be resumed afterwards</summary>
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

	/// <summary>
	/// Starts a battle, setting <see cref="previousScene"/> and <see cref="previousBGM"/> and making the <see cref="player"/>
	/// object inactive 
	/// </summary>
	/// <param name="enemy">The enemy object to battle against</param>
	/// <param name="money">The monetary reward if the battle is won</param>
	/// <param name="item">The item rewards if the battle is won, may be <c>null</c></param>
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

	/// <summary>
	/// Ends the battle, loading the <see cref="previousScene"/>, resuming <see cref="previousBGM"/> and setting the
	/// <see cref="player"/> object to active again     
	/// </summary>
	public void endBattle() {
		SoundManager.instance.playBGM (GlobalFunctions.instance.previousBGM);
		SceneChanger.instance.loadLevel (GlobalFunctions.instance.previousScene);
		player.SetActive (true);
	}
}
