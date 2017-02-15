using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*** class for projectiles and their collision ***/

public class ProjectileController : MonoBehaviour {

	PlayerController player;
	float damage;
	public string enemyTag;
	public string friendlyTag;

	void Start () {
		Destroy (gameObject, 5); // lifetime of projectile (in seconds)
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == enemyTag) {
			player = other.gameObject.GetComponent<PlayerController> ();
			player.takeDamage (damage);
			Destroy (gameObject);
		}
		if (other.gameObject.tag == "Ground")
			Destroy (gameObject);
	}

	public void setDamage(float dmg, string targetTag){
		damage = dmg;
		enemyTag = targetTag;
	}
}
