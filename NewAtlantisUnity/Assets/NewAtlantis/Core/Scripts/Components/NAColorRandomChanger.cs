using UnityEngine;
using System.Collections;
public class NAColorRandomChanger : MonoBehaviour 
{
	public float 	interval 	= 1;
    public float 	timer 		= 0f;
	public Vector3 	color 		= Vector3.one;
	
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!NA.isClient()) //server and standalone
		{
            timer += Time.deltaTime;
            if (timer > interval)
            {
                timer -= interval;
				color = new Vector3(Random.value, Random.value, Random.value);
				if (NA.isServer())
				{
					GetComponent<NetworkView>().RPC("Apply", RPCMode.All, color);
				}
				else if (NA.isStandalone())
				{
					Apply(color);
				}
            }
		}
		else
		{
			//nothing, the client will have his RPC called by the server
		}
	}

	[RPC]
	void Apply(Vector3 _color)
	{
		//this part must be deterministic
		MeshRenderer renderer = GetComponent<MeshRenderer>();
		if (renderer != null)
		{
			renderer.material.color = new Color(_color.x, _color.y, _color.z);
		}
	}
}
