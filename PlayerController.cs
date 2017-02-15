using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

/*** class controlling player movement and actions ***/

public class PlayerController : MonoBehaviour {

	/*** Class with variables needed to create
	 * 	 overlap circles in which you check collisions
	 * 	 with enemy player for different attacks ***/
	[Serializable]
	public class AttackArea
	{
		public Transform pos;
		public float r;
		public float dmg;

		public AttackArea(Transform position, float radius, float damage){
			pos = position;
			r = radius;
			dmg = damage;
		}
	}

	// holds information about all attacks position, radius and damage
	public List<AttackArea> atkAreas = new List<AttackArea>();

	public float jumpForce = 500f;
	public float maxSpeed = 3f;
	public int maxBlocks = 3; // max amount of blocks
	public int blocks; // current amount of blocks
	// time in seconds in which each block refreshes
	public float blockRefreshRate = 5f;
	public string player = "";
	public string enemyTag = "";
	public bool blocking = false; // is player currently blocking?
	public float maxHealth = 100f;
	public float health;
	public LayerMask enemyLayer;
	public bool facingRight=false;

	protected Rigidbody2D rb2d;
	protected Animator anim;
	protected Collider2D[] colliders; // all colliders attached to player
	protected SpriteRenderer sr;
	protected float hSpeed;
	protected float vSpeed;
	protected int walkHash = Animator.StringToHash("walk");
	protected int idleHash = Animator.StringToHash("idle");
	protected int jumpHash = Animator.StringToHash("jump");
	protected PlayerController enemy;
	protected bool grounded = false; // is character is grounded?
	// queue of player input strings
	protected List<string> comboInput = new List<string> ();
	// number of how many input keys are checked for combo attacks
	protected int maxInputNumber = 3;
	// strings combinations needed to perform special attacks 
	protected string[] specialAttacks = new string[2];
	// input length for each special attack
	protected int[] attacksComboNumber = new int[2];
	protected bool xAxisInUse = false;
	protected bool yAxisInUse = false;
	// is character performing special attack?
	protected bool specialAttack = false;

	// variables for ground checking
	public Transform gCheckPosition;
	public float gCheckRadius = 0.1f;
	public LayerMask groundLayer;

	// variables for shooting projectiles
	public Rigidbody2D projectilePrefab;
	public Transform projectileOrigin;
	public Vector2 shootForce;
	public float projectileDmg;

	protected void Awake () {
		rb2d = gameObject.GetComponent<Rigidbody2D> ();
		anim = gameObject.GetComponent<Animator> ();
		colliders = gameObject.GetComponents<Collider2D> ();
		sr = GetComponent<SpriteRenderer>();
		health = maxHealth;
		blocks = maxBlocks;
	}

	// get axis input and move character
	protected void FixedUpdate(){
		float speed = Input.GetAxis (player+" Horizontal");
		grounded = Physics2D.OverlapCircle (gCheckPosition.position, gCheckRadius, groundLayer);
		hSpeed = Mathf.Abs (rb2d.velocity.x);
		vSpeed = Mathf.Abs(rb2d.velocity.y); 
		anim.SetFloat ("speed", hSpeed);
		anim.SetFloat ("vSpeed", vSpeed);

		if (!canMove())
			return;
		rb2d.velocity = new Vector2 (speed * maxSpeed, rb2d.velocity.y); 
	}
		
	// check input for jumping, attacking and blocking
	protected void Update () {
		if (Input.GetButtonDown (player + " Jump")) {
			Jump ();
		}
		if (Input.GetButtonDown (player + " Attack1")) {
			Attack1 ();
		}
		if (Input.GetButtonDown (player + " Attack2")) {
			Attack2 ();
		}
		if (Input.GetButtonDown (player + " Block")) {
			if (blocks > 0) {
				blocking = true;
				anim.SetBool ("block", blocking);
			}
			addInput ("Block");
		}
		if (Input.GetButtonUp (player + " Block")) {
			blocking = false;
			anim.SetBool ("block", blocking);
		}
		if (Input.GetButtonDown ("Cancel")) {
			Restart ();
		}
		checkAxis ();

	}

	// check if character is not performing attacks and can move
	protected bool canMove(){
		int currentState = anim.GetCurrentAnimatorStateInfo (0).shortNameHash;
		if (currentState == walkHash || currentState == idleHash || currentState == jumpHash)
			return true;
		else
			return false;
	}

	protected void Jump(){
		if(grounded && !specialAttack)
			rb2d.AddForce (new Vector2(0,jumpForce));
		addInput ("Jump");
	}

	//special attack 1
	protected void Attack1(){
		if(!specialAttack)
			anim.SetTrigger ("attack1");
		addInput ("Attack1");
	}

	//special attack 2
	protected void Attack2(){
		if(!specialAttack)
			anim.SetTrigger ("attack2");
		addInput ("Attack2");
	}

	// add string to combo queue based on button pressed
	protected void addInput(string input){
		comboInput.Add (input);
		if (comboInput.Count > maxInputNumber)
			comboInput.RemoveAt (0);
		checkInput (attacksComboNumber[0],attacksComboNumber[1]);
	}
		
	// check if last input created a combo attack
	protected void checkInput(int x1, int x2){
		int c = comboInput.Count;
		string combo1="";
		string combo2="";
		if (c >= x1 ) {
			for (int i = c - x1; i < c; i++) {
				combo1 += comboInput [i];
			}
			if (combo1 == specialAttacks [0]) {
				anim.SetTrigger ("attack3");
				specialAttack = true;
				comboInput.Clear ();
				return;
			}
		}
		if (c >= x2) {
			for (int i = c - x2; i < c; i++) {
					combo2 += comboInput [i];
			}
			if (combo2 == specialAttacks [1]) {
				//specialAttack = true;
				anim.SetTrigger ("attack4");
				specialAttack = true;
				comboInput.Clear ();
				return;
			}
		}
	}

	// turn axis input into a combo string
	protected void checkAxis(){
		if (Input.GetAxisRaw (player + " Horizontal") == 1) {
			if (xAxisInUse == false) {
				xAxisInUse = true;
				addInput ("Right");
			}
		} else if (Input.GetAxisRaw (player + " Horizontal") == -1) {
			if (xAxisInUse == false) {
				xAxisInUse = true;
				addInput ("Left");
			}
		} else {
			xAxisInUse = false;
		}
		if (Input.GetAxisRaw (player + " Vertical") == 1) {
			if (yAxisInUse == false) {
				yAxisInUse = true;
				addInput ("Up");
			}
		} else if (Input.GetAxisRaw (player + " Vertical") == -1) {
			if (yAxisInUse == false) {
				yAxisInUse = true;
				addInput ("Down");
			}
		} else {
			yAxisInUse = false;
		}
	}

	// lower player health
	public void takeDamage(float dmg){
		if (blocking == false){
			health -= dmg;
			GameManager.instance.setPlayerHealth (player, health);
		}
		else {
			blocks--;
			StartCoroutine (refreshBlock());
			if (blocks <= 0) {
				blocking = false;
				anim.SetBool ("block", blocking);
			}
		}
	}

	// check for collision when attacking
	protected void castDamageCircle(int n){
		Collider2D hit = Physics2D.OverlapCircle (atkAreas[n].pos.position, atkAreas[n].r, enemyLayer);
		if (hit) {
			enemy = hit.gameObject.GetComponent<PlayerController> ();
			enemy.takeDamage (atkAreas[n].dmg);
		}
	}

	// set player and enemy tags
	public void setPlayer(string player, string enemy){
		this.player = player;
		this.enemyTag = enemy;
		enemyLayer = LayerMask.GetMask(enemy);
		gameObject.tag =  player;
		gameObject.layer = LayerMask.NameToLayer (player);
	}

	// flip character and it's colliders
	public void Flip(){
		sr.flipX = !sr.flipX;
		foreach(Collider2D coll in colliders){
			coll.offset = new Vector2 (-coll.offset.x, coll.offset.y);
		}
		foreach (Transform child in transform) {
				child.localPosition = new Vector2 (-child.localPosition.x, child.localPosition.y);
		}
		facingRight = !facingRight;
	}

	// instantiate and shoot a projectile
	protected void Shoot(){
		Vector2 sF= shootForce;
		if (!facingRight)
			sF = new Vector2(-sF.x,sF.y);
		Rigidbody2D projectile = Instantiate (projectilePrefab, projectileOrigin.position,Quaternion.identity) as Rigidbody2D;
		projectile.AddForce (sF);
		projectile.GetComponent<ProjectileController> ().setDamage (projectileDmg, enemyTag);
	}

	// end special attack allowing player to perform other actions
	protected void endSpecialAttack(){
		specialAttack = false;
	}

	// refresh block after set amount of time
	IEnumerator refreshBlock(){
		yield return new WaitForSeconds (blockRefreshRate);
		if (blocks < maxBlocks)
			blocks++;
		
	}

	// restart game
	void Restart(){
		SceneManager.LoadScene (0);
	}
}
