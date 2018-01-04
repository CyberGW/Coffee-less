using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>A script to control the movement and animations of the player</summary>
public class PlayerMovement : MonoBehaviour {

	/// <summary>The distance to move each frame</summary>
	private float speed = 0.1f;
	/// <summary>
	/// A string describing what the player's last move was so that the player faces the correct way when idle
	/// </summary>
	private string lastMove = "Idle";
	private bool canMove = true;
	/// <summary>
	/// Used in case a scene duplicated the player
	/// </summary>
	private static bool playerExists = false;
	private Vector2 previousPosition;
	/// <summary>
	/// The transform component of the player
	/// </summary>
	private Transform player;
	/// <summary>
	/// The animation controller of the player
	/// </summary>
	private Animator anim;

	// Use this for initialization
	void Start () {

		if (!playerExists) {
			playerExists = true;
			DontDestroyOnLoad (transform.gameObject);

			player = gameObject.GetComponentInParent<Transform> ();
			anim = gameObject.GetComponentInParent<Animator> ();
			previousPosition = gameObject.transform.position;
		} else {
			Destroy (gameObject);
		}

	}

	/// <summary>
	/// Called once per frame to check the user key presses
	/// </summary>
	void FixedUpdate () {

		if (Input.GetKeyDown (KeyCode.Return)) {
			GlobalFunctions.instance.createBattle (new Enemy ("Swinefoogle", 5, 100, 15, 5, 5, 5, 5, 5, new MagicAttack("fireballed", "Fireball", 30, 3),  new MagicAttack("fireballed", "Fireball", 30, 5)), (Texture2D) Resources.Load("Little_Green_Enemy", typeof(Texture2D)), 50, null, false);
		}
		if (Input.GetKeyDown (KeyCode.Escape)) {
			SceneChanger.instance.loadLevel ("ItemMenu");
		}

		if (Input.GetKey (KeyCode.UpArrow)) {
			move ("Up");
		} else {
			if (Input.GetKey (KeyCode.DownArrow)) {
				move ("Down");
			} else {
				if (Input.GetKey (KeyCode.LeftArrow)) {
					move ("Left");
				} else {
					if (Input.GetKey (KeyCode.RightArrow)) {
						move ("Right");
					} else {
						setIdle ();
					}
				}
			}
		}
	}

	public void move(string direction) {
		if (canMove) {
			walkAnimation (direction);
			Vector2 translation;
			switch (direction) {
			case "Up":
				translation = Vector2.up;
				break;
			case "Down":
				translation = Vector2.down;
				break;
			case "Left":
				translation = Vector2.left;
				break;
			case "Right":
				translation = Vector2.right;
				break;
			default:
				translation = Vector2.zero;
				break;
			}
			player.Translate (translation * speed);
			//randomEncounter ();
		}
	}

	/// <summary>
	/// If the player has moved enough since last called, then 5% chance of creating a random encounter battle
	/// </summary>
	private void randomEncounter() {
		Vector2 newPosition = player.transform.position;
		if (Vector2.Distance (newPosition, previousPosition) > 3) {
			previousPosition = newPosition;
			if (Random.value < 0.05) {
				BattleDescriptor battle = GameObject.Find ("RandomEncounter").GetComponent<BattleDescriptor> ();
				battle.createBattle ();
			}
		}
	}

	/// <summary>
	/// Set the trigger on the animation controller to update display appropriately
	/// </summary>
	/// <param name="direction">Direction.</param>
	private void walkAnimation(string direction) {
		if (lastMove != direction) {
			anim.SetTrigger ("Walk" + direction);
			lastMove = direction;
		}
	}

	/// <summary>
	/// When the user isn't moving, set the appropriate idle animation based upon <see cref="lastMove"/> 
	/// </summary>
	private void setIdle () {
		if (lastMove != "Idle") {
			anim.SetTrigger ("Idle" + lastMove);
			lastMove = "Idle";
		}
	}

	/// <summary>
	/// Set whether the player should be able to move or not
	/// </summary>
	/// <param name="value">If <c>true</c> player can move, if <c>false</c> then player can't move.</param>
	public void setCanMove(bool value) {
		canMove = value;
		if (!value) {
			setIdle ();
		}
	}

}
