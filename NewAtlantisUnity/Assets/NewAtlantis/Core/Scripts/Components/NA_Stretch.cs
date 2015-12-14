using UnityEngine;
using System.Collections;

public class NA_Stretch : MonoBehaviour 
{
	public float range = 1;
	float val = 0f;
	// Use this for initialization
	void Start () 
	{
		//store initial state
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!Network.isClient) //server and standalone
		{
			val = rdF(range);
			if (Network.isServer)
			{
				GetComponent<NetworkView>().RPC("ApplyFX", RPCMode.All, val);
			}
			else
			{
				ApplyFX(val);
			}
		}
		else if (Network.isClient)
		{
			//nothing, the client will have his RPC called by the server
		}
	}


	[RPC]
	void ApplyFX(float _val)
	{
		//this part must be deterministic
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector3[] vertices = mesh.vertices;
		int i = 0;
		while (i < vertices.Length) 
		{
			float modX = ((_val+i)%8f)*10f;
			float modY = ((_val+i)%10f)*10f;
			float modZ = ((_val+i)%12f)*10f;
			//vertices[i] += v (0,_rdF(range),0) * Time.deltaTime;
			vertices[i] = v (modX,modY,modZ) * Time.deltaTime;
			i++;
		}
		mesh.vertices = vertices;
		mesh.RecalculateBounds();
	}

	Vector3 rdV(float r)
	{
		
		return new Vector3 (Random.Range (-r, r), Random.Range (-r, r), Random.Range (-r, r));
	}

	Vector3 v(float x,float y,float z){
		
		return new Vector3 (x,y,z);
	}

	float rdF(float r){

		return Random.Range(-r,r);
	}

}
