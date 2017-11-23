using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	private GameObject target;

	// Use this for initialization
	void Start () {
		target = GameObject.Find ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -6);
	}
}
