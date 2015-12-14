using UnityEngine;
using System.Collections;

public class NAElipseMover : MonoBehaviour {
	
		public float speed = 3f;//speed of rotation
		public Vector3 range = new Vector3(10f, 10f, 10f);
		Vector3 startupPosition;

		float angle = -3.14116f;

		void Start()
		{
			startupPosition = transform.position;
		}
		
		void Update () 
		{
			angle -= Time.deltaTime * speed; // position on the elipse in rads (- is for counterclockwise)
			
		float x = Mathf.Cos (angle) * range.x;
		float z = Mathf.Sin (angle) * range.z;
		float y = Mathf.Sin (angle) * range.y; // changes y for a 3D elipse
	
		transform.localPosition = new Vector3 (x, y, z); // sets new local position

		}	
	}