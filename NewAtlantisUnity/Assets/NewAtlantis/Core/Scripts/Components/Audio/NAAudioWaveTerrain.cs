using UnityEngine;
using System.Collections;

public class NAAudioWaveTerrain : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnCollisionEnter(Collision collision) 
	{
		if (NA.isClient())
			return;

		//Debug.Log("paf");
		NAAudioSynthConvolutionLooper c = GetComponent<NAAudioSynthConvolutionLooper>();
		if (c)
		{
			Vector3 lpos = collision.contacts[0].point; //local
			//Debug.Log("pos="+lpos);
			Collider collider = GetComponent<Collider>();
			float x = (lpos.x-collider.bounds.min.x)/collider.bounds.size.x;
			float z = (lpos.z-collider.bounds.min.z)/collider.bounds.size.z;
			//Debug.Log("size="+collider.bounds.size);
			//Debug.Log("x=" + x + " z=" + z);
			c.pos1 = x;
			c.pos2 = z;
			c.duration1 = 0.05f;//Random.value*0.05f;
			c.duration2 = 0.05f;//Random.value*0.05f;
			c.Generate();
		}

	}
}
