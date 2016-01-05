using UnityEngine;
using System.Collections;

public class sm_moon : MonoBehaviour {

	public AudioClip mysound;
	bool on = false;
	float angle = -3.14f;
	//float angleOffset = 0.5f;
	
	void Start () {
		if (GetComponent<AudioSource>())
			GetComponent<AudioSource>().Stop ();
		GameObject parent = GameObject.Find ("Vahalla");
		transform.parent = parent.transform;
		
	}
	void OnMouseEnter()
	{
		on = !on;
		if (on)
			
			GetComponent<AudioSource>().Play ();
		
		else
			
			GetComponent<AudioSource>().Stop ();
	}
	void Update () 
	{

		float speed = 0.14f;//speed of rotation
		angle -= Time.deltaTime * speed; // position on the elipse in rads (- is for counterclockwise)

		float x = Mathf.Cos (angle) * 0.6f;
		float z = Mathf.Sin (angle) * 1.0f;
		Vector3 offset = new Vector3 (-0.05f, 0, 0.05f); //positions it slightly off-center (0, 0, 0,= centered)
		float y = Mathf.Sin (angle) * 0.2f; // changes y for a 3D elipse
		transform.localPosition = new Vector3 (x, y, z) + offset; // sets new local position
		//Vector3 centerrotation = transform.parent.position;
		//transform.RotateAround(centerrotation, new Vector3(0.4f, 1.25f,0f), -Time.deltaTime * -4.0f);


	}	
}


