using UnityEngine;
using System.Collections;

public class CuvetteRight : MonoBehaviour {

	//public GameObject paddle;
	public GameObject paddleObj;
	public static bool fire = false;

	void OnCollisionEnter ( Collision other )
	{
		if (!NA.isClient ()) 
		{
			fire = true;
			if (paddleObj == null) {
				paddleObj = GameObject.Find ("Paddle");
				
			}
			if (paddleObj != null) {
				//BroadcastMessage("GoLeft");
				Paddle paddleComponent = paddleObj.GetComponent<Paddle> ();
				paddleComponent.GoRight ();

			}

			//Destroy (other.gameObject);
		}




	}
}


