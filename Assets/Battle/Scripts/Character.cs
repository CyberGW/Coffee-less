using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Character {

	string Name {
		get;
		set;
	}

	int Health {
		get;
		set;
	}

	int Attack {
		get;
		set;
	}

	int Defence {
		get;
		set;
	}

	int MaximumMagic {
		get;
		set;
	}

	int Magic {
		get;
		set;
	}

	int Luck {
		get;
		set;
	}

	int Speed {
		get;
		set;
	}

	SpecialMove Special1 {
		get;
		set;
	}

	SpecialMove Special2 {
		get;
		set;
	}
}
