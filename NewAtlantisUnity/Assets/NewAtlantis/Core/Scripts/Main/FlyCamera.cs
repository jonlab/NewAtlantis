using UnityEngine;
using System.Collections;

public class FlyCamera : MonoBehaviour {

	Vector3 previousmouseposition;
	private bool bPhysics = true;
	private bool bGravity = false;

	public float timerFly = 0;
	// Use this for initialization
	void Start () 
	{

	
	}

	void OnCollisionEnter(Collision collision) 
	{
		/*if (!bGravity)
		{
			bGravity = true;
			LogManager.LogWarning("you are now in walk mode");
		}*/
	}


	// Update is called once per frame
	void Update () 
	{

		if (NAInput.GetControl(NAControl.NextTool) || NAInput.GetControl(NAControl.PreviousTool))
		{
			return;
		}
		float speed = 2f;
		if (Input.GetKey (KeyCode.LeftShift)) 
		{
						speed = 20f;
		}
		float dt = Time.deltaTime;
		/*
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			//transform.position -= transform.right * speed * dt;
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			//transform.position += transform.right * speed * dt;
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			//transform.position += transform.forward * speed * dt;
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			//transform.position -= transform.forward * speed * dt;
		}
		if (Input.GetKey (KeyCode.RightShift)) 
		{
			//transform.position += transform.up * speed * dt;
		}
		*/

		//axes

		float x = NAInput.GetAxis(NAControl.MoveHorizontal);
		float y = NAInput.GetAxis(NAControl.MoveVertical);

		float joy_speed = 4f;
		if (NAInput.GetControl(NAControl.Jump))
			joy_speed = 20f;
		if (Input.GetKeyDown(KeyCode.W))
		{
			bGravity = !bGravity;
		}


		if (NAInput.GetControl(NAControl.Jump) && x == 0 && y == 0 )
		{
			timerFly += Time.deltaTime;

			if (timerFly > 1f)
			{
				timerFly = 0f;
				bGravity = !bGravity;
				if (bGravity)
					LogManager.LogWarning("you are now in walk mode");
				else
					LogManager.LogWarning("you are now in fly mode");
			}
		}
		else
		{
			timerFly = 0;
		}

		bool jump = NAInput.GetControlUp(NAControl.Jump);






		if (bPhysics)
		{
			Rigidbody rb = GetComponent<Rigidbody>();
			float coeff = Time.deltaTime*60f*5f;
			rb.AddForce(transform.right * joy_speed *x *coeff, ForceMode.Force);
			rb.AddForce(transform.forward * joy_speed *y*coeff, ForceMode.Force);
			//rb.velocity = transform.right * joy_speed * dt*x *100+transform.forward * joy_speed * dt*y*100;
			//transform.position += transform.right * joy_speed * dt*x;
			//transform.position += transform.forward * joy_speed * dt*y;

			if (jump)
			{
				//question do use world up or local up ?
				//mix 
				Vector3 jumpup = (transform.up+Vector3.up)/2f;
				rb.AddForce(jumpup * joy_speed *100, ForceMode.Force);
			}

			/*if (rb.velocity.magnitude>1)
			{
				rb.velocity.Normalize();
			}
			*/


			rb.useGravity = bGravity;
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
