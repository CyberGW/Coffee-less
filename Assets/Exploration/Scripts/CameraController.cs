using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Script to move the camera around so that it follows the player</summary>
public class CameraController : MonoBehaviour {

	private GameObject target;

	// Use this for initialization
	void Start () {
		target = GameObject.Find ("Player");
	}
	
	/// <summary>
	/// On every update, set the position of the camera to the same position as the player object
	/// </summary>
	void Update () {
		transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -6);
	}
}
