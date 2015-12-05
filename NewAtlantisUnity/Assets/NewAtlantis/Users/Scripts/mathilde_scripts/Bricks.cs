using UnityEngine;
using System.Collections;

public class Bricks : MonoBehaviour {
	
	public GameObject brickParticle;

	public void Reset()
	{
		GetComponent<Collider> ().enabled = true;
		GetComponent<MeshRenderer> ().enabled = true;
	}
	void OnCollisionEnter (Collision other) 
	{
		if (!NA.isClient ()) {
			if (other.gameObject.name == "Ball") {
				//Instantiate(brickParticle, transform.position, Quaternion.identity);
				GM.instance.DestroyBrick ();
				//Destroy (boxCollider);
				//Destroy (meshRenderer);
				//Destroy (rigidbody);

				//Destroy(GetComponent<Collider> ());
				//Destroy (GetComponent<MeshRenderer>());
				//Destroy(GameObject,3);
				GetComponent<Collider> ().enabled = false;
				GetComponent<MeshRenderer> ().enabled = false;

				//MeshRenderer mr = gameObject.
			}
		}
	}
}