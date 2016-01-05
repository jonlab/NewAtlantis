using UnityEngine;
using System.Collections;

public class NAAudioEnvelopeADSR : MonoBehaviour {

	public AnimationCurve envelope = new AnimationCurve(new Keyframe(0, 0.0f), new Keyframe(0.1f, 1.0f), new Keyframe(1.0f, 0.0f));
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
