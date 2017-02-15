using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*** script for one of the playable characters ***/

public class Sloth : PlayerController {

	void Awake () {
		base.Awake ();

		// special combo attacks input lengths and strings
		maxInputNumber = 4;
		attacksComboNumber [0] = 4;
		attacksComboNumber [1] = 4;
		specialAttacks [0] = "LeftRightDownDown";
		specialAttacks [1] = "UpUpDownDown";
		Flip();
	}
		
	void Update () {
		base.Update ();
		/*** checking if character is grounded
		 * 	 needed for one of special attacks ***/
		anim.SetBool("grounded",grounded);
	}

	void FixedUpdate(){
		base.FixedUpdate ();
	}

	/*** check n'th collision trigger area and deal damage on hit
	 *   method invoked by various attack animations	***/
	void Damage(int n){
		castDamageCircle(n);
	}

	// one of special combo attacks
	void castClaws(){
		Shoot ();
	}
}
