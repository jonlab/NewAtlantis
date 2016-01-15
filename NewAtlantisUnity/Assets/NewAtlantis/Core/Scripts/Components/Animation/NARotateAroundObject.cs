using UnityEngine;
using System.Collections;

 // rotates a sound source around a GameObject
 // params: 
	// reference GameObject (center of the rotation)
	// rotation angle 3D vector (selfrotation, y [-1 1], z [-1 1])
	// Speed (tours per min. )



	
public class NARotateAroundObject : MonoBehaviour 
{
	public GameObject CentralObject;
	public Vector3 Angle3DVector;
	public float Speed = 10f;

	void Update () 
	{
				
		if (CentralObject != null)
		{
			transform.RotateAround (CentralObject.transform.position, Angle3DVector, Speed * Time.deltaTime);
		}
	
	}
		
}

