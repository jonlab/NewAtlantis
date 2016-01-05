using UnityEngine;
using System.Collections;

public class FaireAvancerLaBalle : MonoBehaviour {

	public KeyCode up;
	public KeyCode down;
	public KeyCode left;
	public KeyCode right;
	public KeyCode jump;

	public float puissancejump = 100;
	public float force = 3.0f;
	int compteur = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//je recupere le rigidbody
		Rigidbody rg = transform.GetComponent<Rigidbody>();
		Vector3 vCamToBall = transform.position - Camera.main.transform.position;
		vCamToBall.Normalize ();
		Vector3 vCamToBallRight = Quaternion.AngleAxis (-90, Vector3.up) * vCamToBall;
		// quand j'appuie sur la touche "devant"
		if (Input.GetKey (up)) {

			//Vector3 direction = transform.right;
			Vector3 direction = vCamToBall;			
			rg.AddForce(direction * force);

		}

		if (Input.GetKey (down)) {
			
			//Vector3 direction = transform.right;
			Vector3 direction = vCamToBall*-1;		
			rg.AddForce(direction * force);
			
		}

		if (Input.GetKey (left)) {
			
			//Vector3 direction = transform.right;
			Vector3 direction = vCamToBallRight;			
			rg.AddForce(direction * force);
			
		}

		if (Input.GetKey (right)) {
			
			//Vector3 direction = transform.right;
			Vector3 direction = vCamToBallRight*-1;				
			rg.AddForce(direction * force);
			
		}

		if (Input.GetKeyDown (jump)&& compteur > 30) {

			//Vector3 direction = transform.up;	
		 rg.AddForce(Vector3.up*puissancejump); 
			compteur = 0;
		}
		compteur++;






	}
}
