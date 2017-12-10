using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemButton : MonoBehaviour {

    private GameObject itemsPanel;
    private bool itemPanelActive;

	// Use this for initialization
	void Start () {
        itemsPanel = GameObject.Find("itemsPanel");
        itemsPanel.SetActive(false);
        itemPanelActive = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setItemPanelActive() {
        if (itemPanelActive) {
            itemsPanel.SetActive(false);
            itemPanelActive = false;
        } else {
            itemsPanel.SetActive (true);
            itemPanelActive = true;
        }
    }
}
