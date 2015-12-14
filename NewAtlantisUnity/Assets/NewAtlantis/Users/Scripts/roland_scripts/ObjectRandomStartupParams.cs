using UnityEngine;
using System.Collections;

public class ObjectRandomStartupParams : MonoBehaviour {
	

	public Vector3 minTransform;
	public Vector3 maxTransform;
	private float MyXPosition;
	private float MyYPosition;
	private float MyZPosition;

	
	public float MinPitch = 1f;
	public float MaxPitch = 1f;
	public float MinVelocity = 1f;
	public float MaxVelocity = 1f;


	
	// Use this for initialization
	void Start () {
		
		GetComponent<AudioSource>().pitch = Random.Range (MinPitch, MaxPitch);
		GetComponent<AudioSource>().volume = Random.Range (MinVelocity, MaxVelocity);

		//renderer.material.color.r = Random.Range (0f, 256f);


		MyXPosition = Random.Range (minTransform.x, maxTransform.x);
		MyYPosition = Random.Range (minTransform.y, maxTransform.y);
		MyZPosition = Random.Range (minTransform.z, maxTransform.z);
		Vector3 p = transform.position;

		p.x = MyXPosition;
		p.y = MyYPosition;
		p.z = MyZPosition;
		transform.position = p;
	}

}
