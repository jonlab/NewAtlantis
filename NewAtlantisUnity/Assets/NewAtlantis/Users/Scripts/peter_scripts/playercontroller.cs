 using UnityEngine;
using System.Collections;

public class playercontroller : MonoBehaviour 
{ 
	public float speed; 
	void FixedUpdate ()

	{ 
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		GetComponent<Rigidbody>().AddForce (movement * speed * Time.deltaTime);  
		float vitesse = GetComponent<Rigidbody>().velocity.magnitude;
		GetComponent<AudioSource>().pitch = vitesse * 1;
		//Debug.Log (vitesse);
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "PickUp") 
		{
			other.gameObject.SetActive (false);
		}
	}
}
