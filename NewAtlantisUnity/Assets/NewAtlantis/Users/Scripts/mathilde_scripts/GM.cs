using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GM : MonoBehaviour {
	
	public int lives = 3;
	public int initialbricks = 19;
	public float resetDelay = 1f;
	//public GameObject particleLives;
	public GameObject gameOver;
	public GameObject youWon;
	public GameObject bricksPrefab;
	public GameObject paddle;
	public GameObject deathParticles;
	public GameObject scoreParticles;
	public static GM instance = null;
	//public CuvetteRight cuvetteRight;
	//public CuvetteLeft cuvetteLeft;
//	public GameObject paddle;
	public GameObject ball;
	private int bricks = 0;


	// Use this for initialization
	void Awake () 
	{
		instance = this;
		/*if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		*/
		Setup();
		
	}
	
	public void Setup()
	{
		bricks = initialbricks;
		//ball =  GameObject.FindWithTag("Ball");
		//clonePaddle = Instantiate(paddle, new Vector3(0,-13f,0), Quaternion.identity) as GameObject;
		//cuvetteLeft.paddle = clonePaddle;
		//cuvetteRight.paddle = clonePaddle;
		//Instantiate(bricksPrefab, transform.position, Quaternion.identity);
	}
	
	void CheckGameOver()
	{
		Debug.Log ("Check GameOver " + bricks + " " + lives);
		if (bricks < 1)
		{
			youWon.SetActive(true);
			//Time.timeScale = .25f;
			Invoke ("Reset", resetDelay);
		}
		
		if (lives < 1)
		{
			gameOver.SetActive(true);
			//Time.timeScale = .25f;
			Invoke ("Reset", resetDelay);
		}

		
	}

	void Reset()
	{

		youWon.SetActive(false);
		gameOver.SetActive(false);


		// Reset bricks :
		//bricksPrefab.GetComponent<Collider> ().enabled = true;
		//bricksPrefab.GetComponent<MeshRenderer> ().enabled = true;
		bricks = initialbricks;

		Bricks[] scripts = bricksPrefab.GetComponentsInChildren<Bricks> ();
		foreach (Bricks b in scripts) 
		{
			//Debug.Log ("Reset brick");
			b.Reset();
		}

		//Reset Paddle :

		Invoke ("SetupPaddle",0f);

		// lives : 

		lives = 3;
		for (int i=0; i<scoreParticles.transform.childCount; ++i) {
			GameObject goChild = scoreParticles.transform.GetChild (i).gameObject;
			goChild.SetActive (true);
		}





		//Time.timeScale = 1f;
		//Application.LoadLevel(Application.loadedLevel);
	}

	public void LoseLife()
	{
		lives--;
		//Destroy (ParticleSystem);
		//livesText.text = "Lives: " + lives;
		//Instantiate(deathParticles, clonePaddle.transform.position, Quaternion.identity);
		//Destroy(clonePaddle);
		Invoke ("SetupPaddle", resetDelay);
		CheckGameOver();

		for (int i=0; i<scoreParticles.transform.childCount; ++i) 
		{
			GameObject goChild = scoreParticles.transform.GetChild(i).gameObject;
			if (i<lives)
				goChild.SetActive(true);
			else
				goChild.SetActive(false);
		}
	}
	
	void SetupPaddle()
	{
		//ball.transform.position = new Vector3 (0, -13f, 0); 
		//paddle.transform.position = new Vector3 (0, -13f, 0); 

		ball.transform.localPosition = new Vector3 (0,-11.92f, 0);
		paddle.transform.localPosition = new Vector3 (0, -13f, 0); 
		Rigidbody rb = ball.GetComponent<Rigidbody> ();
		rb.isKinematic = true;
		Ball.ballInPlay = false;
		CuvetteLeft.fire = false;
		CuvetteRight.fire = false;
		//ball.transform.parent = paddle;
		//Ball.ballInPlay = true;

		//paddle.transform.GetChild("ball") = new Vector3 (0, -13f, 0);
		//clonePaddle = Instantiate(paddle,new Vector3(0,-13f,0), Quaternion.identity) as GameObject;
	}
	
	public void DestroyBrick()
	{
		bricks--;
		CheckGameOver();

	}
}