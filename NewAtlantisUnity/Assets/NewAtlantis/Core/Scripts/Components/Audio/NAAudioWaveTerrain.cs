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
			Vector3 pos = collision.contacts[0].point;
			Vector3 lpos = transform.InverseTransformPoint(pos);

			//Debug.Log("pos="+lpos);
			Collider collider = GetComponent<Collider>();
			float x = Mathf.Abs((lpos.x / collider.bounds.extents.x) / 2f + 0.5f);
			float z = Mathf.Abs((lpos.z / collider.bounds.extents.z) / 2f + 0.5f);
			Debug.Log("x=" + x + " z=" + z);
			c.pos1 = x;
			c.pos2 = z;
			c.duration1 = 0.04f;//Random.value*0.05f;
			c.duration2 = 0.04f;//Random.value*0.05f;
			c.Generate();
		}

	}
}
