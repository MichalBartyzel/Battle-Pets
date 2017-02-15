using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*** script managing flow of the game ***/

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	public Rigidbody2D player1Prefab, player2Prefab; // prefabs of players characters
	public Transform player1Origin, player2Origin; // players spawn points
	public float Player1Health, Player2Health;

	GameObject Player1, Player2;

	int player1Score = 0;
	int player2Score = 0;

	bool gameOn=false; // false - paused or menu, true - playing

	// UI components
	Image p1HealthBar, p2HealthBar;
	Text scoreText;

	// singleton of game manager instance
	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
		InitGame ();
	}

	void Update(){
		if (!gameOn)
			return;
		updateHealthBars ();
	}
		
	// update health and check win conditions
	public void setPlayerHealth(string player, float health){
		if (player == "Player1")
			Player1Health = health;
		else if (player == "Player2")
			Player2Health = health;
		if (Player1Health <= 0 || Player2Health <= 0)
			resetMatch ();
	}

	void InitGame(){
		p1HealthBar = GameObject.Find ("P1Health").GetComponent<Image>();
		p2HealthBar = GameObject.Find ("P2Health").GetComponent<Image>();
		scoreText = GameObject.Find ("ScoreText").GetComponent<Text> ();
	}

	public void startGame(){
		gameOn = true;
	}

	void resetMatch(){
		setScore ();
		Destroy (Player1);
		Destroy (Player2);
		summonPlayers();
		Player1Health = 100;
		Player2Health = 100;
	}

	public void setPlayersObj(GameObject p1, GameObject p2){
		Player1 = p1;
		Player2 = p2;
	}

	void setScore(){
		if (Player1Health > Player2Health)
			player1Score++;
		else if (Player1Health < Player2Health)
			player2Score++;
		scoreText.text = player1Score + " : " + player2Score;
	}

	// instantiate and set players characters
	public void summonPlayers(){
		Rigidbody2D P1, P2;

		P1 = Instantiate (player1Prefab, player1Origin.position, Quaternion.identity);
		PlayerController Player1Script = P1.gameObject.GetComponent<PlayerController> ();
		Player1Script.setPlayer ("Player1", "Player2");
		Player1Script.Flip ();

		P2 = Instantiate (player2Prefab, player2Origin.position, Quaternion.identity);
		PlayerController Player2Script = P2.gameObject.GetComponent<PlayerController> ();
		Player2Script.setPlayer ("Player2", "Player1");

		setPlayersObj (P1.gameObject, P2.gameObject);
	}

	void updateHealthBars(){
		p1HealthBar.rectTransform.sizeDelta = new Vector2 (Player1Health, 20);
		p2HealthBar.rectTransform.sizeDelta = new Vector2 (Player2Health, 20);
	}

	// on scene reload
	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode){
		InitGame ();
	}

	void OnEnable(){
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	void OnDisable(){
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}
}
