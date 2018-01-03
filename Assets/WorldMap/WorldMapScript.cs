using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapScript : MonoBehaviour {

	GameObject TFTV;

	// Use this for initialization
	void Start () {
		string[] levelOrder = GlobalFunctions.instance.levelOrder;
		int currentLevel = GlobalFunctions.instance.currentLevel;
		GameObject building;
		GameObject image;
		GameObject collider;
		for (int i = 0; i < currentLevel; i++) { //For all levels already beat
			building = GameObject.FindWithTag (levelOrder [i]); //Find building
			image = building.transform.Find(levelOrder [i]).gameObject; //Get the image part
			image.GetComponent<MeshRenderer> ().material.color = Color.red; //Set mesh colour to red
			collider = building.transform.Find ("Collision").gameObject; //Get collider element
			//Remove trigger component so that level cannot be re-entered and building can't be walked through
			collider.GetComponent<PolygonCollider2D> ().isTrigger = false;
		}
		building = GameObject.FindWithTag (levelOrder [currentLevel]).transform.Find(levelOrder [currentLevel]).gameObject;
		building.GetComponent<MeshRenderer> ().material.color = Color.green;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
