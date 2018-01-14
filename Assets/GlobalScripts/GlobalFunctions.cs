using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
	/// <summary>Holds whether a battle to be started can be run from or not</summary>
	[System.NonSerialized]
	public bool canRunAway;
	/// <summary>Refers to the player object so it can be set active or inactive</summary>
	private GameObject player;
	/// <summary>Refers to the scene a battle was initiated from so it can be returned to afterwards</summary>
	private string previousScene;
	/// <summary>Refers to the music playing before the battle was initiated so it can be resumed afterwards</summary>
	private AudioClip previousBGM;
	/// <summary>
	/// Stores an objects unique ID and whether they should be active or not, as to determine whether to display
	/// it </summary>
	public IDictionary<string, bool> objectsActive;
	/// <summary>The current level that the player is up to</summary>
	[System.NonSerialized]
	public int currentLevel;


	/// <summary>
	/// An enum type representing items so that they can be selected from within the Unity Editor
	/// </summary>
	public enum ItemTypes { None, Hammer, Trainers, RabbitFoot, MagicAmulet, Shield, Armour };
	[System.NonSerialized]
	public string[] levelOrder = new string[] { "CS", "TFTV", "RCH", "PHY", "LIB" };

	void Awake() {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
			objectsActive = new Dictionary <string, bool> ();
			currentLevel = 0;
		} else if (instance != this) {
			Destroy (gameObject);
		}
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 60;

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
	public void createBattle(Enemy enemy, int money, Item item, bool canRunAway) {
		this.enemy = enemy;
		this.money = money;
		this.item = item;
		this.canRunAway = canRunAway;
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

	/// <summary>
	/// Converts an enum type of <see cref="ItemTypes"/> to an <see cref="Item"/> instance </summary>
	/// <returns>An item instance</returns>
	/// <param name="itemType">The type of item to create</param>
	public Item createItem (ItemTypes? itemType) {
		Item item;
		switch (itemType)
		{
		case ItemTypes.Hammer:
			item = new Hammer();
			break;
		case ItemTypes.Trainers:
			item = new Trainers ();
			break;
		case ItemTypes.RabbitFoot:
			item = new RabbitFoot ();
			break;
		case ItemTypes.MagicAmulet:
			item = new MagicAmulet ();
			break;
		case ItemTypes.Shield:
			item = new Shield ();
			break;
		case ItemTypes.Armour:
			item = new Armour ();
			break;
		default:
			item = null;
			break;
		}
		return item;
	}
}
