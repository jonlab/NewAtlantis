using UnityEngine;
using System.Collections;

public class RandomMover3D_03: MonoBehaviour {
	
	public float speed = 100;
	public float speedvar = 0;

	public float rangex = 20;
	public float rangey = 20;
	public float rangez = 20;

	public float smoothing = 99f;
	private float instantspeed = 0;
	
	//public Color startupColor;
	
	
	
	Vector3 target ;
	Vector3 smoothtarget;
	
	void Start () 
	{
		//myalpha = startupColor.a;
		//Color startupColor = new Color (255, Random.Range (0, 255), 0);
		target = new Vector3(-50f, 10f, 50f);	
	}
	
	
	void Update () 
	{
		smoothtarget = smoothtarget * smoothing/100f + target * (1f - smoothing/100f);
		
		instantspeed = speed + Random.Range (0f, speedvar);
		//print (instantspeed);
		transform.Translate(Vector3.forward * Time.deltaTime * instantspeed);
		transform.LookAt(smoothtarget);
		float distance = Vector3.Distance (transform.position, target);
		
		//Move to next waypoint:
		if (distance < rangex) 
		{
			target = new Vector3(Random.Range(-rangex, rangex), Random.Range(0, rangey), Random.Range(-rangez, rangez));
			GetComponent<AudioSource>().pitch = distance * 0.5f;
			Color macouleur = Color.white;
			
			//macouleur.r = startupColor.r + distance * 0.1f;
			//macouleur.g = startupColor.g - distance * 0.1f;
			//macouleur.r = startupColor.r + distance * 0.1f;
			//renderer.material.color = macouleur;
			
		}
	}
}
