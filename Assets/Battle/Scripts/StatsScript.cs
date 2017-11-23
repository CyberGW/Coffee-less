using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsScript : MonoBehaviour {

	private RectTransform rect;
	private UnityEngine.UI.Text textDisplay;

	// Use this for initialization
	void Start () {
		rect = gameObject.transform.Find ("HealthBar").GetComponent<RectTransform> ();
		textDisplay = gameObject.transform.Find ("HealthText").GetComponent<UnityEngine.UI.Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public IEnumerator updatePlayerHealth(int previousHealth, int newHealth) {
		int frames = 60;
		int difference = previousHealth - newHealth;
		int lastUpdatedHealth = previousHealth;
		float currentHealth = (float) lastUpdatedHealth;
		float healthPerFrame = (float) difference / frames;
		float sizePerFrame = 2 * healthPerFrame;
		for (int i = 0; i < frames; i++) {
			rect.sizeDelta -= new Vector2 (sizePerFrame, 0);
			currentHealth -= healthPerFrame;
			if (Mathf.RoundToInt (currentHealth) != lastUpdatedHealth) {
				lastUpdatedHealth = Mathf.RoundToInt (currentHealth);
				textDisplay.text = "Health: " + lastUpdatedHealth;
			}
			yield return new WaitForFixedUpdate ();
		}
	}
}
