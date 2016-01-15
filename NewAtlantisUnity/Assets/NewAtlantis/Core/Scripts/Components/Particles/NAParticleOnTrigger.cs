using UnityEngine;
using System.Collections;

public class NAParticleOnTrigger : MonoBehaviour 
{
	ParticleSystem p;
	public GameObject target;
	Rigidbody rg;

	// Use this for initialization
	void Start () 
	{
		if (target == null)
			target = this.gameObject;


		p = target.GetComponent<ParticleSystem> ();

		if (p == null) 
		{
			target.gameObject.AddComponent<ParticleSystem>();
			p = target.GetComponent<ParticleSystem> ();
			p.Stop ();
		}
		rg = target.GetComponent<Rigidbody> ();
		if (rg == null) 
		{
			target.gameObject.AddComponent<Rigidbody> ();
			rg = target.GetComponent<Rigidbody> ();
			rg.isKinematic = true;
			rg.useGravity = false;

		} 
		else 
		{
			rg.isKinematic = true;
			rg.useGravity = false;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnTriggerEnter(Collider collider) 
	{
		if (NA.isClient() && NA.syncMode != SyncMode.NoInDepthSync)
			return;
		ParticlePlay();
		/*
		if (!NA.isClient ()) 
		{
			if (NA.isServer())
			{
				GetComponent<NetworkView>().RPC("ParticlePlay", RPCMode.All);
			}
			else if (NA.isStandalone())
			{
				ParticlePlay();
			}
		}
		*/
	}

	void OnTriggerExit (Collider other) 
	{

		if (NA.isClient() && NA.syncMode != SyncMode.NoInDepthSync)
			return;
		ParticleStop();
		/*
		if (!NA.isClient ()) 
		{
			if (NA.isServer())
			{
				GetComponent<NetworkView>().RPC("ParticleStop", RPCMode.All);
			}
			else if (NA.isStandalone())
            {
				ParticleStop();
            }
        }
        */

	}



	[RPC]
	void ParticlePlay()
	{
		p.Play ();
	}

	[RPC]
	void ParticleStop()
	{
		p.Stop ();
    }


}
