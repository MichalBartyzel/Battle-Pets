using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*** script for one of the playable characters ***/

public class Seal : PlayerController {

	void Awake () {
		base.Awake ();

		// special combo attacks input lengths and strings
		maxInputNumber = 4;
		attacksComboNumber [0] = 4;
		attacksComboNumber [1] = 3;
		specialAttacks [0] = "LeftRightDownDown";
		specialAttacks [1] = "UpJumpJump";
	}
		
	void Update () {
		base.Update ();
		/*** checking if character is grounded
		 * 	 needed for Slide special attack ***/
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
	void Slide(int force){
		rb2d.gravityScale = 0;
		foreach(Collider2D col in colliders){
			col.isTrigger = true;
		}
		int direction;
		if (facingRight)
			direction = 1;
		else
			direction = -1;
		rb2d.velocity = new Vector2 (force * direction, 0);
		if (force == 0) {
			rb2d.velocity = new Vector2 (0, 0);
			foreach(Collider2D col in colliders){
				col.isTrigger = false;
				rb2d.gravityScale = 1;
			}
		}
	}
}
