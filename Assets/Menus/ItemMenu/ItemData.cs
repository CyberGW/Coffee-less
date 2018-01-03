using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A component to add to a drag and drop cell, carrying the data for the item the cell references
/// </summary>
public class ItemData : MonoBehaviour {

	private Item item;

	public Item Item {
		get {
			return this.item;
		}
		set {
			item = value;
		}
	}
}
