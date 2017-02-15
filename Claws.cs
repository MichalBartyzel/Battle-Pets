using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claws : MonoBehaviour {

	// destroy claws at the end of animation
	void destruction(){
		DestroyObject (gameObject);
	}
}
