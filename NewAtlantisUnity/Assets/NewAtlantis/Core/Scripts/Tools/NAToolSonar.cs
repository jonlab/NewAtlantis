using UnityEngine;
using System.Collections;

public class NAToolSonar : NAToolBase 
{
	public GameObject sonarObject;
	public float MaximumDistance = 100f;
	public float Speed = 1f;
	private bool Active = false;
	// Use this for initialization
	void Start () 
	{
		sonarObject.transform.localScale = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Active && sonarObject != null)
		{
			sonarObject.transform.localScale += Vector3.one * Speed * Time.deltaTime;
			if (sonarObject.transform.localScale.magnitude > MaximumDistance)
			{
				Active = false;
				sonarObject.transform.localScale = Vector3.zero;
			}
		}
	}

	public override void Action() 
	{
		Debug.Log ("NAToolSonar action");
		Active = !Active;

		sonarObject.transform.localScale = Vector3.zero;
		//sonarObject.transform.localPosition = Vector3.zero;
		sonarObject.transform.position = transform.position;
	}
}
