using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*** script handling menu interactions ***/

public class MenuManager : MonoBehaviour {

	GameObject menu;

	// array of playable characters prefabs
	public Rigidbody2D[] characters = new Rigidbody2D[4]; 

	// indicates if character has been taken by another player
	bool[] available = new bool[4];

	bool player1Choosing = true;
	bool player2Choosing = true;

	int p1Pos = 0;
	int p2Pos = 2;

	bool p1AxisInUse = false;
	bool p2AxisInUse = false;

	public Text p1Text, p2Text;

	void Start () {
		p1Text = GameObject.Find ("Player1Text").GetComponent<Text> ();
		p2Text = GameObject.Find ("Player2Text").GetComponent<Text>();
		menu = GameObject.Find ("CharacterSelection");

		for (int i = 0; i < available.Length; i++) {
			available [i] = true;
		}
	}

	// handles player input in menu
	void Update () {
		// Player 1 keyboard input
		if (Input.GetAxisRaw ("Player1 Horizontal") != 0) {
			if (!p1AxisInUse && player1Choosing) {
				p1AxisInUse = true;
				p1Pos+=(int)Input.GetAxisRaw("Player1 Horizontal");
				if (p1Pos > characters.Length - 1)
					p1Pos = 0;
				if (p1Pos < 0)
					p1Pos = characters.Length - 1;
				Vector3 rT = p1Text.rectTransform.localPosition;
				p1Text.rectTransform.localPosition = new Vector3 ((p1Pos - 1) * 100 - 15, rT.y, rT.z);
			}
		} else {
			p1AxisInUse = false;
		}

		// Player 2 gamepad input
		if (Input.GetAxisRaw ("Player2 Horizontal") != 0) {
			if (!p2AxisInUse && player2Choosing) {
				p2AxisInUse = true;
				p2Pos+=(int)Input.GetAxisRaw("Player2 Horizontal");;
				if (p2Pos > characters.Length - 1)
					p2Pos = 0;
				if (p2Pos < 0)
					p2Pos = characters.Length - 1;
				Vector3 rT = p2Text.rectTransform.localPosition;
				p2Text.rectTransform.localPosition = new Vector3 ((p2Pos - 1) * 100 + 15, rT.y, rT.z);
			}
		} else {
			p2AxisInUse = false;
		}

		if (Input.GetButtonDown ("Player1 Submit")) {
			if (available [p1Pos] == true) {
				player1Choosing = false;
				available [p1Pos] = false;
			}
		}
		if (Input.GetButtonDown ("Player2 Submit")) {
			if (available [p2Pos] == true) {
				player2Choosing = false;
				available [p2Pos] = false;
			}
		}

		// if both players have chosen they characters, hide menu and start game
		if (!player1Choosing && !player2Choosing) {
			GameManager.instance.player1Prefab = characters [p1Pos];
			GameManager.instance.player2Prefab = characters [p2Pos];
			GameManager.instance.summonPlayers ();
			GameManager.instance.startGame ();
			DestroyObject (menu);
		}
	}
}
