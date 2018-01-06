using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTestSetupScript : MonoBehaviour {

	private static bool initialised = false;

	// Use this for initialization
	void Start () {
		if (!initialised) {
			DataManager data = PlayerData.instance.data;
			data.addPlayer (new Player ("Hannah", 5, 100, 5, 5, 5, 5, 5, 5, 0, null,
				new IncreaseMoney ("stole money from", "Increase money returns by 50%", 2, 0.5f),
				new MagicAttack ("threw wine battles at", "Thorw wine bottles with damage 15", 2, 15),
				(Texture2D)Resources.Load ("Character2", typeof(Texture2D))));
			data.addItem (new Hammer ());
			data.addItem (new RabbitFoot ());
			initialised = true;
		}
	}

}
