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
		player = GameObject.Find ("Player").GetComponent<Transform>();
		anim = GameObject.Find("Player").GetComponent<Animator> ();

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
			Initiate.Fade ("Battle", Color.black, 3f);
			//UnityEngine.SceneManagement.SceneManager.LoadScene ("Battle");
		}

		if (canMove) {
			Vector2 movement = Vector2.zero;

			if (Input.GetKey (KeyCode.UpArrow)) {
				walkAnimation ("Up");
				movement = Vector2.up * speed;
			} else {
				if (Input.GetKey (KeyCode.DownArrow)) {
					walkAnimation ("Down");
					movement = Vector2.down * speed;
				} else {
					if (Input.GetKey (KeyCode.LeftArrow)) {
						walkAnimation ("Left");
						movement = Vector2.left * speed;
					} else {
						if (Input.GetKey (KeyCode.RightArrow)) {
							walkAnimation ("Right");
							movement = Vector2.right * speed;
						} else {
							setIdle ();
						}
					}
				}
			}
			player.Translate (movement);
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
