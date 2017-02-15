using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*** script for one of the playable characters ***/

public class Cat : PlayerController {

	void Awake () {
		base.Awake ();

		// special combo attacks input lengths and strings
		maxInputNumber = 3;
		attacksComboNumber [0] = 3;
		attacksComboNumber [1] = 3;
		specialAttacks [0] = "LeftRightAttack1";
		specialAttacks [1] = "UpUpAttack2";
	}

	void Update () {
		base.Update ();
	}

	void FixedUpdate(){
		base.FixedUpdate ();
	}

	/*** check n'th collision trigger area and deal damage on hit
	 *   method invoked by various attack animations	***/
	void Damage(int n){
		castDamageCircle(n);
	}
}
