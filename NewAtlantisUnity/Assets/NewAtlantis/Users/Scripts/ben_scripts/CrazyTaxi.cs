using UnityEngine;
using System.Collections;

public class CrazyTaxi : MonoBehaviour {

	public float speed=1.0f;

	public float turnProbability = 1.0f; 

	public float maxTurnRadius = 90.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!NA.isClient())
		{
			float r = Random.value;
			if (r < turnProbability / 100.0f)
			{
				// do random rotation

				float angle = Random.value  * (maxTurnRadius*2) - maxTurnRadius;

				transform.Rotate(0,angle,0);
			}
			transform.Translate(Vector3.forward * speed * Time.deltaTime);
		}
	}
}
