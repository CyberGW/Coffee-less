using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackButton : MonoBehaviour {

	private GameObject attacksPanel;
	private bool panelActive;

	// Use this for initialization
	void Start () {
		attacksPanel = GameObject.Find ("AttacksPanel");
		attacksPanel.SetActive (false);
		panelActive = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setPanelActive() {
		if (panelActive) {
			attacksPanel.SetActive (false);
			panelActive = false;
		} else {
			attacksPanel.SetActive (true);
			panelActive = true;
		}
	}
}
