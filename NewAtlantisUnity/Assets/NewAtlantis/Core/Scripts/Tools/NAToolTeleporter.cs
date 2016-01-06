using UnityEngine;
using System.Collections;

public class NAToolTeleporter : NAToolBase 
{
    public GameObject   Target          = null;         //GameObject to teleport
    public Vector3      WorldPosition   = Vector3.zero; //teleport world position
	
    // Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

	public override void Action() 
	{
        Debug.Log ("NAToolTeleporter action");
		if (Target == null)
            transform.position = WorldPosition;
        else
            Target.transform.position = WorldPosition;
	}
}
