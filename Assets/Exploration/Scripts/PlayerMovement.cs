using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	private float speed = 0.1f;
	private string lastMove = "Idle";
	private bool canMove = true;
	private static bool playerExists = false;
	private Transform player;
	private Animator anim;

	// Use this for initialization
	void Start () {
		player = gameObject.GetComponentInParent<Transform> ();
		anim = gameObject.GetComponentInParent<Animator> ();

		if (!playerExists) {
			playerExists = true;
			DontDestroyOnLoad (transform.gameObject);
		} else {
			Destroy (gameObject);
		}

	}

	// Update is called once per frame
	void FixedUpdate () {

		if (Input.GetKeyDown (KeyCode.Return)) {
			GlobalFunctions.instance.createBattle (new Enemy ("Swinefoogle", 5, 100, 15, 5, 5, 5, 5, 5,  new Fireball("fireballed", "N/A", 3),  new Fireball("fireballed", "N/A", 5)), 50, null);
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
		}
	}

	private void walkAnimation(string direction) {
		if (lastMove != direction) {
			anim.SetTrigger ("Walk" + direction);
			lastMove = direction;
		}
	}

	private void setIdle () {
		if (lastMove != "Idle") {
			anim.SetTrigger ("Idle" + lastMove);
			lastMove = "Idle";
		}
	}

	public void setCanMove(bool value) {
		canMove = value;
		if (!value) {
			setIdle ();
		}
	}

}
