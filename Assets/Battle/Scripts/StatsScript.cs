using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsScript : MonoBehaviour {

	public string textHeader;
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
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setUpDisplay (int initalValue, int maximumValue) {
		widthPerUnit = initialWidth / maximumValue;
		rect.sizeDelta = new Vector2 (widthPerUnit * initalValue, 30);
		textDisplay.text = textHeader + initalValue;
	}

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
				textDisplay.text = textHeader + lastUpdatedValue;
			}
			yield return new WaitForFixedUpdate ();
		}
	}
}
