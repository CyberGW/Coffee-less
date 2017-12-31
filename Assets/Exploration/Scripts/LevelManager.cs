using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (!GlobalFunctions.instance.objectsActive [SceneManager.GetActiveScene ().name + "Boss"]) {
			Debug.Log ("Beat the level!");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
