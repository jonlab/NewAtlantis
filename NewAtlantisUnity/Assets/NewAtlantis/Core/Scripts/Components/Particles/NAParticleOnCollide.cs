using UnityEngine;
using System.Collections;

public class NAParticleOnCollide : MonoBehaviour {
	ParticleSystem p;

	public GameObject target;
	Rigidbody rg;
	public Color c;
	//Particle p;
	// Use this for initialization
	void Start () {
	

		if (target == null)
			target = this.gameObject;


		p = target.GetComponent<ParticleSystem> ();

		if (p == null) {

			target.gameObject.AddComponent<ParticleSystem>();
			p = target.GetComponent<ParticleSystem> ();
			p.Stop ();
		//	p.startColor = c;

		}

		rg = target.GetComponent<Rigidbody> ();
		if (rg == null) {


			target.gameObject.AddComponent<Rigidbody> ();
			rg = target.GetComponent<Rigidbody> ();
			rg.isKinematic = true;
			rg.useGravity = false;

		} else {

			rg.isKinematic = true;
			rg.useGravity = false;
		}


		/* a supprimer */

	

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision e){


		if (NA.isClient() && NA.syncMode != SyncMode.NoInDepthSync)
			return;
		Apply();
		/*
		if (!NA.isClient ()) {
	

			if (NA.isServer())
				{
					GetComponent<NetworkView>().RPC("Apply", RPCMode.All);
				}
				else if (NA.isStandalone())
				{
					Apply();
				}
				//p.Play ();
			}
			*/




	}


	[RPC]
	void Apply()
	{
		//p.startColor = c;
		p.Play ();
	
	}


}
