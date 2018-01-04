using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Added to a canvas holding the dialogue elements to ensure it's not destroyed when a new scene
/// is created </summary>
public class CanvasScript : MonoBehaviour {

	public static CanvasScript instance = null;

	/// <summary>
	/// Ensures that the canvas isn't duplicated, and the same one is carried acrossed multiple scenes
	/// </summary>
	void Start () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
	}

}
