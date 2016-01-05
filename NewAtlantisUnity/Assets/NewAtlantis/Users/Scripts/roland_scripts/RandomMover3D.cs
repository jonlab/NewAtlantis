using UnityEngine;
using System.Collections;

public class RandomMover3D: MonoBehaviour {
	
	public float speed = 100;
	public float speedvar = 0;
	public float range = 20;
	public float smoothing = 99f;
	private float instantspeed = 0;

	//public Color startupColor;

	 
	
	Vector3 target ;
	Vector3 smoothtarget;

	void Startup () 
	{
		//myalpha = startupColor.a;
		//Color startupColor = new Color (255, Random.Range (0, 255), 0);

	}


	void Update () 
	{
		smoothtarget = smoothtarget * smoothing/100 + target * (1 - smoothing/100);
		 
		instantspeed = speed + Random.Range (0f, speedvar);
		//print (instantspeed);
		transform.Translate(Vector3.forward * Time.deltaTime * instantspeed);
		transform.LookAt(smoothtarget);
		float distance = Vector3.Distance (transform.position, target);

		//Move to next waypoint:
		if (distance < range) 
		{
			target = new Vector3(Random.Range(-range, range), Random.Range(-range, range), Random.Range(-range, range));
			GetComponent<AudioSource>().pitch = distance * 0.5f;
			Color macouleur = Color.white;

			//macouleur.r = startupColor.r + distance * 0.1f;
			//macouleur.g = startupColor.g - distance * 0.1f;
			//macouleur.r = startupColor.r + distance * 0.1f;
			//renderer.material.color = macouleur;

		}
	}
}
