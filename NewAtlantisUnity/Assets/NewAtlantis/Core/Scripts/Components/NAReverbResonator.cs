using UnityEngine;
using System.Collections;
using System.Collections.Generic;



//put this script on the resonnant space (needs a trigger collider and an AudioReverbFilter)
public class NAReverbResonator : MonoBehaviour 
{

	/*public  float decayHFRatio;
	public float decayTime;
	public float density;	
	public float diffusion;
	public float dryLevel;
	public float hfReference;
	public float lFReference;
	public float reflectionsDelay;
	public float reflectionsLevel;
	public float reverbDelay;
	public float reverbLevel;
	*/
	public AudioReverbPreset reverbPreset;
	/*public float room;	
	public float roomHF;
	public float roomLF;
	public float roomRolloff;
	*/

	// Use this for initialization
	void Start () 
	{
		//we use the AudioReverbZone only as a state container
		AudioReverbZone arz = GetComponent<AudioReverbZone> ();
		if (arz != null)
			arz.enabled = false;

	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void OnTriggerEnter(Collider collider)
	{
		//Debug.Log ("OnTriggerEnter " + collider.gameObject.name);
		//LogManager.Log ("OnTriggerEnter " + collider.gameObject.name);
		//activate effect
		NAReverbEffector e = collider.gameObject.GetComponent<NAReverbEffector> ();
		if (e)
			e.AddResonator (this);
	}

	void OnTriggerExit(Collider collider)
	{
		//Debug.Log ("OnTriggerExit " + collider.gameObject.name);
		//LogManager.Log ("OnTriggerExit " + collider.gameObject.name);
		//unactivate effect
		NAReverbEffector e = collider.gameObject.GetComponent<NAReverbEffector> ();
		if (e)
			e.RemoveResonator (this);
	}


}
