 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Defines a playable character for use in battle scenes</summary>
public class Player : Character {

	/// <summary>The amount of exp points gained in current level</summary>
	private int exp;
	/// <summary>The number of exp points required to reach next level</summary>
	private int expToNextLevel;
	/// <summary>The item that the player has equipped</summary>
	private Item item;

	public Player (string name, int level, int health, int attack, int defence, int maximumMagic, int magic,
		int luck, int speed, int exp, Item item, SpecialMove special1, SpecialMove special2, Texture2D image = null)
		   : base(name, level, health, attack, defence, maximumMagic, magic, luck, speed, special1, special2, image)
	{
		this.exp = exp;
		setExpToNextLevel ();
		this.item = item;
	}

	public int Exp {
		get {
			return this.exp;
		}
		set {
			exp = value;
		}
	}

	/// <summary>
	/// Gets or sets the item. If setting, <see cref="Item.applyBuffs"/> is called immediately on this player.
	/// If the item wasn't null beforehand, <see cref="Item.revertBuffs"/> is also called beforehand  
	/// </summary>
	/// <value>The item.</value>
	public Item Item {
		get {
			return this.item;
		}
		set {
			if (item != null) {
				item.revertBuffs (this);
			}
			item = value;
			if (item != null) {
				value.applyBuffs (this);
			}
		}
	}

	public int ExpToNextLevel {
		get {
			return this.expToNextLevel;
		}
	}

	/// <summary>
	/// Recursive function to gain experience points.
	/// If enough exp to level up, call <see cref="levelUp"/> and recurse on remaining exp after this level up.
	/// If not enough exp to level up, simply add this to <see cref="exp"/>   
	/// </summary>
	/// <param name="gainedExp">The remaining amount of exp to gain</param>
	public void gainExp(int gainedExp) {
		int difference = expToNextLevel - exp;
		if (difference <= gainedExp) {
			levelUp ();
			gainExp (gainedExp - difference);
		} else {
			exp += gainedExp;
		}
	}

	/// <summary>
	/// Increases all stats by 2, while increasing level by 1, recalculating <see cref="setExpToNextLevel"/> and setting current exp back to 0
	/// </summary>
	public void levelUp() {
		Debug.Log ("Levelled Up!");
		level += 1;
		attack += 2;
		defence += 2;
		magic += 2;
		maximumMagic += 2;
		luck += 2;
		speed += 2;
		exp = 0;
		setExpToNextLevel ();
	}

	/// <summary> Defines the exp points to the next level as current <see cref="level"/> * 200</summary>
	private void setExpToNextLevel() {
		expToNextLevel = level * 200;
	}

}
