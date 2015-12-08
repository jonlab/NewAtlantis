using UnityEngine;
using System.Collections;

public class FlyCamera : MonoBehaviour {

	Vector3 previousmouseposition;
	public bool bPhysics = true;
	// Use this for initialization
	void Start () 
	{

	
	}
	
	// Update is called once per frame
	void Update () 
	{
		float speed = 2f;
		if (Input.GetKey (KeyCode.LeftShift)) 
		{
						speed = 20f;
		}
		float dt = Time.deltaTime;
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			transform.position -= transform.right * speed * dt;
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			transform.position += transform.right * speed * dt;
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			transform.position += transform.forward * speed * dt;
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			transform.position -= transform.forward * speed * dt;
		}
		if (Input.GetKey (KeyCode.RightShift)) 
		{
			transform.position += transform.up * speed * dt;
		}

		//axes
		float joy_speed = 4f;
		if (NAInput.GetControl(NAControl.Jump))
			joy_speed = 20f;

		bool jump = NAInput.GetControlUp(NAControl.Jump);

		float x = NAInput.GetAxis(NAControl.MoveHorizontal);
		float y = NAInput.GetAxis(NAControl.MoveVertical);

		if (bPhysics)
		{
			Rigidbody rb = GetComponent<Rigidbody>();
			rb.AddForce(transform.right * joy_speed * dt*x *100);
			rb.AddForce(transform.forward * joy_speed * dt*y*100);
			//transform.position += transform.right * joy_speed * dt*x;
			//transform.position += transform.forward * joy_speed * dt*y;

			if (jump)
			{
				rb.AddForce(transform.up * joy_speed *100);
			}
		}
		else
		{
			transform.position += transform.right * joy_speed * dt*x;
			transform.position += transform.forward * joy_speed * dt*y;
		}

		/*float joy_speed = 50f;
		float y = Input.GetAxis("Vertical");
		float x = Input.GetAxis("Horizontal");
		transform.position += transform.right * joy_speed * dt*x;
		transform.position += transform.forward * joy_speed * dt*y;
		*/

		/*if (Input.GetMouseButtonDown (0)) 
		{
			previousmouseposition = Input.mousePosition;

		}
		else if (Input.GetMouseButton(0))
		{
			Vector3 move = Input.mousePosition - previousmouseposition;
			previousmouseposition = Input.mousePosition;
			transform.Rotate(transform.up, move.x);
			transform.Rotate(transform.right, move.y*-1f);

		}
		*/



		/*float vx = Input.GetAxis("ViewX");
		float vy = Input.GetAxis("ViewY");

		Vector3 angles = transform.eulerAngles;
		angles.x += vy;
		angles.y += vx;
		transform.eulerAngles = angles;
		*/
	
	}
}
