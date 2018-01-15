using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script used to manage and update all of the stat displays in the battle scene
/// </summary>
public class StatsScript : MonoBehaviour {

	[SerializeField]
	/// <summary>The text to display before the current value of the variable (e.g. Health: )</summary>
	private string textHeader;
	/// <summary>The text to display after the current value of the variable (e.g. / 100)</summary>
	private string textFooter;
	private RectTransform rect;
	private UnityEngine.UI.Text textDisplay;
	private float initialWidth;
	private float widthPerUnit;

	// Use this for initialization
	void Start () {
		rect = gameObject.transform.Find ("Bar").GetComponent<RectTransform> ();
		textDisplay = gameObject.transform.Find ("Text").GetComponent<UnityEngine.UI.Text> ();
		initialWidth = rect.sizeDelta.x;
	}

	/// <summary>
	/// Set up initial values
	/// </summary>
	/// <param name="initalValue">The value of the variable at the start of the battle</param>
	/// <param name="maximumValue">The maximum value of the variable, so the bar proportions can
	/// be set appropriately</param>
	public void setUpDisplay (int initalValue, int maximumValue) {
		textFooter = " / " + maximumValue;
		widthPerUnit = initialWidth / maximumValue;
		rect.sizeDelta = new Vector2 (widthPerUnit * initalValue, rect.sizeDelta.y);
		textDisplay.text = textHeader + initalValue + textFooter;
	}		

	/// <summary>
	/// Updates the display to the new value, with the bars and text updating over numerous frames
	/// </summary>
	/// <returns>Coroutines to update on multiple frames</returns>
	/// <param name="previousValue">The value that the varaible started as</param>
	/// <param name="newValue">The value that the variable should now be displayed as.</param>
	public IEnumerator updateDisplay(int previousValue, int newValue) {
		int frames = 60;
		int difference = previousValue - newValue;
		int lastUpdatedValue = previousValue;
		float currentValue = (float) lastUpdatedValue;
		float valuePerFrame = (float) difference / frames;
		float sizePerFrame = widthPerUnit * valuePerFrame;
		for (int i = 0; i < frames; i++) {
			rect.sizeDelta -= new Vector2 (sizePerFrame, 0);
			currentValue -= valuePerFrame;
			if (Mathf.RoundToInt (currentValue) != lastUpdatedValue) {
				lastUpdatedValue = Mathf.RoundToInt (currentValue);
				textDisplay.text = textHeader + lastUpdatedValue + textFooter;
			}
			yield return new WaitForFixedUpdate ();
		}
	}
}
