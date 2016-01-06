using UnityEngine;
using System.Collections;

public class testanimationradio : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		Animation animation = GetComponent<Animation>();
		foreach (AnimationState state in animation) {
			//state.speed = 0.1F;
			state.normalizedTime = 0.5f;

		}
	
	}
}
