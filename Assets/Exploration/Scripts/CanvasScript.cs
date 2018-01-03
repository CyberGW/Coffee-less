using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Added to a canvas holding the dialogue elements to ensure it's not destroyed when a new scene
/// is created </summary>
public class CanvasScript : MonoBehaviour {

	/// <summary>
	/// Calls <c>DontDestroyOnLoad</c> here when the object is created
	/// </summary>
	void Start () {
		DontDestroyOnLoad (gameObject);
	}

}
