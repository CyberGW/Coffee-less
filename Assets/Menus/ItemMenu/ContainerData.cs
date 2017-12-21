using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerData : MonoBehaviour {

	private string type;
	private int index;

	void Start() {
		type = gameObject.name.Substring (0, gameObject.name.Length - 1);
		index = int.Parse (gameObject.name.Substring (gameObject.name.Length - 1));
	}

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
