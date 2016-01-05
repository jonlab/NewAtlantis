using UnityEngine;
using System.Collections;

public class NARandomMover: MonoBehaviour {
	public float speed = 10f; 
	public float speedvar = 0f; // random variation of instant speed
	public Vector3 range = new Vector3 (10f, 10f, 10f);
	public float smoothing = 10f;	//smoothing direction changes
	private float instantspeed = 0;

	Vector3 target ;
	Vector3 smoothtarget;

	void Update () 
	{
		smoothtarget = smoothtarget * smoothing/100f + target * (1f - smoothing/100f); // smoothing curves

		// translation
		transform.Translate(Vector3.forward * Time.deltaTime * instantspeed);
		transform.LookAt(smoothtarget);
		float distance = Vector3.Distance (transform.position, target);
		
		//Move to next waypoint:
		if (distance < (range.x + range.y + range.z)/3) 
		{
			target = new Vector3(Random.Range(-range.x, range.x), Random.Range(0, range.y), Random.Range(-range.z, range.z));
			//print ("distance " + distance);
			instantspeed = speed + Random.Range (0f, speedvar);
			//print (instantspeed);
		}
	}
}
