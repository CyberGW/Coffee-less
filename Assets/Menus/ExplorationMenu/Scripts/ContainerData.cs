using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An attribute to store information for a container in the item menu
/// </summary>
public class ContainerData : MonoBehaviour {

	/// <summary>
	/// The type of container, describing whether it represents a player or an item slot
	/// </summary>
	public string type;
	/// <summary>
	/// The array index that this container represents
	/// </summary>
	public int index;

	public string Type {
		get {
			return this.type;
		}
	}

	public int Index {
		get {
			return this.index;
		}
	}
}
