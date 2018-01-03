using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	[SerializeField]
	private string bossObjectName = "Boss";
	[SerializeField]
	private Vector2 worldMapExitPosition;

	// Use this for initialization
	void Start () {
		IDictionary<string,bool> objectsActive = GlobalFunctions.instance.objectsActive;
		string key = SceneManager.GetActiveScene ().name + bossObjectName;
		if (objectsActive.ContainsKey(key)) {
			if (!GlobalFunctions.instance.objectsActive [key]) {
				GlobalFunctions.instance.currentLevel += 1;
				Debug.Log ("Beat the level!");
				SceneChanger.instance.loadLevel ("WorldMap", worldMapExitPosition);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
