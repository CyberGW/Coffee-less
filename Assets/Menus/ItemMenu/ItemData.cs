using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
