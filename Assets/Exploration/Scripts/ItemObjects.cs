using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObjects : MonoBehaviour {

	public Dictionary<string, Item> items;

	// Use this for initialization
	void Start () {
		items = new Dictionary<string, Item> ();
		items.Add ("Hammer", new Hammer ());
	}
}
