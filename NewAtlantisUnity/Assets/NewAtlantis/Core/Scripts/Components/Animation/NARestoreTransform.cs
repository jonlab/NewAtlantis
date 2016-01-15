using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TransformState
{
	public Vector3 localPosition;
	public Vector3 localEulerAngles;
	public Vector3 localScale;

}
public class NARestoreTransform : MonoBehaviour 
{
	private Dictionary<int,TransformState> initial_states = new Dictionary<int, TransformState>();
	private float timer = 0f;
	public float interval = 10f;
	// Use this for initialization
	void Start () 
	{
		//get initial position
		if (!NA.isClient()) //server and standalone
		{
			Transform[] transforms = this.GetComponentsInChildren<Transform>();
			foreach (Transform t in transforms)
			{
				TransformState state = new TransformState();
				state.localPosition = t.localPosition;
				state.localEulerAngles = t.localEulerAngles;
				state.localScale = t.localScale;
				initial_states.Add(t.GetInstanceID(), state);
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (NA.isClient() && NA.syncMode != SyncMode.NoInDepthSync)
			return;
		
		timer += Time.deltaTime;
		if (timer > interval)
		{
			timer -= interval;
			Restore();
		}
			
	}

	void Restore()
	{
		Transform[] transforms = this.GetComponentsInChildren<Transform>();
		foreach (Transform t in transforms)
		{
			TransformState tr = initial_states[t.GetInstanceID()];
			t.localPosition 	= tr.localPosition;
			t.localEulerAngles = tr.localEulerAngles;
			t.localScale = tr.localScale;
		}
		//on reset aussi les RigidBodies
		Rigidbody[] rbs = this.GetComponentsInChildren<Rigidbody>();

		foreach (Rigidbody rb in rbs)
		{
			rb.velocity = Vector3.zero;
		}
	}
}
