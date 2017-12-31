using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (!GlobalFunctions.instance.objectsActive [SceneManager.GetActiveScene ().name + "Boss"]) {
			GlobalFunctions.instance.currentLevel += 1;
			Debug.Log ("Beat the level!");
			SceneChanger.instance.loadLevel ("WorldMap", new Vector2 (-10.55f, -1.46f));
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
