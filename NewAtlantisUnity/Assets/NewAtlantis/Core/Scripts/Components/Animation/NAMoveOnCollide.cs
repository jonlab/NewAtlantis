using UnityEngine;
using System.Collections;

public class NAMoveOnCollide : MonoBehaviour 
{
	public GameObject TargetObject = null;
	public Vector3 localdirection = new Vector3(0,0,1);
	public float MinTime = 1;
	public float MaxTime = 1;
	public float Speed = 1;

	private bool moving = false;
	private float CurrentTime = 0;

	private float timer = 0f;

	// Use this for initialization
	void Start () 
	{
		//localdirection.Normalize();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (moving && (NA.isServer() || NA.isStandalone()))
		{
			timer += Time.deltaTime;
			GameObject go = TargetObject != null ? TargetObject : gameObject;

			Vector3 localposition = go.transform.localPosition;
			localposition += localdirection*Speed*Time.deltaTime;
			go.transform.localPosition = localposition;

			if (timer >= CurrentTime)
			{
				moving = false;
			}
		}
	}

	void OnCollisionEnter(Collision collision) 
	{
		if (!moving)
		{
			//let's move
			moving = true;
			timer = 0f;
			CurrentTime = MinTime+(MaxTime-MinTime)*Random.value;
		}
	}
}
