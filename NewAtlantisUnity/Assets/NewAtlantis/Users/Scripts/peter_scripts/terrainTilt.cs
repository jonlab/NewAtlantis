using UnityEngine;
using System.Collections;

public class terrainTilt : MonoBehaviour {


	public float roll_speed = 16f;
	public float pitch_speed = 12f;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 angles = transform.eulerAngles;

		angles.x += pitch_speed * Time.deltaTime;
		angles.z += roll_speed * Time.deltaTime;
		if (angles.x > 180)
			angles.x -= 360;
		if (angles.z > 180)
			angles.z -= 360;

		transform.eulerAngles = angles;
		if (pitch_speed > 0)
		{
			if (angles.x > 30)
			{
				pitch_speed *= -1;
			}
		}

		if (pitch_speed < 0)
		{
			if (angles.x < -30)
			{
				pitch_speed *= -1;
			}
		}

		if (roll_speed > 0)
		{
			if (angles.z > 30)
			{
				roll_speed *= -1;
			}
		}

		if (roll_speed < 0)
		{
			if (angles.z < -30)
			{
				roll_speed *= -1;
			}
		}


	
	}
}
