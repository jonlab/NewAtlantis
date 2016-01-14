using UnityEngine;
using System.Collections;

public class NASpaceTeleportOnTrigger : MonoBehaviour 
{	
	public int 		SpaceId 	= 0;
	public Vector3 	TargetPosition 	= Vector3.zero;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnTriggerEnter(Collider collider) 
	{
		if (NA.isClient())
			return; //only the server and standalone is able to teleport everybody
		//FIXME : if this is an avatar with appropriate rights to do that
		Debug.Log ("NASpaceTeleportOnTrigger OnTriggerEnter");
		if (SpaceId == -1) //means "back"
		{
			if (NA.PreviousSpace != null)
			{
				NA.app.GoToSpace(NA.PreviousSpace.id);
			}
		}
		else
		{
			NA.app.GoToSpace(SpaceId);
		}

		collider.gameObject.transform.position = TargetPosition;
	}
}
