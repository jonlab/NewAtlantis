using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour {
	
	public float paddleSpeed = 1f;
	private Vector3 playerPos = new Vector3 (0, -13f, 0);


	//public static Paddle instance;


	
	/*	void Update () 
	{

		instance = this;
		float xPos = transform.position.x + (Input.GetAxis("Horizontal") * paddleSpeed);
		playerPos = new Vector3 (Mathf.Clamp (xPos, -8f, 8f), -13f, 0f);
		transform.position = playerPos;
		
	}*/
	/*public void Awake () 
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
	
		
	}*/

	public  void GoRight ()

	{

			float xPos = transform.position.x + 5;
			playerPos = new Vector3 (Mathf.Clamp (xPos, -6f, 6f), -13f, 0f);
			transform.localPosition = playerPos;
		}

	public  void GoLeft () 
	{
		float xPos = transform.position.x + (-5);
		playerPos = new Vector3 (Mathf.Clamp (xPos, -6f, 6f), -13f, 0f);
		transform.localPosition = playerPos;
	}

}