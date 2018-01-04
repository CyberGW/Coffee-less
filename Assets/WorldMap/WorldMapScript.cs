using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A controler for the world map, stopping players from entering old or future levels, and
/// colouring the departments approriately</summary>
public class WorldMapScript : MonoBehaviour {

	/// <summary>
	/// When World Map is loaded, set all beaten levels to red and stop them acting as portals and set the next level to green.
	/// The future levels are then defaultly uncoloured
	/// </summary>
	void Start () {
		string[] levelOrder = GlobalFunctions.instance.levelOrder;
		int currentLevel = GlobalFunctions.instance.currentLevel;
		for (int i = 0; i < currentLevel; i++) { //For all levels already beat
			renderBuilding(levelOrder[i], Color.red, true);
		}
		renderBuilding (levelOrder [currentLevel], Color.green);
	}

	/// <summary>
	/// Renders the building, when called by <see cref="Start"/> 
	/// </summary>
	/// <param name="buildingName">The name of the building to render</param>
	/// <param name="colour">The colour to colour the building as</param>
	/// <param name="removeCollider">If set to <c>true</c> remove collider. <c>false</c> by default</param>
	private void renderBuilding (string buildingName, Color colour, bool removeCollider = false) {
		GameObject building = GameObject.FindWithTag (buildingName);
		GameObject image = building.transform.Find(buildingName).gameObject; //Get the image part
		image.GetComponent<MeshRenderer> ().material.color = colour; //Set mesh colour
		GameObject collider = building.transform.Find ("Collision").gameObject; //Get collider element
		collider.GetComponent<PolygonCollider2D> ().isTrigger = !removeCollider;
	}

}
