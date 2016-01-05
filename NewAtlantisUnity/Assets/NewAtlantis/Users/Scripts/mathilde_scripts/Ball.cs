using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
	
	public float ballInitialVelocity = 600f;
	
	
	private Rigidbody rb;
	public static bool ballInPlay;
	
	void Awake () {
		
		rb = GetComponent<Rigidbody>();
		
	}
	
	void Update () 
	{
		if (!NA.isClient())
		{
			if ((CuvetteLeft.fire == true || CuvetteRight.fire == true) && ballInPlay == false)
			{
				//transform.parent = null;
				ballInPlay = true;
				rb.isKinematic = false;
				rb.AddRelativeForce(new Vector3(ballInitialVelocity, ballInitialVelocity, 0));
			}
		}
	}
}