using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A component that be added to an object to describe the parameters of a battle from within the Unity Editor.
/// Can be used for random encounters and through <see cref="ObjectInteraction"/> 
/// </summary>
public class BattleDescriptor : MonoBehaviour {

	[Header("Battle Variables")]
	public int battleMoney = 0;
	public GlobalFunctions.ItemTypes battleItem = GlobalFunctions.ItemTypes.None;
	public bool canRunAway = false;

	[Header("Battle Enemy Stats")]
	new public string name = "";
	public Texture2D sprite;
	public int level = 0;
	public int health = 100;
	public int attack = 0;
	public int defence = 0;
	public int maximumMagic = 0;
	public int luck = 0;
	public int speed = 0;
	/// <summary>
	/// An enum type representing special moves so they can be selected from within the Unity Editor
	/// </summary>
	public enum EnemyMoves { MagicAttack, LowerDefence, LowerSpeed, RaiseAttack, RaiseDefence };
	[Header("Battle Enemy Special Moves")]
	public EnemyMoves special1;
	public string special1Text;
	public int special1Magic;
	public float special1Value;
	public EnemyMoves special2;
	public string special2Text;
	public int special2Magic;
	public float special2Value;

	/// <summary>
	/// Call <see cref="GlobalFunctions.createBattle"/> with all the variables set in this descriptor 
	/// </summary>
	public void createBattle() {
		GlobalFunctions.instance.createBattle (new Enemy (name, level, health, attack, defence, maximumMagic, maximumMagic,
			luck, speed, createSpecialMove (special1, special1Text, special1Magic, special1Value),
			createSpecialMove (special2, special2Text, special2Magic, special2Value), sprite),
			battleMoney, GlobalFunctions.instance.createItem(battleItem), canRunAway);
	}



	/// <summary>
	/// Converts an enum type of <see cref="EnemyMoves"/> to an <see cref="SpecialMove"/> instance  
	/// </summary>
	/// <returns>A special move instance</returns>
	/// <param name="moveType">The type of special move to create.</param>
	/// <param name="text">The text to show when the move is used</param>
	/// <param name="magic">The magic it will consume</param>
	/// <param name="value">A number value, used for the calculation of that specific special move</param>
	private SpecialMove createSpecialMove (EnemyMoves moveType, string text, int magic, float value) {
		SpecialMove specialMove;
		switch (moveType) {
		case EnemyMoves.MagicAttack:
			specialMove = new MagicAttack (text, "", magic, (int) value);
			break;
		case EnemyMoves.RaiseAttack:
			specialMove = new RaiseAttack (text, "", magic, value);
			break;
		case EnemyMoves.RaiseDefence:
			specialMove = new RaiseDefence (text, "", magic, value);
			break;
		case EnemyMoves.LowerDefence:
			specialMove = new LowerDefence (text, "", magic, value);
			break;
		case EnemyMoves.LowerSpeed:
			specialMove = new LowerSpeed (text, "", magic, value);
			break;
		default:
			specialMove = null;
			break;
		}
		return specialMove;
	}
}
